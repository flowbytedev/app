using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Data;

[PrimaryKey(nameof(IpAddress), nameof(Name))]
[Table("host", Schema = "data")]
public class Host
{
    public string? IpAddress { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    //public int Port { get; set; }
}
