using Application.Attributes;
using Application.Helpers;
using Application.Models;
using Application.Services.Sales;
using Application.Shared.Enums;
using Application.Shared.Models.Admin;
using Application.Shared.Models.Data;
using Application.Shared.Models.Org;
using Application.Shared.Models.Sales;
using Application.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[Route("api/sales/channels")]
[ApiController]
public class SalesChannelsController : ControllerBase
{

    private readonly ISalesChannelService _salesChannelService;

    public SalesChannelsController(ISalesChannelService salesChannelService)
    {
        _salesChannelService = salesChannelService;
    }


    // GET: api/salesChannel
    [HttpGet()]
    //[ValidateCompanyMembership]
    public async Task<ActionResult<Response<List<SalesChannel>>>> GetSalesChannels(
                                                                    [FromQuery] string? expand = null,
                                                                    [FromQuery] int limit = 1000,
                                                                    [FromQuery] int page = 0,
                                                                    [FromQuery] SalesChannel? Filter = null,
                                                                    [FromQuery] string? orderBy = null,
                                                                    [FromQuery] SortDirection orderDirection = SortDirection.Ascending)
    {




        var userId = Request.Headers["userId"];

        var response = await _salesChannelService.GetSalesChannelsAsync(userId, expand, limit, page, Filter, orderBy, orderDirection);

        return response;
    }

}
