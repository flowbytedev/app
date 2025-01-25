using Application.Shared.Models.Sales;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Shared.Models.Inventory;



[PrimaryKey(nameof(CompanyId), nameof(ItemCode), nameof(VariantCode))]
public class Variant : BaseModel
{
    public string ItemCode { get; set; }

    public string VariantCode { get; set; }

    public string VariantOptionName { get; set; }
    
    [JsonIgnore]
    public VariantOption VariantOption { get; set; }

    public string Value { get; set; }


    // public Variant Variant { get; set; }
    public List<VariantOption> VariantOptions { get; set; }

    public List<SalesPrice> SalesPrices { get; set; }

}
