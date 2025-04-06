using Microsoft.AspNetCore.Mvc;
using Application.Shared.Services.Data;
using Application.Shared.Models;
using Application.Shared.Models.Data;
using Application.Shared.Models.Inventory;
using Application.Shared.Enums;
using System.Threading.Tasks;
using System.Linq;
using Application.Models;
using Database = Application.Shared.Models.Data.Database;
using Application.Shared.Services;

namespace Application.Controllers.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabasesController : ControllerBase
    {
        private readonly IDatabaseService _databaseService;

        public DatabasesController(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        // GET: api/Database?userId={userId}&expand={expand}&pageSize={pageSize}&page={page}&orderBy={orderBy}&orderDirection={orderDirection}
        [HttpGet]
        public async Task<IActionResult> GetDatabases(
            [FromQuery] string? userId,
            [FromQuery] string? expand = null,
            [FromQuery] Database? filter = null,
            [FromQuery] int? pageSize = 1000,
            [FromQuery] int? page = 0,
            [FromQuery] string? orderBy = null,
            [FromQuery] SortDirection orderDirection = SortDirection.Ascending)
        {

            // In this example, the filter parameter is left as null.
            var response = await _databaseService.GetDatabasesAsync(userId, expand, pageSize.Value, page.Value, filter: filter, orderBy, orderDirection);

            if (response.Status == ResponseStatus.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }



        // GET: api/Database/{ipAddress}/{name}
        [HttpGet("{ipAddress}/{name}")]
        public async Task<IActionResult> GetDatabase(string ipAddress, string name)
        {
            var database = await _databaseService.GetDatabaseByIdAsync(ipAddress, name);
            if (database == null)
            {
                return NotFound();
            }
            return Ok(database);
        }



        // POST: api/Database
        [HttpPost]
        public async Task<IActionResult> CreateDatabase([FromBody] Database database)
        {
            if (database == null)
            {
                return BadRequest();
            }
            var createdDatabase = await _databaseService.CreateDatabaseAsync(database);
            return CreatedAtAction(nameof(GetDatabase), new { ipAddress = createdDatabase.Host, name = createdDatabase.Name }, createdDatabase);
        }



        // PUT: api/Database/{ipAddress}/{name}
        [HttpPut("{ipAddress}/{name}")]
        public async Task<IActionResult> UpdateDatabase(string ipAddress, string name, [FromBody] Database database)
        {
            if (database == null || ipAddress != database.Host || name != database.Name)
            {
                return BadRequest();
            }
            var updated = await _databaseService.UpdateDatabaseAsync(database);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }



        // DELETE: api/Database/{ipAddress}/{name}
        [HttpDelete("{ipAddress}/{name}")]
        public async Task<IActionResult> DeleteDatabase(string ipAddress, string name)
        {
            var deleted = await _databaseService.DeleteDatabaseAsync(ipAddress, name);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
