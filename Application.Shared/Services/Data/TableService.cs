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

public class TableService : ITableService
{
    private readonly ApplicationDbContext _context;
    private readonly ICompanyService _companyService;
    private readonly QueryService<Table> _queryService = new();

    public TableService(ApplicationDbContext context, ICompanyService companyService, QueryService<Table> queryService)
    {
        _context = context;
        _queryService = queryService;
        _companyService = companyService;
    }


    // CREATE: Add a new Table
    public async Task<Table> CreateTableAsync(Table table)
    {
        _context.Table.Add(table);

        await _context.SaveChangesAsync();

        return table;
    }

    // READ: Retrieve all tables
    public async Task<Response<List<Table>>> GetTablesAsync(string userId, string? expand = null,
                                                   int pageSize = 1000,
                                                   int page = 0,
                                                   Table? filter = null,
                                                   string? orderBy = null,
                                                   SortDirection orderDirection = SortDirection.Ascending)
    {
        var response = new Response<List<Table>>();

        
        // Retrieve the companies associated with the user
        var userCompanies = await _companyService.GetCompanies(userId);
        var companies = userCompanies.Select(c => c.Id).ToArray();


        // Start with the full queryable set of items
        var query = _context.Table.AsQueryable();

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
        response = new Response<List<Table>>
        {
            TotalItems = totalItems,
            Items = items,
            DataState = dataState,
            Status = ResponseStatus.Success
        };

        return response;
    }

    // READ: Retrieve a table by its composite key (IpAddress, Name)
    public async Task<Table?> GetTableByIdAsync(string ipAddress, string database, string name)
    {
        return await _context.Table.FindAsync(ipAddress, database, name);
    }

    // UPDATE: Update an existing table
    public async Task<bool> UpdateTableAsync(Table table)
    {
        _context.Entry(table).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TableExists(table.Host, table.DatabaseName, table.Name))
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

    // DELETE: Remove a table by its composite key
    public async Task<bool> DeleteTableAsync(string ipAddress, string database, string name)
    {
        var table = await GetTableByIdAsync(ipAddress, database, name);
        if (table == null)
        {
            return false; // Not found
        }

        _context.Table.Remove(table);
        await _context.SaveChangesAsync();
        return true;
    }


    private bool TableExists(string host, string database, string table)
    {
        return _context.Table.Any(e => e.Host == host && e.DatabaseName == database && e.Name == table);
    }





}
