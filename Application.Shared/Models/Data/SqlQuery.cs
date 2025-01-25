using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Data;

public class SqlQuery : BaseModel
{
    [Key]
    public int Id { get; set; }


    [Required]
    [StringLength(2000, ErrorMessage = "SQL Query cannot exceed 2000 characters.")]
    public string Query { get; set; }


    public string Description { get; set; }

}
