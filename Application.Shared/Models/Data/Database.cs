using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Data;


[PrimaryKey(nameof(Host), nameof(Name))]
public class Database
{
    public string Host { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
