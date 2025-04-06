using Microsoft.AspNetCore.Mvc;
using Application.Shared.Services.Data;
using Application.Shared.Models;
using Application.Shared.Models.Data;
using Application.Shared.Models.Inventory;
using Application.Shared.Enums;
using System.Threading.Tasks;
using System.Linq;
using Application.Models;
using DatabaseCredential = Application.Shared.Models.Data.DatabaseCredential;
using Application.Shared.Services;

namespace Application.Controllers.Data.Controllers
{
    [ApiController]
    [Route("api/databaseCredentials")]
    public class DatabaseCredentialsController : ControllerBase
    {
        private readonly IDatabaseCredentialService _databaseCredentialService;

        public DatabaseCredentialsController(IDatabaseCredentialService databaseCredentialService)
        {
            _databaseCredentialService = databaseCredentialService;
        }

        // GET: api/DatabaseCredential?userId={userId}&expand={expand}&pageSize={pageSize}&page={page}&orderBy={orderBy}&orderDirection={orderDirection}
        [HttpGet]
        public async Task<IActionResult> GetDatabaseCredentials(
            [FromQuery] string? userId,
            [FromQuery] string? expand = null,
            [FromQuery] DatabaseCredential? filter = null,
            [FromQuery] int? pageSize = 1000,
            [FromQuery] int? page = 0,
            [FromQuery] string? orderBy = null,
            [FromQuery] SortDirection orderDirection = SortDirection.Ascending)
        {

            // In this example, the filter parameter is left as null.
            var response = await _databaseCredentialService.GetDatabaseCredentialsAsync(userId, expand, pageSize.Value, page.Value, filter: filter, orderBy, orderDirection);

            if (response.Status == ResponseStatus.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }



        // GET: api/DatabaseCredential/{ipAddress}/{name}
        [HttpGet("{ipAddress}/{name}/{username}")]
        public async Task<IActionResult> GetDatabaseCredential(string ipAddress, string name, string username)
        {
            var databaseCredential = await _databaseCredentialService.GetDatabaseCredentialByIdAsync(ipAddress, name, username);
            if (databaseCredential == null)
            {
                return NotFound();
            }
            return Ok(databaseCredential);
        }



        // POST: api/DatabaseCredential
        [HttpPost]
        public async Task<IActionResult> CreateDatabaseCredential([FromBody] DatabaseCredential databaseCredential)
        {
            if (databaseCredential == null)
            {
                return BadRequest();
            }
            var createdDatabaseCredential = await _databaseCredentialService.CreateDatabaseCredentialAsync(databaseCredential);
            return CreatedAtAction(nameof(GetDatabaseCredential), new { ipAddress = createdDatabaseCredential.IpAddress, name = createdDatabaseCredential.Username, username = createdDatabaseCredential.Username }, createdDatabaseCredential);
        }



        // PUT: api/DatabaseCredential/{ipAddress}/{name}
        [HttpPut("{ipAddress}/{name}/{username}")]
        public async Task<IActionResult> UpdateDatabaseCredential(string ipAddress, string name, string username, [FromBody] DatabaseCredential databaseCredential)
        {
            if (databaseCredential == null || ipAddress != databaseCredential.IpAddress || name != databaseCredential.DatabaseName || username != databaseCredential.Username)
            {
                return BadRequest();
            }
            var updated = await _databaseCredentialService.UpdateDatabaseCredentialAsync(databaseCredential);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }



        // DELETE: api/DatabaseCredential/{ipAddress}/{name}
        [HttpDelete("{ipAddress}/{name}/{username}")]
        public async Task<IActionResult> DeleteDatabaseCredential(string ipAddress, string name, string username)
        {
            var deleted = await _databaseCredentialService.DeleteDatabaseCredentialAsync(ipAddress, name, username);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
