using Application.Models;
using Application.Shared.Enums;
using Application.Shared.Models.Inventory;
using Application.Shared.Models.Org;

namespace Application.Shared.Services.Sales;

public interface IItemService
{

    Task<Response<List<Item>>> GetItemsAsync(string userId, string? expand,
                                                        int pageSize,
                                                        int page,
                                                        Item? Filter,
                                                        string? orderBy,
                                                        SortDirection orderDirection);
}
