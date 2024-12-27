using Application.Shared.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.BYOD;

public class UserUploadedTable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public string TableName { get; set; }
    public string FileName { get; set; }
    public DateTime UploadedOn { get; set; } = DateTime.Now;
}
