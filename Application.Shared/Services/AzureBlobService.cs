using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System.Data;
using Parquet.Schema;
using Parquet;
using Microsoft.FluentUI.AspNetCore.Components;
using Azure.Core;
using Azure.Storage.Blobs.Models;
using Application.Shared.Models.Data;
using Azure;
using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace Application.Shared.Services;


public class AzureBlobService : IAzureBlobService
{
    private readonly string _connectionString;
    private readonly string _containerName;
    private readonly IToastService _toastService;

    public AzureBlobService(IConfiguration configuration)
    {
        _connectionString = configuration["AzureBlob:ConnectionString"]
            ?? throw new Exception("AzureBlob:ConnectionString not found in configuration.");
        _containerName = configuration["AzureBlob:ContainerName"]
            ?? throw new Exception("AzureBlob:ContainerName not found in configuration.");

        _toastService = new ToastService();
    }

    /// <summary>
    /// Lists all blobs (file names) in the given folder (prefix) inside the "user-data" container.
    /// </summary>
    /// <param name="folderName">The virtual folder (e.g., user ID) under which the files reside.</param>
    /// <returns>A list of blob names (full paths including the folder prefix).</returns>
    public async Task<List<DataFile>> ListFilesInFolderAsync(string folderName)
    {
        // 1. Create a client pointing to the "user-data" container
        var containerClient = new BlobContainerClient(_connectionString, _containerName);

        // 2. Ensure container exists
        await containerClient.CreateIfNotExistsAsync();

        // 3. Prepare a list to hold blob info (filename + tags)
        var filesWithTags = new List<DataFile>();

        // 4. The prefix is typically "folderName/" to list all blobs in that directory
        var prefix = folderName.EndsWith("/") ? folderName : folderName + "/";

        // 5. Enumerate all blobs with the given prefix
        await foreach (var blobItem in containerClient.GetBlobsAsync(prefix: prefix))
        {
            // blobItem.Name is the "full path" of the blob in the container

            // Get a reference to the blob client
            var blobClient = containerClient.GetBlobClient(blobItem.Name);

            // 2. Fetch the blob properties (which include metadata)
            var propertiesResponse = await blobClient.GetPropertiesAsync();
            BlobProperties properties = propertiesResponse.Value;

            Console.WriteLine($"blobItem.Name: {properties.Metadata.Count()}");

            // 3. The metadata is here as an IDictionary<string, string>
            IDictionary<string, string> metadata = properties.Metadata;


            // remove the directory prefix from the blob name
            var fileName = blobItem.Name.Substring(prefix.Length);

            // remove extension from file name
            //fileName = fileName.Substring(0, fileName.LastIndexOf('.'));

            DataFile file = new DataFile()
            {
                FileName = fileName
            };

            file.SetTags(metadata);
        }

        return filesWithTags;
    }



    public async Task<Response<BlobContentInfo>> ProcessAndStoreFileToBlobAsync(DataFile file)
    {

        _toastService.ShowInfo("Processing and storing file to Azure Blob Storage...");
        var dataTable = ParseCsvToDataTable(file.Content);

        var parquetStream = await DataTableToParquetStream(dataTable);


        // Create or get reference to the container
        var containerClient = new BlobContainerClient(_connectionString, _containerName);
        await containerClient.CreateIfNotExistsAsync();

        //var path = $"{blobName}/{tableName}";

        

        var fileName = file.Directory + "/" + file.FileName + ".parquet";

        // Get a reference to the blob
        var blobClient = containerClient.GetBlobClient(fileName);

        // Upload the stream to Azure (overwrites if it already exists)
        parquetStream.Position = 0; // Ensure stream starts at the beginning
        var tags = new Dictionary<string, string>
        {
            { "fileType", "parquet" },
            { "uploadedBy", "user" }
        };

        //await blobClient.UploadAsync(parquetStream, overwrite: true);

        // Upload with tags in one step
        var blobContentInfo = await blobClient.UploadAsync(
            parquetStream,
            new BlobUploadOptions
            {
                Metadata = tags,
                
            }
        );

        //// check if file was uploaded successfully
        if (blobContentInfo.GetRawResponse().Status == 201)
        {
            _toastService.ShowSuccess("File uploaded successfully.");
        }
        else
        {
            return null;
        }

        return blobContentInfo;


        



    }




    // write a function that reads parquet file from azure blob storage and returns it as a list of dictionaries
    public async Task<List<Dictionary<string, object>>> ReadParquetFromBlobAsync(DataFile dataFile)
    {

        int pageNumber = 1; 
            
        int pageSize = 100;

        // Create a BlobServiceClient
        var blobServiceClient = new BlobServiceClient(_connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

        var path = dataFile.Directory + "/" + dataFile.FileName + ".parquet";
        var blobClient = containerClient.GetBlobClient(path);

        // Download the blob to a stream
        using (var memoryStream = new MemoryStream())
        {
            await blobClient.DownloadToAsync(memoryStream);
            memoryStream.Position = 0; // Reset the stream position

            // Read the Parquet file
            using (var parquetReader = await ParquetReader.CreateAsync(memoryStream))
            {
                var result = new List<Dictionary<string, object>>();

                // Calculate the total number of rows
                int totalRowCount = parquetReader.RowGroupCount > 0 ? parquetReader.ReadEntireRowGroupAsync(0).Result[0].Data.Length : 0;

                // Calculate the start and end indices for pagination
                int startIndex = (pageNumber - 1) * pageSize;
                int endIndex = Math.Min(startIndex + pageSize, totalRowCount);

                // Ensure indices are within bounds
                if (startIndex >= totalRowCount || startIndex < 0)
                {
                    return result; // Return an empty result if the page is out of range
                }

                // Read the rows for the requested page
                for (int i = 0; i < parquetReader.RowGroupCount; i++)
                {
                    var rowGroup = await parquetReader.ReadEntireRowGroupAsync(i);
                    var columns = rowGroup;

                    for (int j = startIndex; j < endIndex; j++)
                    {
                        var row = new Dictionary<string, object>();

                        foreach (var column in columns)
                        {
                            var columnData = column.Data.GetValue(j);
                            row[column.Field.Name] = columnData;
                        }

                        result.Add(row);
                    }
                }

                return result;
            }
        }
    }


    public async Task<string> ConvertParquetToCsvAsync(DataFile dataFile)
    {
        // Create a BlobServiceClient
        var blobServiceClient = new BlobServiceClient(_connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_containerName);

        var path = dataFile.Directory + "/" + dataFile.FileName + ".parquet";
        var blobClient = containerClient.GetBlobClient(path);


        var tempFilePath = Path.Combine(Path.GetTempPath(), $"{Path.GetFileNameWithoutExtension(path)}.csv");

        using var writer = new StreamWriter(tempFilePath);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);


        using (var memoryStream = new MemoryStream())
        {
            await blobClient.DownloadToAsync(memoryStream);
            memoryStream.Position = 0; // Reset the stream position

            // Read the Parquet file
            using (var parquetReader = await ParquetReader.CreateAsync(memoryStream))
            {
                // save the data in csv

                var rowGroup = await parquetReader.ReadEntireRowGroupAsync(0);

                foreach (var column in rowGroup)
                {
                    csv.WriteField(column.Field.Name);
                }

                csv.NextRecord();

                for (int i = 0; i < rowGroup[0].Data.Length; i++)
                {
                    foreach (var column in rowGroup)
                    {
                        csv.WriteField(column.Data.GetValue(i));
                    }
                    csv.NextRecord();

                }
            }

            
        }

        await writer.FlushAsync();
        return Path.GetFileName(tempFilePath);

    }







    private async Task<Stream> DataTableToParquetStream(DataTable table)
    {
        var parquetStream = new MemoryStream();

        // 1. Build the schema
        //    Since your DataTable columns are strings, we'll treat everything as a string field.
        // 1. Build the schema
        var fields = new List<Field>();
        foreach (System.Data.DataColumn col in table.Columns)
        {
            // each column is strings
            fields.Add(new DataField<string>(col.ColumnName));
        }
        var schema = new ParquetSchema(fields);

        // 2. Write data into the Parquet file
        using (ParquetWriter parquetWriter = await ParquetWriter.CreateAsync(schema, parquetStream))
        {
            using (var rowGroupWriter = parquetWriter.CreateRowGroup())
            {
                // For each column in the DataTable, get the *existing* field from the schema
                foreach (System.Data.DataColumn column in table.Columns)
                {
                    var columnValues = new string[table.Rows.Count];
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        columnValues[i] = table.Rows[i][column].ToString() ?? string.Empty;
                    }

                    // Grab the existing field from the schema by matching name
                    var dataField = (DataField<string>)schema.DataFields
                        .Single(f => f.Name == column.ColumnName);

                    // Now use that field instance to write data
                    await rowGroupWriter.WriteColumnAsync(new Parquet.Data.DataColumn(dataField, columnValues));
                }
            }
        }


        // Reset to the beginning of the stream so it can be read later
        parquetStream.Position = 0;
        return parquetStream;
    }


    private DataTable ParseCsvToDataTable(string csvContent)
    {
        // TODO: Add condition to convert " 1,517.48 " to 1517.48
        // Those are the cases when the user did not convert the field to number

        var dataTable = new DataTable();
        using var reader = new StringReader(csvContent);
        string? line;
        bool isHeader = true;

        while ((line = reader.ReadLine()) != null)
        {
            var values = line.Split(',');

            if (isHeader)
            {
                foreach (var header in values)
                {
                    dataTable.Columns.Add(header.Trim(), typeof(string)); // Initializing as string, can infer types later
                }
                isHeader = false;
            }
            else
            {
                //dataTable.Rows.Add(values);

                if (values.Length == dataTable.Columns.Count)
                {
                    dataTable.Rows.Add(values);
                }
                else
                {
                    // TODO: Handle the mismatch, e.g., log an error or skip the row
                    Console.WriteLine(values[0]);
                    Console.WriteLine("Row has a different number of columns than the header.");
                }
            }
        }

        return dataTable;
    }


    private string GenerateFileName(string fileName)
    {
        // Sanitize and generate a unique table name, e.g., using GUID
        var sanitizedFileName = Path.GetFileNameWithoutExtension(fileName).Replace(" ", "_");
        return $"{sanitizedFileName}_{Guid.NewGuid().ToString("N")}.parquet";
    }

}
