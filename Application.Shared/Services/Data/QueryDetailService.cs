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

public class QueryDetailService : IQueryDetailService
{
    private readonly ApplicationDbContext _context;
    private readonly ICompanyService _companyService;
    private readonly QueryService<QueryDetail> _queryService = new();

    public QueryDetailService(ApplicationDbContext context, ICompanyService companyService, QueryService<QueryDetail> queryService)
    {
        _context = context;
        _queryService = queryService;
        _companyService = companyService;
    }


    // CREATE: Add a new QueryDetail
    public async Task<QueryDetail> CreateQueryDetailAsync(QueryDetail queryDetail)
    {
        _context.QueryDetail.Add(queryDetail);

        await _context.SaveChangesAsync();

        return queryDetail;
    }

    // READ: Retrieve all queryDetails
    public async Task<Response<List<QueryDetail>>> GetQueryDetailsAsync(string userId, string? expand = null,
                                                   int pageSize = 1000,
                                                   int page = 0,
                                                   QueryDetail? filter = null,
                                                   string? orderBy = null,
                                                   SortDirection orderDirection = SortDirection.Ascending)
    {
        var response = new Response<List<QueryDetail>>();

        
        // Retrieve the companies associated with the user
        var userCompanies = await _companyService.GetCompanies(userId);
        var companies = userCompanies.Select(c => c.Id).ToArray();


        // Start with the full queryable set of items
        var query = _context.QueryDetail.AsQueryable();

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
            orderBy = "Id";
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
        response = new Response<List<QueryDetail>>
        {
            TotalItems = totalItems,
            Items = items,
            DataState = dataState,
            Status = ResponseStatus.Success
        };

        return response;
    }

    // READ: Retrieve a queryDetail by its composite key (IpAddress, Name)
    public async Task<QueryDetail?> GetQueryDetailByIdAsync(int Id)
    {
        return await _context.QueryDetail.FindAsync(Id);
    }

    // UPDATE: Update an existing queryDetail
    public async Task<bool> UpdateQueryDetailAsync(QueryDetail queryDetail)
    {
        _context.Entry(queryDetail).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!QueryDetailExists(queryDetail.Id.Value))
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

    // DELETE: Remove a queryDetail by its composite key
    public async Task<bool> DeleteQueryDetailAsync(int Id)
    {
        var queryDetail = await GetQueryDetailByIdAsync(Id);
        if (queryDetail == null)
        {
            return false; // Not found
        }

        _context.QueryDetail.Remove(queryDetail);
        await _context.SaveChangesAsync();
        return true;
    }



    private bool QueryDetailExists(int id)
    {
        return _context.QueryDetail.Any(e => e.Id == id);
    }

}
