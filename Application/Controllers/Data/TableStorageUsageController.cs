using Microsoft.AspNetCore.Mvc;
using Application.Shared.Services.Data;
using Application.Shared.Models;
using Application.Shared.Models.Data;
using Application.Shared.Models.Inventory;
using Application.Shared.Enums;
using System.Threading.Tasks;
using System.Linq;
using Application.Models;
using TableStorageUsage = Application.Shared.Models.Data.TableStorageUsage;
using Application.Shared.Services;

namespace Application.Controllers.Data.Controllers
{
    [ApiController]
    [Route("api/tables/storage/usage")]
    public class TableStorageUsagesController : ControllerBase
    {
        private readonly ITableStorageUsageService _tableStorageUsageService;

        public TableStorageUsagesController(ITableStorageUsageService tableStorageUsageService)
        {
            _tableStorageUsageService = tableStorageUsageService;
        }

        // GET: api/TableStorageUsage?userId={userId}&expand={expand}&pageSize={pageSize}&page={page}&orderBy={orderBy}&orderDirection={orderDirection}
        [HttpGet]
        public async Task<IActionResult> GetTableStorageUsages(
            [FromQuery] string? userId,
            [FromQuery] string? expand = null,
            [FromQuery] TableStorageUsage? filter = null,
            [FromQuery] int? pageSize = 1000,
            [FromQuery] int? page = 0,
            [FromQuery] string? orderBy = null,
            [FromQuery] SortDirection orderDirection = SortDirection.Ascending)
        {

            // In this example, the filter parameter is left as null.
            var response = await _tableStorageUsageService.GetTableStorageUsagesAsync(userId, expand, pageSize.Value, page.Value, filter: filter, orderBy, orderDirection);

            if (response.Status == ResponseStatus.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }



        // GET: api/TableStorageUsage/{ipAddress}/{name}
        [HttpGet("{ipAddress}/{database}/{table}")]
        public async Task<IActionResult> GetTableStorageUsage(string ipAddress, string database, string table)
        {
            var tableStorageUsage = await _tableStorageUsageService.GetTableStorageUsageByIdAsync(ipAddress, database, table);
            if (tableStorageUsage == null)
            {
                return NotFound();
            }
            return Ok(tableStorageUsage);
        }



        // POST: api/TableStorageUsage
        [HttpPost]
        public async Task<IActionResult> CreateTableStorageUsage([FromBody] TableStorageUsage tableStorageUsage)
        {
            if (tableStorageUsage == null)
            {
                return BadRequest();
            }
            var createdTableStorageUsage = await _tableStorageUsageService.CreateTableStorageUsageAsync(tableStorageUsage);
            return CreatedAtAction(nameof(GetTableStorageUsage), new { ipAddress = createdTableStorageUsage.Host, database = createdTableStorageUsage.DatabaseName, table = createdTableStorageUsage.TableName }, createdTableStorageUsage);
        }



        // PUT: api/TableStorageUsage/{ipAddress}/{name}
        [HttpPut("{ipAddress}/{database}/{table}")]
        public async Task<IActionResult> UpdateTableStorageUsage(string ipAddress, string database, string table, [FromBody] TableStorageUsage tableStorageUsage)
        {
            if (tableStorageUsage == null || ipAddress != tableStorageUsage.Host || database != tableStorageUsage.DatabaseName || table != tableStorageUsage.TableName)
            {
                return BadRequest();
            }
            var updated = await _tableStorageUsageService.UpdateTableStorageUsageAsync(tableStorageUsage);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }



        // DELETE: api/TableStorageUsage/{ipAddress}/{name}
        [HttpDelete("{ipAddress}/{database}/{table}")]
        public async Task<IActionResult> DeleteTableStorageUsage(string ipAddress, string database, string table)
        {
            var deleted = await _tableStorageUsageService.DeleteTableStorageUsageAsync(ipAddress, database, table);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
