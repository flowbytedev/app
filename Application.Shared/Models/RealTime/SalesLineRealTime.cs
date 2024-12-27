using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Shared.Models.RealTime;



[PrimaryKey(nameof(DateTime), nameof(CompanyId), nameof(StoreCode))]
public class SalesLineRealTime
{
    [JsonPropertyName("date_time")]
    public DateTime? DateTime { get; set; }

    [JsonPropertyName("store_code")]
    public string StoreCode { get; set; }

    [JsonPropertyName("net_amount_acy")]
    [Column("net_amount_acy", TypeName = "decimal(38, 20)")]
    public decimal NetAmountAcy { get; set; }

    [JsonPropertyName("scheme")]
    public string? Scheme { get; set; }

    [JsonPropertyName("company_id")]
    [Column("company_id")]
    public string? CompanyId { get; set; }

}

