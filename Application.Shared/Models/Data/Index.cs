using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Data;

public class Index
{
    public string Host { get; set; }
    public string DatabaseName { get; set; }
    public string Schema { get; set; }
    public string TableName { get; set; }

    public string Name { get; set; }
    public string Columns { get; set; }
    public string Type { get; set; }

    // col1; col2; col3
    public List<string> ColumnsList
    {
        get
        {
            return Columns?.Split(";").ToList() ?? new List<string>();
        }
    }

}
