using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Data;


[Table("database_credentials", Schema = "data")]
[PrimaryKey(nameof(IpAddress), nameof(DatabaseName), nameof(Username))]
public class DatabaseCredential
{
    [Column("host")]
    public string? IpAddress { get; set; }
    
    public string? DatabaseName { get; set; }
    
    public string? Username { get; set; }

    [PasswordPropertyText]
    public string? Password { get; set; }
}
