using Application.Shared.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Admin;

public class UserAccessRequest : BaseModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }

    public string? CompanyId { get; set; }

    public Company? Company { get; set; }

    public string? ApplicationUserId { get; set; }

    public ApplicationUser? ApplicationUser { get; set; }

    public string? ApplicationPageId { get; set; }

    public ApplicationPage? ApplicationPage { get; set; }

    public string Department { get; set; }

    public string ManagerName { get; set; }

    public string Reason { get; set; }

    [NotMapped]
    public string? Page { get; set; }


    [NotMapped]
    public string? FullName { get; set; }

    [NotMapped]
    public string? Email { get; set; }
}
