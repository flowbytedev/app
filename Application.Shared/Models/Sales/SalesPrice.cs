using Application.Shared.Models.Inventory;
using Application.Shared.Models.Org;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Shared.Models.Sales;


[PrimaryKey(nameof(CompanyId), nameof(ItemCode), nameof(VariantCode), nameof(SalesChannelCode), nameof(CurrencyCode))]
public class SalesPrice : BaseModel
{
    public string ItemCode { get; set; }
    public string VariantCode { get; set; }
    public string SalesChannelCode { get; set; }
    public SalesChannel SalesChannel { get; set; }

    public string CurrencyCode { get; set; }

    public decimal UnitPrice { get; set; }

    [JsonIgnore]
    public decimal UnitPriceVat { get; set; }

    public decimal DiscountPrice { get; set; }

    [JsonIgnore]
    public decimal DiscountPriceVat { get; set; }



    [JsonIgnore]
    public Variant Variant { get; set; }


}
