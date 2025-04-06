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

public class TableStorageUsageService : ITableStorageUsageService
{
    private readonly ApplicationDbContext _context;
    private readonly ICompanyService _companyService;
    private readonly QueryService<TableStorageUsage> _queryService = new();

    public TableStorageUsageService(ApplicationDbContext context, ICompanyService companyService, QueryService<TableStorageUsage> queryService)
    {
        _context = context;
        _queryService = queryService;
        _companyService = companyService;
    }


    // CREATE: Add a new TableStorageUsage
    public async Task<TableStorageUsage> CreateTableStorageUsageAsync(TableStorageUsage tableStorageUsage)
    {
        _context.TableStorageUsage.Add(tableStorageUsage);

        await _context.SaveChangesAsync();

        return tableStorageUsage;
    }

    // READ: Retrieve all tableStorageUsages
    public async Task<Response<List<TableStorageUsage>>> GetTableStorageUsagesAsync(string userId, string? expand = null,
                                                   int pageSize = 1000,
                                                   int page = 0,
                                                   TableStorageUsage? filter = null,
                                                   string? orderBy = null,
                                                   SortDirection orderDirection = SortDirection.Ascending)
    {
        var response = new Response<List<TableStorageUsage>>();

        
        // Retrieve the companies associated with the user
        var userCompanies = await _companyService.GetCompanies(userId);
        var companies = userCompanies.Select(c => c.Id).ToArray();


        // Start with the full queryable set of items
        var query = _context.TableStorageUsage.AsQueryable();

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
            orderBy = "TableName";
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
        response = new Response<List<TableStorageUsage>>
        {
            TotalItems = totalItems,
            Items = items,
            DataState = dataState,
            Status = ResponseStatus.Success
        };

        return response;
    }

    // READ: Retrieve a tableStorageUsage by its composite key (IpAddress, Name)
    public async Task<TableStorageUsage?> GetTableStorageUsageByIdAsync(string ipAddress, string database, string table)
    {
        return await _context.TableStorageUsage.FindAsync(ipAddress, database, table);
    }

    // UPDATE: Update an existing tableStorageUsage
    public async Task<bool> UpdateTableStorageUsageAsync(TableStorageUsage tableStorageUsage)
    {
        _context.Entry(tableStorageUsage).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TableStorageUsageExists(tableStorageUsage.Host, tableStorageUsage.DatabaseName, tableStorageUsage.TableName))
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

    // DELETE: Remove a tableStorageUsage by its composite key
    public async Task<bool> DeleteTableStorageUsageAsync(string ipAddress, string database, string table)
    {
        var tableStorageUsage = await GetTableStorageUsageByIdAsync(ipAddress, database, table);
        if (tableStorageUsage == null)
        {
            return false; // Not found
        }

        _context.TableStorageUsage.Remove(tableStorageUsage);
        await _context.SaveChangesAsync();
        return true;
    }


    private bool TableStorageUsageExists(string host, string database, string table)
    {
        return _context.TableStorageUsage.Any(e => e.Host == host && e.DatabaseName == database && e.TableName == table);
    }



}
