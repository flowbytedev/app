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

public interface IDatabaseCredentialService
{

    Task<DatabaseCredential> CreateDatabaseCredentialAsync(DatabaseCredential databaseCredential);


    Task<Response<List<DatabaseCredential>>> GetDatabaseCredentialsAsync(string userId, string? expand = null,
                                                   int pageSize = 1000,
                                                   int page = 0,
                                                   DatabaseCredential? filter = null,
                                                   string? orderBy = null,
                                                   SortDirection orderDirection = SortDirection.Ascending);

    Task<DatabaseCredential?> GetDatabaseCredentialByIdAsync(string ipAddress, string name, string username);

    Task<bool> UpdateDatabaseCredentialAsync(DatabaseCredential databaseCredential);

    Task<bool> DeleteDatabaseCredentialAsync(string ipAddress, string name, string username);



}
