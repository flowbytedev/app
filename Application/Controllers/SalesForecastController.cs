using Application.Attributes;
using Application.Helpers;
using Application.Models;
using Application.Shared.Enums;
using Application.Shared.Models.Admin;
using Application.Shared.Models.Data;
using Application.Shared.Models.Org;
using Application.Shared.Models.Sales;
using Application.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Shared.Models;
using System.Text.Json;
using Application.Services.Sales;
using Application.Services.Org;

namespace Application.Controllers;

[Route("api/sales/forecast")]
[ApiController]
public class SalesForecastController : ControllerBase
{

    private readonly ISalesForecastService _salesForecastService;
    private readonly ICompanyService _companyService;

    public SalesForecastController(ISalesForecastService salesForecastService, ICompanyService companyService)
    {
        _salesForecastService = salesForecastService;
        _companyService = companyService;
    }



    // GET: api/userData
    [HttpGet("sales-channel")]
    //[ValidateCompanyMembership]
    public async Task<ActionResult<Response<IEnumerable<SalesForecastBySalesChannel>>>> GetSalesForecastBySalesChannel(
                                                                    [FromQuery] string? expand = null,
                                                                    [FromQuery] SalesForecastBySalesChannel? Filter = null,
                                                                    [FromQuery] Int64 CdcKey = 0)
    {


        // page, pagesize,sortlabel,sortdirection

        var userId = Request.Headers["userId"];

        var dataStateHeader = HttpContext.Request.Headers["X-Data-State"];

        DataState dataState = new DataState();

        if (!String.IsNullOrEmpty(dataStateHeader))
        {
            dataState = JsonSerializer.Deserialize<DataState>(dataStateHeader);
        }

        

        var response = await _salesForecastService.GetSalesForecastBySalesChannel(userId, expand, dataState.PageSize, dataState.Page, Filter, dataState.SortLabel, dataState.SortDirection, CdcKey);

        return response;
    }

}
