using Application.Models;
using Application.Shared.Enums;
using Application.Shared.Models.Sales;

namespace Application.Shared.Services.Sales;

public interface ISalesForecastService
{

    Task<Response<IEnumerable<SalesForecastBySalesChannel>>> GetSalesForecastBySalesChannel(string userId, string? expand,
                                                        int pageSize,
                                                        int page,
                                                        SalesForecastBySalesChannel? Filter,
                                                        string? orderBy,
                                                        SortDirection orderDirection,
                                                        long CdcKey);
}
