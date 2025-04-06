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

public interface IDatabaseService
{

    Task<Database> CreateDatabaseAsync(Database database);


    Task<Response<List<Database>>> GetDatabasesAsync(string userId, string? expand = null,
                                                   int pageSize = 1000,
                                                   int page = 0,
                                                   Database? filter = null,
                                                   string? orderBy = null,
                                                   SortDirection orderDirection = SortDirection.Ascending);

    Task<Database?> GetDatabaseByIdAsync(string ipAddress, string name);

    Task<bool> UpdateDatabaseAsync(Database database);

    Task<bool> DeleteDatabaseAsync(string ipAddress, string name);



}
