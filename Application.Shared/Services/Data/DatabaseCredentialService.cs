using Application.Models;
using Application.Shared.Services;
using Application.Shared.Data;
using Application.Shared.Models;
using Application.Shared.Models.Data;
using Application.Shared.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Shared.Services.Org;
using Application.Shared.Enums;

namespace Application.Shared.Services.Data;

public class DatabaseCredentialService : IDatabaseCredentialService
{
    private readonly ApplicationDbContext _context;
    private readonly ICompanyService _companyService;
    private readonly QueryService<DatabaseCredential> _queryService = new();

    public DatabaseCredentialService(ApplicationDbContext context, ICompanyService companyService, QueryService<DatabaseCredential> queryService)
    {
        _context = context;
        _queryService = queryService;
        _companyService = companyService;
    }


    // CREATE: Add a new DatabaseCredential
    public async Task<DatabaseCredential> CreateDatabaseCredentialAsync(DatabaseCredential databaseCredential)
    {
        _context.DatabaseCredential.Add(databaseCredential);

        await _context.SaveChangesAsync();

        return databaseCredential;
    }

    // READ: Retrieve all databaseCredentials
    public async Task<Response<List<DatabaseCredential>>> GetDatabaseCredentialsAsync(string userId, string? expand = null,
                                                   int pageSize = 1000,
                                                   int page = 0,
                                                   DatabaseCredential? filter = null,
                                                   string? orderBy = null,
                                                   SortDirection orderDirection = SortDirection.Ascending)
    {
        var response = new Response<List<DatabaseCredential>>();

        
        // Retrieve the companies associated with the user
        var userCompanies = await _companyService.GetCompanies(userId);
        var companies = userCompanies.Select(c => c.Id).ToArray();


        // Start with the full queryable set of items
        var query = _context.DatabaseCredential.AsQueryable();

        // Apply filters if a filter object is provided
        if (filter != null)
        {
            _queryService.Filter = filter;
            query = _queryService.ApplyFilters(query);
        }

        // Apply ordering: use provided orderBy or default to "ItemNo"
        if (!string.IsNullOrEmpty(orderBy))
        {
            bool descending = orderDirection == SortDirection.Descending;
            query = _queryService.ApplyOrdering(query, orderBy, descending);
        }
        else
        {
            orderBy = "Username";
            query = _queryService.ApplyOrdering(query, orderBy);
        }

        // Get the total count before pagination
        var totalItems = await query.CountAsync();

        // Apply pagination using Skip and Take
        query = query.Skip(page * pageSize).Take(pageSize);

        // Retrieve the list of items
        var items = await query.ToListAsync();

        // Build the data state information
        var dataState = new DataState
        {
            Page = page,
            PageSize = pageSize,
            SortLabel = orderBy,
            SortDirection = orderDirection
        };

        // Create the final response
        response = new Response<List<DatabaseCredential>>
        {
            TotalItems = totalItems,
            Items = items,
            DataState = dataState,
            Status = ResponseStatus.Success
        };

        return response;
    }

    // READ: Retrieve a databaseCredential by its composite key (IpAddress, Name)
    public async Task<DatabaseCredential?> GetDatabaseCredentialByIdAsync(string ipAddress, string name, string username)
    {
        return await _context.DatabaseCredential.FindAsync(ipAddress, name, username);
    }

    // UPDATE: Update an existing databaseCredential
    public async Task<bool> UpdateDatabaseCredentialAsync(DatabaseCredential databaseCredential)
    {
        _context.Entry(databaseCredential).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DatabaseCredentialExists(databaseCredential.IpAddress, databaseCredential.DatabaseName, databaseCredential.Username))
            {
                return false;
            }
            else
            {
                throw;
            }
        }

        return true;
    }

    // DELETE: Remove a databaseCredential by its composite key
    public async Task<bool> DeleteDatabaseCredentialAsync(string ipAddress, string name, string username)
    {
        var databaseCredential = await GetDatabaseCredentialByIdAsync(ipAddress, name, username);
        if (databaseCredential == null)
        {
            return false; // Not found
        }

        _context.DatabaseCredential.Remove(databaseCredential);
        await _context.SaveChangesAsync();
        return true;
    }


    private bool DatabaseCredentialExists(string host, string database, string username)
    {
        return _context.DatabaseCredential.Any(e => e.IpAddress == host && e.DatabaseName == database && e.Username == username);
    }




}
