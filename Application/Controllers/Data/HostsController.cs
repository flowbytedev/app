using Microsoft.AspNetCore.Mvc;
using Application.Shared.Services.Data;
using Application.Shared.Models;
using Application.Shared.Models.Data;
using Application.Shared.Models.Inventory;
using Application.Shared.Enums;
using System.Threading.Tasks;
using System.Linq;
using Application.Models;
using Host = Application.Shared.Models.Data.Host;
using Application.Shared.Services;

namespace Application.Controllers.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HostsController : ControllerBase
    {
        private readonly IHostService _hostService;

        public HostsController(IHostService hostService)
        {
            _hostService = hostService;
        }

        // GET: api/Host?userId={userId}&expand={expand}&pageSize={pageSize}&page={page}&orderBy={orderBy}&orderDirection={orderDirection}
        [HttpGet]
        public async Task<IActionResult> GetHosts(
            [FromQuery] string? userId,
            [FromQuery] string? expand = null,
            [FromQuery] Host? filter = null,
            [FromQuery] int? pageSize = 1000,
            [FromQuery] int? page = 0,
            [FromQuery] string? orderBy = null,
            [FromQuery] SortDirection orderDirection = SortDirection.Ascending)
        {

            // In this example, the filter parameter is left as null.
            var response = await _hostService.GetHostsAsync(userId, expand, pageSize.Value, page.Value, filter: filter, orderBy, orderDirection);

            if (response.Status == ResponseStatus.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }



        // GET: api/Host/{ipAddress}/{name}
        [HttpGet("{ipAddress}/{name}")]
        public async Task<IActionResult> GetHost(string ipAddress, string name)
        {
            var host = await _hostService.GetHostByIdAsync(ipAddress, name);
            if (host == null)
            {
                return NotFound();
            }
            return Ok(host);
        }



        // POST: api/Host
        [HttpPost]
        public async Task<IActionResult> CreateHost([FromBody] Host host)
        {
            if (host == null)
            {
                return BadRequest();
            }
            var createdHost = await _hostService.CreateHostAsync(host);
            return CreatedAtAction(nameof(GetHost), new { ipAddress = createdHost.IpAddress, name = createdHost.Name }, createdHost);
        }



        // PUT: api/Host/{ipAddress}/{name}
        [HttpPut("{ipAddress}/{name}")]
        public async Task<IActionResult> UpdateHost(string ipAddress, string name, [FromBody] Host host)
        {
            if (host == null || ipAddress != host.IpAddress || name != host.Name)
            {
                return BadRequest();
            }
            var updated = await _hostService.UpdateHostAsync(host);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }



        // DELETE: api/Host/{ipAddress}/{name}
        [HttpDelete("{ipAddress}/{name}")]
        public async Task<IActionResult> DeleteHost(string ipAddress, string name)
        {
            var deleted = await _hostService.DeleteHostAsync(ipAddress, name);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
