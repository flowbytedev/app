using Application.Shared.Models.Org;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Sales;

[PrimaryKey(nameof(CompanyId), nameof(SalesChannelGroup), nameof(Date))]
public class SalesForecastBySalesChannelGroup : SalesForecast
{
    public string SalesChannelGroup { get; set; }

}
