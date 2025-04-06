using Application.Shared.Data;
using Application.Shared.Services.Data;
using Application.Shared.Services.RealTime;
using Application.Shared.Models.RealTime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Application.Hubs;

namespace Application.Controllers
{
    [Route("api/rt")]
    [ApiController]
    public class RealTimeDataController : ControllerBase
    {

        private readonly IRealTimeDataService _realTimeDataService;
        private readonly IHubContext<DataHub> _hubContext;
        private readonly ApplicationDbContext _context;

        public RealTimeDataController(IRealTimeDataService realTimeDataService, IHubContext<DataHub> hubContext, ApplicationDbContext context)
        {
            _realTimeDataService = realTimeDataService;
            _hubContext = hubContext;
            _context = context;
        }

        // POST: api/rt/sales/agg/store
        [HttpGet("sales/agg/store")]
        public List<SalesLineRealTime> GetSalesLinesByStore()
        {
            //var salesLines = _realTimeDataService.GetAllSalesData();
            //var salesLinesGrouped = salesLines.GroupBy(s => s.StoreId).Select(s => new SalesLineRealTime
            //{
            //    StoreId = s.Key,
            //    NetAmountAcy = s.Sum(s => s.NetAmountAcy)
            //}).ToList();

            // get the last records of each store
            var salesLines = _context.SalesLineRealTime
                .GroupBy(s => s.StoreCode)
                .Select(s => s.OrderByDescending(s => s.DateTime).FirstOrDefault())
                .ToList();

            return salesLines;

        }

        // POST: api/rt/sales
        [HttpPost("sales")]
        public async Task<IActionResult> SendData(List<SalesLineRealTime> salesLines)
        {
            if (salesLines == null)
            {
                return BadRequest("Data is null.");
            }



            // Push data to all connected clients
            //await _hubContext.Clients.All.SendAsync("ReceiveData", salesLinesGrouped.Sum(s => s.NetAmountAcy));
            await _hubContext.Clients.All.SendAsync("ReceiveData", salesLines);

            // add sales lines to db
            await _realTimeDataService.AddSalesLineRealTimeToDb(salesLines);



            return Ok(new { Message = "Data sent to clients successfully." });
        }

    }
}
