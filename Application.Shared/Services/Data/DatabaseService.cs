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
using Application.Shared.Models.Admin;

namespace Application.Shared.Services.Data;

public class DatabaseService : IDatabaseService
{
    private readonly ApplicationDbContext _context;
    private readonly ICompanyService _companyService;
    private readonly QueryService<Database> _queryService = new();

    public DatabaseService(ApplicationDbContext context, ICompanyService companyService, QueryService<Database> queryService)
    {
        _context = context;
        _queryService = queryService;
        _companyService = companyService;
    }


    // CREATE: Add a new Database
    public async Task<Database> CreateDatabaseAsync(Database database)
    {
        _context.Database.Add(database);

        await _context.SaveChangesAsync();

        return database;
    }

    // READ: Retrieve all databases
    public async Task<Response<List<Database>>> GetDatabasesAsync(string userId, string? expand = null,
                                                   int pageSize = 1000,
                                                   int page = 0,
                                                   Database? filter = null,
                                                   string? orderBy = null,
                                                   SortDirection orderDirection = SortDirection.Ascending)
    {
        var response = new Response<List<Database>>();

        
        // Retrieve the companies associated with the user
        var userCompanies = await _companyService.GetCompanies(userId);
        var companies = userCompanies.Select(c => c.Id).ToArray();


        // Start with the full queryable set of items
        var query = _context.Database.AsQueryable();

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
            orderBy = "Name";
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
        response = new Response<List<Database>>
        {
            TotalItems = totalItems,
            Items = items,
            DataState = dataState,
            Status = ResponseStatus.Success
        };

        return response;
    }

    // READ: Retrieve a database by its composite key (IpAddress, Name)
    public async Task<Database?> GetDatabaseByIdAsync(string ipAddress, string name)
    {
        return await _context.Database.FindAsync(ipAddress, name);
    }

    // UPDATE: Update an existing database
    public async Task<bool> UpdateDatabaseAsync(Database database)
    {
        _context.Entry(database).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DatabaseExists(database.Host, database.Name))
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

    // DELETE: Remove a database by its composite key
    public async Task<bool> DeleteDatabaseAsync(string ipAddress, string name)
    {
        var database = await GetDatabaseByIdAsync(ipAddress, name);
        if (database == null)
        {
            return false; // Not found
        }

        _context.Database.Remove(database);
        await _context.SaveChangesAsync();
        return true;
    }


    private bool DatabaseExists(string host, string database)
    {
        return _context.Database.Any(e => e.Host == host && e.Name == database);
    }





}
