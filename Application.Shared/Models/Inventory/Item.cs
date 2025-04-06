using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Inventory;


[PrimaryKey(nameof(CompanyId), nameof(ItemNo))]
public class Item : BaseModel
{
    public string? ItemNo { get; set; }

    public string? Title { get; set; }
    public string? Description { get; set; }

    public string? Brand { get; set; }


    //public List<Variant>? Variants { get; set; }
    //public List<VariantOption>? VariantOptions { get; set; }
}
