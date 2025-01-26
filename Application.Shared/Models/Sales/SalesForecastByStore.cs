using Application.Shared.Models.Org;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Sales;

[PrimaryKey(nameof(CompanyId), nameof(SalesChannelCode), nameof(Date))]
public class SalesForecastByStore
{
    public string CompanyId { get; set; }
    public Company Company { get; set; }
    public DateTime Date { get; set; }
    public string SalesChannelCode { get; set; }
    public SalesChannel SalesChannel { get; set; }
    public double Amount { get; set; }
    public double? LowerBound { get; set; }
    public double? UpperBound { get; set; }
}
