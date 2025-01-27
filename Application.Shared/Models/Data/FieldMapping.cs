using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Data;

[PrimaryKey(nameof(SourceTable), nameof(SourceColumn), nameof(DestinationTable), nameof(DestinationColumn))]
public class FieldMapping
{
    public string SourceTable { get; set; }
    public string SourceColumn { get; set; }
    public string SourceDataType { get; set; }
    public string DestinationTable { get; set; }
    public string DestinationColumn { get; set; }
    public string DestinationDataType { get; set; }
    public bool? IsGroupBy { get; set; }
    public bool? IsSum { get; set; }
    public bool? IsCount { get; set; }
    public string? FilterQuery { get; set; }
    public string? DefaultValue {  get; set; }
    public bool IsAttribute { get; set; }

    
}
