using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Inventory;


[PrimaryKey(nameof(CompanyId), nameof(Code))]
public class Item : BaseModel
{
    public string? Code { get; set; }

    public string? Description { get; set; }

    public string? Brand { get; set; }
    public string? SubBrand { get; set; }
    public string? BaseUom { get; set; }

    public string? DefaultVendorNo { get; set; }


    //public List<Variant>? Variants { get; set; }
    public List<VariantOption>? VariantOptions { get; set; }
}
