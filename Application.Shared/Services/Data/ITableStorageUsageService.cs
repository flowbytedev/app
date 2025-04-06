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

public interface ITableStorageUsageService
{

    Task<TableStorageUsage> CreateTableStorageUsageAsync(TableStorageUsage tableStorageUsage);


    Task<Response<List<TableStorageUsage>>> GetTableStorageUsagesAsync(string userId, string? expand = null,
                                                   int pageSize = 1000,
                                                   int page = 0,
                                                   TableStorageUsage? filter = null,
                                                   string? orderBy = null,
                                                   SortDirection orderDirection = SortDirection.Ascending);

    Task<TableStorageUsage?> GetTableStorageUsageByIdAsync(string ipAddress, string database, string table);

    Task<bool> UpdateTableStorageUsageAsync(TableStorageUsage tableStorageUsage);

    Task<bool> DeleteTableStorageUsageAsync(string ipAddress, string database, string table);



}
