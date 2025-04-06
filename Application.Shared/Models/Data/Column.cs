using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Data;

[PrimaryKey(nameof(Host), nameof(DatabaseName), nameof(Schema), nameof(TableName), nameof(Name))]
public class Column : BaseModel
{
    public string Host { get; set; }
    public string DatabaseName { get; set; }
    public string Schema { get; set; }
    public string TableName { get; set; }

    public string Name { get; set; }

    public string DataType { get; set; }

    public int Precision { get; set; }
    public int Scale { get; set; }
}
