using Application.Models;
using Application.Shared.Enums;
using Application.Shared.Models.Data;
using Application.Shared.Models.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Services.Data;

public interface ITableService
{

    Task<Table> CreateTableAsync(Table table);


    Task<Response<List<Table>>> GetTablesAsync(string userId, string? expand = null,
                                                   int pageSize = 1000,
                                                   int page = 0,
                                                   Table? filter = null,
                                                   string? orderBy = null,
                                                   SortDirection orderDirection = SortDirection.Ascending);

    Task<Table?> GetTableByIdAsync(string ipAddress, string database, string name);

    Task<bool> UpdateTableAsync(Table table);

    Task<bool> DeleteTableAsync(string ipAddress, string database, string name);



}
