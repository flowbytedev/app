using Application.Shared.Models.Org;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Sales;


[PrimaryKey(nameof(CompanyId), nameof(SalesChannelCode), nameof(Date))]
public class SalesForecastBySalesChannel : SalesForecast
{
    public string? SalesChannelCode { get; set; }
    //public SalesChannel SalesChannel { get; set; }


    public decimal CalculateDeviation()
    {
        var lowerDeviation = (this.Amount.Value - this.LowerBound.Value) / this.Amount.Value;
        var upperDeviation = (this.Amount.Value - this.UpperBound.Value) / this.Amount.Value;

        return Math.Abs((lowerDeviation + upperDeviation) / 2);
    }

}