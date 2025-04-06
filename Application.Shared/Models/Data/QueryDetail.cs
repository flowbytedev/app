using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Data;


[Table("query_detail", Schema = "data")]
public class QueryDetail
{
    [Key]
    public int? Id { get; set; }

    public string? Query { get; set; }


    public string? Description { get; set; }

}
