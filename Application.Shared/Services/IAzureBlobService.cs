using Application.Shared.Models.Data;
using Azure;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Services;

public interface IAzureBlobService
{

    Task<Response<BlobContentInfo>> ProcessAndStoreFileToBlobAsync(DataFile file);
    Task<List<DataFile>> ListFilesInFolderAsync(string folderName);
    Task<List<Dictionary<string, object>>> ReadParquetFromBlobAsync(DataFile dataFile);

    Task<string> ConvertParquetToCsvAsync(DataFile dataFile);

}
