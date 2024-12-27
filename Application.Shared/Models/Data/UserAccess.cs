using Application.Shared.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Data;

internal class UserAccess
{
    public string ApplicationUserId { get; set; }

    public ApplicationUser ApplicationUser { get; set; }
    
    public string Tables { get; set; }

    public Column Columns { get; set; }
}
