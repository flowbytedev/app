using Application.Shared.Models.RealTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.RealTime;

public interface IRealTimeDataService
{
    //Task UpdateSalesData(IEnumerable<SalesLineRealTime> salesData);

    List<SalesLineRealTime> GetAllSalesData();
    Task AddSalesLineRealTimeToDb(List<SalesLineRealTime> salesLines);
}
