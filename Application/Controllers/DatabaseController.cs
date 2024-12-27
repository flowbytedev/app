using Application.Services;
using Application.Shared.Models;
using Application.Shared.Models.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DatabaseController : ControllerBase
{

    private readonly DatabaseService _databaseService;

    public DatabaseController(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }


    [HttpGet("databases")]
    public async Task<IActionResult> GetDatabasesAsync()
    {
        var databases = await _databaseService.GetDatabasesAsync();
        return Ok(databases);
    }

    [HttpGet("tables")]
    public async Task<IActionResult> GetAllTablesAsync()
    {
        Database database = new();

        if (Request.Headers.ContainsKey("database"))
        {
            string dbHeader = Request.Headers["database"];
            database = JsonSerializer.Deserialize<Database>(dbHeader);
        }

        var tables = await _databaseService.GetAllTablesAsync(database);
        return Ok(tables);
    }

    [HttpGet("columns/{tableName}")]
    public async Task<IActionResult> GetColumnsForTableAsync(Table table)
    {
        Database database = new();

        if (Request.Headers.ContainsKey("database"))
        {
            string dbHeader = Request.Headers["database"];
            database = JsonSerializer.Deserialize<Database>(dbHeader);
        }

        var columns = await _databaseService.GetColumnsForTableAsync(database, table.Name);
        return Ok(columns);
    }


    [HttpGet("tables/data")]
    public async Task<List<Dictionary<string, object>>> GetTableDataAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
    {
        // get filterClause from request header
        var filterClause = "";
        Database database = new();
        Table table = new();
        

        if (Request.Headers.ContainsKey("filterquery"))
        {
            filterClause = Request.Headers["filterquery"];
        }


        if (Request.Headers.ContainsKey("database"))
        {
            string dbHeader = Request.Headers["database"];
            database = JsonSerializer.Deserialize<Database>(dbHeader);
        }

        if (Request.Headers.ContainsKey("table"))
        {
            string tableHeader = Request.Headers["table"];
            table = JsonSerializer.Deserialize<Table>(tableHeader);
        }


        return await _databaseService.GetTableDataAsync(schema: table.Schema, tableName: table.Name, filterClause: filterClause, pageNumber: pageNumber, pageSize: pageSize, database);
    }


    [HttpGet("tables/{tableName}/count")]
    public async Task<IActionResult> GetTotalRowCount(string tableName)
    {
        Database database = new();

        if (Request.Headers.ContainsKey("database"))
        {
            string dbHeader = Request.Headers["database"];
            database = JsonSerializer.Deserialize<Database>(dbHeader);
        }

        if(database is null)
        {
            // return error Database not found in the header
            string errorMessage = "Database not found in the header";
            return BadRequest(errorMessage);
        }

        var totalCount = await _databaseService.GetTotalRowCountAsync(database, tableName);

        return Ok(totalCount);
    }
}
