using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Net.Http.Json;

namespace Application.Shared.Services;

public class DataService : IDataService
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    private readonly HttpClient _client;

    public DataService(IConfiguration configuration, HttpClient client)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
        _client = client;
    }

    public async Task ProcessAndStoreFileAsync(string fileName, string content)
    {
        var dataTable = ParseCsvToDataTable(content);
        var tableName = GenerateTableName(fileName);
        string[] columns = dataTable.Columns.Cast<DataColumn>().Select(col => col.ColumnName).ToArray();

        // first get the columns that are duplicated
        var duplicateColumns = columns.GroupBy(x => x)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();

        // if there are duplicates, rename them. add ud_ prefix
        if (duplicateColumns.Count > 0)
        {
            for (int i = 0; i < columns.Length; i++)
            {
                if (duplicateColumns.Contains(columns[i]))
                {
                    columns[i] = "ud_" + columns[i];
                }
            }
        }


        // convert dataTable to List<Dictionary<string, object>>
        var data = new List<Dictionary<string, object>>();
        foreach (DataRow row in dataTable.Rows)
        {
            var dict = new Dictionary<string, object>();
            foreach (DataColumn col in dataTable.Columns)
            {
                dict[col.ColumnName] = row[col];
            }
            data.Add(dict);
        }


        var tableCreateRequest = await _client.PostAsJsonAsync($"api/userData/tables/create/{tableName}", columns);
        Console.WriteLine("---------------- Table Created ----------------");

        var tableInsertRequest = await _client.PostAsJsonAsync($"api/userData/tables/insert/{tableName}", data);
        Console.WriteLine("---------------- Data Inserted ----------------");


        // Optionally, store metadata about the table
        //await _client.PostAsJsonAsync($"api/userData/tables/metadata/{tableName}", dataTable);
        //await StoreMetadataAsync(tableName, fileName);
    }






    private DataTable ParseCsvToDataTable(string csvContent)
    {
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
                dataTable.Rows.Add(values);
            }
        }

        return dataTable;
    }

    private string GenerateTableName(string fileName)
    {
        // Sanitize and generate a unique table name, e.g., using GUID
        var sanitizedFileName = Path.GetFileNameWithoutExtension(fileName).Replace(" ", "_");
        return $"[ud].[UserData_{sanitizedFileName}_{Guid.NewGuid().ToString("N")}]";
    }


    








    private async Task InsertDataBulkAsync(string tableName, DataTable dataTable)
    {
        using var bulkCopy = new SqlBulkCopy(_connectionString)
        {
            DestinationTableName = tableName
        };

        await bulkCopy.WriteToServerAsync(dataTable);
    }


    //private async Task StoreMetadataAsync(string tableName, string originalFileName) //, string userId
    //{

    //    var insertMetadataQuery = "INSERT INTO UserUploadedTables (UserId, TableName, OriginalFileName) VALUES (@UserId, @TableName, @OriginalFileName)";

    //    using var connection = new SqlConnection(_connectionString);
    //    using var command = new SqlCommand(insertMetadataQuery, connection);
    //    //command.Parameters.AddWithValue("@UserId", userId);
    //    command.Parameters.AddWithValue("@TableName", tableName);
    //    command.Parameters.AddWithValue("@OriginalFileName", originalFileName);

    //    await connection.OpenAsync();
    //    await command.ExecuteNonQueryAsync();
    //}
}