using Microsoft.AspNetCore.Mvc;
using Application.Shared.Services.Data;
using Application.Shared.Models;
using Application.Shared.Models.Data;
using Application.Shared.Models.Inventory;
using Application.Shared.Enums;
using System.Threading.Tasks;
using System.Linq;
using Application.Models;
using QueryDetail = Application.Shared.Models.Data.QueryDetail;
using Application.Shared.Services;

namespace Application.Controllers.Data.Controllers
{
    [ApiController]
    [Route("api/queries")]
    public class QueryDetailsController : ControllerBase
    {
        private readonly IQueryDetailService _queryDetailService;

        public QueryDetailsController(IQueryDetailService queryDetailService)
        {
            _queryDetailService = queryDetailService;
        }

        // GET: api/QueryDetail?userId={userId}&expand={expand}&pageSize={pageSize}&page={page}&orderBy={orderBy}&orderDirection={orderDirection}
        [HttpGet]
        public async Task<IActionResult> GetQueryDetails(
            [FromQuery] string? userId,
            [FromQuery] string? expand = null,
            [FromQuery] QueryDetail? filter = null,
            [FromQuery] int? pageSize = 1000,
            [FromQuery] int? page = 0,
            [FromQuery] string? orderBy = null,
            [FromQuery] SortDirection orderDirection = SortDirection.Ascending)
        {

            // In this example, the filter parameter is left as null.
            var response = await _queryDetailService.GetQueryDetailsAsync(userId, expand, pageSize.Value, page.Value, filter: filter, orderBy, orderDirection);

            if (response.Status == ResponseStatus.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }



        // GET: api/QueryDetail/{ipAddress}/{name}
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetQueryDetail(int Id)
        {
            var queryDetail = await _queryDetailService.GetQueryDetailByIdAsync(Id);
            if (queryDetail == null)
            {
                return NotFound();
            }
            return Ok(queryDetail);
        }



        // POST: api/QueryDetail
        [HttpPost]
        public async Task<IActionResult> CreateQueryDetail([FromBody] QueryDetail queryDetail)
        {
            if (queryDetail == null)
            {
                return BadRequest();
            }
            var createdQueryDetail = await _queryDetailService.CreateQueryDetailAsync(queryDetail);
            return CreatedAtAction(nameof(GetQueryDetail), new { Id = createdQueryDetail.Id }, createdQueryDetail);
        }



        // PUT: api/QueryDetail/{ipAddress}/{name}
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateQueryDetail(int Id, [FromBody] QueryDetail queryDetail)
        {
            if (queryDetail == null || Id != queryDetail.Id)
            {
                return BadRequest();
            }
            var updated = await _queryDetailService.UpdateQueryDetailAsync(queryDetail);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }



        // DELETE: api/QueryDetail/{ipAddress}/{name}
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteQueryDetail(int Id)
        {
            var deleted = await _queryDetailService.DeleteQueryDetailAsync(Id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
