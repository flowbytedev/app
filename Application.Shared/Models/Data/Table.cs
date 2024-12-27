using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Data;

[Keyless]
public class Table
{
    public string Schema { get; set; }
    public string Name { get; set; }
}
