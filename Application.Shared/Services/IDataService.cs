using Application.Shared.Models.BYOD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Services;

public interface IDataService
{
    Task ProcessAndStoreFileAsync(string fileName, string content);
    //Task<List<UserUploadedTable>> GetUserTablesAsync();
    //Task<DataTable> GetTableDataAsync(string tableName);
}
