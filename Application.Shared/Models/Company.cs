using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models;

public class Company : BaseModel
{
    [Key]
    [MaxLength(10)]
    public string? Id { get; set; }

    public string? Name { get; set; }

}
