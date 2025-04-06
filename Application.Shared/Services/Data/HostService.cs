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

public class HostService : IHostService
{
    private readonly ApplicationDbContext _context;
    private readonly ICompanyService _companyService;
    private readonly QueryService<Host> _queryService = new();

    public HostService(ApplicationDbContext context, ICompanyService companyService, QueryService<Host> queryService)
    {
        _context = context;
        _queryService = queryService;
        _companyService = companyService;
    }


    // CREATE: Add a new Host
    public async Task<Host> CreateHostAsync(Host host)
    {
        _context.Host.Add(host);

        await _context.SaveChangesAsync();

        return host;
    }

    // READ: Retrieve all hosts
    public async Task<Response<List<Host>>> GetHostsAsync(string userId, string? expand = null,
                                                   int pageSize = 1000,
                                                   int page = 0,
                                                   Host? filter = null,
                                                   string? orderBy = null,
                                                   SortDirection orderDirection = SortDirection.Ascending)
    {
        var response = new Response<List<Host>>();

        
        // Retrieve the companies associated with the user
        var userCompanies = await _companyService.GetCompanies(userId);
        var companies = userCompanies.Select(c => c.Id).ToArray();


        // Start with the full queryable set of items
        var query = _context.Host.AsQueryable();

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
            orderBy = "IpAddress";
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
        response = new Response<List<Host>>
        {
            TotalItems = totalItems,
            Items = items,
            DataState = dataState,
            Status = ResponseStatus.Success
        };

        return response;
    }

    // READ: Retrieve a host by its composite key (IpAddress, Name)
    public async Task<Host?> GetHostByIdAsync(string ipAddress, string name)
    {
        return await _context.Host.FindAsync(ipAddress, name);
    }

    // UPDATE: Update an existing host
    public async Task<bool> UpdateHostAsync(Host host)
    {
        _context.Entry(host).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!HostExists(host.IpAddress))
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

    // DELETE: Remove a host by its composite key
    public async Task<bool> DeleteHostAsync(string ipAddress, string name)
    {
        var host = await GetHostByIdAsync(ipAddress, name);
        if (host == null)
        {
            return false; // Not found
        }

        _context.Host.Remove(host);
        await _context.SaveChangesAsync();
        return true;
    }



    private bool HostExists(string ipAddress)
    {
        return _context.Host.Any(e => e.IpAddress == ipAddress);
    }

}
