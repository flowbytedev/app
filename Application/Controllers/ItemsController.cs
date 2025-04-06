using Application.Attributes;
using Application.Helpers;
using Application.Models;
using Application.Shared.Services.Sales;
using Application.Shared.Enums;
using Application.Shared.Models;
using Application.Shared.Models.Admin;
using Application.Shared.Models.Data;
using Application.Shared.Models.Inventory;
using Application.Shared.Models.Org;
using Application.Shared.Models.Sales;
using Application.Shared.Services;
using DuckDB.NET.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[Route("api/items")]
[ApiController]
public class ItemsController : ControllerBase
{

    private readonly IItemService _itemService;

    private readonly string _dbPath = @"C:\Users\kevork.keheian\Desktop\Projects\flowbyte\streamlit\Items\flowbyte.duckdb"; 

    public ItemsController(IItemService itemService)
    {
        _itemService = itemService;
    }


    // GET: api/item
    [HttpGet()]
    //[ValidateCompanyMembership]
    public async Task<ActionResult<Response<List<Item>>>> GetItems(
                                                                    [FromQuery] string? expand = null,
                                                                    [FromQuery] int limit = 1000,
                                                                    [FromQuery] int page = 0,
                                                                    [FromQuery] Item? Filter = null,
                                                                    [FromQuery] string? orderBy = null,
                                                                    [FromQuery] SortDirection orderDirection = SortDirection.Ascending)
    {




        var userId = Request.Headers["userId"];

        var response = await _itemService.GetItemsAsync(userId, expand, limit, page, Filter, orderBy, orderDirection);

        return response;
    }


    // GET: api/item/duckdb
    [HttpGet("duckdb")]
    //[ValidateCompanyMembership]
    public async Task<ActionResult<Response<List<Item>>>> GetItemsDuckDb(
                                                                    [FromQuery] string? expand = null,
                                                                    [FromQuery] int limit = 1000,
                                                                    [FromQuery] int page = 0,
                                                                    [FromQuery] Item? Filter = null,
                                                                    [FromQuery] string? orderBy = null,
                                                                    [FromQuery] SortDirection orderDirection = SortDirection.Ascending)
    {




        
        var items = new List<Item>();

        using (var conn = new DuckDBConnection($"Data Source={_dbPath}"))
        {
            await conn.OpenAsync();

            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT item_no, description as title, description as description, brand FROM item LIMIT 100000;";

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var item = new Item
                        {
                            ItemNo = reader["item_no"] as string,
                            Title = reader["title"] as string,
                            Description = reader["description"] as string,
                            Brand = reader["brand"] as string
                        };
                        items.Add(item);
                    }
                }
            }
        }

        int totaItems = items.Count;


        DataState dataState = new DataState()
        {
            Page = page,
            PageSize = 200000,
            SortLabel = orderBy,
            SortDirection = orderDirection
        };

        Response<List<Item>> response = new Response<List<Item>>
        {
            TotalItems = totaItems,
            Items = items,
            DataState = dataState,
            Status = ResponseStatus.Success

        };



        return response;
        
       



    }

}
