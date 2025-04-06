using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Data;

[Table("table", Schema ="data")]
[PrimaryKey(nameof(Host), nameof(DatabaseName), nameof(Schema), nameof(Name))]
public class Table
{
    public string? Host { get; set; }
    public string? DatabaseName { get; set; }
    public string? Schema { get; set; }
    public string? Name { get; set; }
}
