using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Data;


[PrimaryKey(nameof(Host), nameof(DatabaseName), nameof(Username))]
public class DatabaseCredential
{
    public string Host { get; set; }
    
    public string DatabaseName { get; set; }
    
    public string Username { get; set; }

    [PasswordPropertyText]
    public string Password { get; set; }
}
