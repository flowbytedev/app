using Application.Data;
using Application.Shared.Models.Data;
using Application.Shared.Models;
using Application.Shared.Services;
using Application.Shared.Models.Sales;
using Application.Models;
using Application.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Application.Shared.Models.Org;
using Application.Services.Org;

namespace Application.Services.Sales;

public class SalesChannelService : ISalesChannelService
{
    private readonly ApplicationDbContext _context;
    private readonly QueryService<SalesChannel> _queryService;
    private readonly ICompanyService _companyService;

    public SalesChannelService(ApplicationDbContext context, ICompanyService companyService, QueryService<SalesChannel> queryService)
    {
        _context = context;
        _companyService = companyService;
        _queryService = queryService;
    }


    public async Task<Response<List<SalesChannel>>> GetSalesChannelsAsync(string userId, string? expand = null,
                                                        int pageSize = 1000,
                                                        int page = 0,
                                                        SalesChannel? Filter = null,
                                                        string? orderBy = null,
                                                        SortDirection orderDirection = SortDirection.Ascending)
    {

        var response = new Response<List<SalesChannel>>();

        var userCompanies = await _companyService.GetCompanies(userId);
        var companies = userCompanies.Select(c => c.Id).ToArray();

        if (!companies.Contains(Filter.CompanyId))
        {
            response.Status = ResponseStatus.Error;
        }


        var query = _context.SalesChannel.AsQueryable();


        // Filter
        if (Filter != null)
        {
            _queryService.Filter = Filter;
            query = _queryService.ApplyFilters(query);
        }


        // Order by
        if (!string.IsNullOrEmpty(orderBy))
        {
            bool descending = orderDirection == SortDirection.Descending ? true : false;
            query = _queryService.ApplyOrdering(query, orderBy, descending);

        }
        else
        {
            orderBy = "Code";
            query = _queryService.ApplyOrdering(query, orderBy);
        }

        var totaItems = await query.CountAsync();



        // Pagination
        query = query.Skip(page * pageSize).Take(pageSize);



        var items = await query.ToListAsync();

        DataState dataState = new DataState()
        {
            Page = page,
            PageSize = pageSize,
            SortLabel = orderBy,
            SortDirection = orderDirection
        };

        response = new Response<List<SalesChannel>>
        {
            TotalItems = totaItems,
            Items = items,
            DataState = dataState,
            Status = ResponseStatus.Success

        };



        return response;


    }
}
