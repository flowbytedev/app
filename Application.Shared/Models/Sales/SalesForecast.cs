using Application.Shared.Models.Org;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Sales;


public class SalesForecast
{
    public string? CompanyId { get; set; }
    //public Company Company { get; set; }
    public DateTime? Date { get; set; }

    [Column("net_amount_acy")]
    public decimal? Amount { get; set; }

    [Column("net_amount_acy_lower")]
    public decimal? LowerBound { get; set; }

    [Column("net_amount_acy_upper")]
    public decimal? UpperBound { get; set; }
}
