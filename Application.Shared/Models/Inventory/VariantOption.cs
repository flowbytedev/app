using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Shared.Models.Inventory;


[PrimaryKey(nameof(CompanyId), nameof(ItemCode))]
public class VariantOption : BaseModel
{
    public string ItemCode { get; set; }
    public string Name { get; set; }


    //[JsonIgnore]
    //public Variant Variant { get; set; }

    [JsonIgnore]
    public Item Item { get; set; }
    public List<Variant> Variants { get; set; }
}
