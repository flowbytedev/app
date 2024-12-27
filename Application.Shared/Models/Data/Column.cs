using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Data;

[Index(nameof(TableId), nameof(Name), IsUnique = true)]
public class Column : BaseModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }

    public string TableId { get; set; }

    public string Name { get; set; }
}
