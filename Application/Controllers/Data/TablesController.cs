using Microsoft.AspNetCore.Mvc;
using Application.Shared.Services.Data;
using Application.Shared.Models;
using Application.Shared.Models.Data;
using Application.Shared.Models.Inventory;
using Application.Shared.Enums;
using System.Threading.Tasks;
using System.Linq;
using Application.Models;
using Table = Application.Shared.Models.Data.Table;
using Application.Shared.Services;

namespace Application.Controllers.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TablesController : ControllerBase
    {
        private readonly ITableService _tableService;

        public TablesController(ITableService tableService)
        {
            _tableService = tableService;
        }

        // GET: api/Table?userId={userId}&expand={expand}&pageSize={pageSize}&page={page}&orderBy={orderBy}&orderDirection={orderDirection}
        [HttpGet]
        public async Task<IActionResult> GetTables(
            [FromQuery] string? userId,
            [FromQuery] string? expand = null,
            [FromQuery] Table? filter = null,
            [FromQuery] int? pageSize = 1000,
            [FromQuery] int? page = 0,
            [FromQuery] string? orderBy = null,
            [FromQuery] SortDirection orderDirection = SortDirection.Ascending)
        {

            // In this example, the filter parameter is left as null.
            var response = await _tableService.GetTablesAsync(userId, expand, pageSize.Value, page.Value, filter: filter, orderBy, orderDirection);

            if (response.Status == ResponseStatus.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }



        // GET: api/Table/{ipAddress}/{name}
        [HttpGet("{ipAddress}/{database}/{name}")]
        public async Task<IActionResult> GetTable(string ipAddress, string database, string name)
        {
            var table = await _tableService.GetTableByIdAsync(ipAddress, database, name);
            if (table == null)
            {
                return NotFound();
            }
            return Ok(table);
        }



        // POST: api/Table
        [HttpPost]
        public async Task<IActionResult> CreateTable([FromBody] Table table)
        {
            if (table == null)
            {
                return BadRequest();
            }
            var createdTable = await _tableService.CreateTableAsync(table);
            return CreatedAtAction(nameof(GetTable), new { ipAddress = createdTable.Host, name = createdTable.Name }, createdTable);
        }



        // PUT: api/Table/{ipAddress}/{name}
        [HttpPut("{ipAddress}/{database}/{name}")]
        public async Task<IActionResult> UpdateTable(string ipAddress, string database, string name, [FromBody] Table table)
        {
            if (table == null || ipAddress != table.Host || database != table.DatabaseName || name != table.Name)
            {
                return BadRequest();
            }
            var updated = await _tableService.UpdateTableAsync(table);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }



        // DELETE: api/Table/{ipAddress}/{name}
        [HttpDelete("{ipAddress}/{database}/{name}")]
        public async Task<IActionResult> DeleteTable(string ipAddress, string database, string name)
        {
            var deleted = await _tableService.DeleteTableAsync(ipAddress, database, name);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
