using Application.Models;
using Application.Shared.Enums;
using Application.Shared.Models.Inventory;
using Application.Shared.Models.Org;

namespace Application.Shared.Services.Sales;

public interface ISalesChannelService
{

    Task<Response<List<SalesChannel>>> GetSalesChannelsAsync(string userId, string? expand,
                                                        int pageSize,
                                                        int page,
                                                        SalesChannel? Filter,
                                                        string? orderBy,
                                                        SortDirection orderDirection);
}
