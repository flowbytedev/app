using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Data;

public class IndexStorageUsage
{
    public DateTime Date { get; set; }
    public string Host { get; set; }
    public string DatabaseName { get; set; }
    public string Schema { get; set; }
    public string TableName { get; set; }

    public string Name { get; set; }

    public decimal UsedSpaceMb { get; set; }
    public decimal AllocatedSpaceMb { get; set; }

}
