using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Org;


[PrimaryKey(nameof(CompanyId), nameof(Code))]
public class SalesChannel : BaseModel
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Region { get; set; } // Optional: Region or cluster for grouping
    public string? Address { get; set; } // Optional: Address details

    [Display(Name = "Store Group")]
    public string? StoreGroup { get; set; }

}
