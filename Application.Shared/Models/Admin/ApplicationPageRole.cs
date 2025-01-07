using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Admin;

[PrimaryKey(nameof(ApplicationPageId), nameof(IdentityRoleId))]
public class ApplicationPageRole
{
    public string ApplicationPageId { get; set; }

    public ApplicationPage ApplicationPage { get; set; }

    public string IdentityRoleId { get; set; }

    public IdentityRole IdentityRole { get; set; }
}
