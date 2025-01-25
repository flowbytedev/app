using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Support;

public class RequestComment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int RequestId { get; set; } // Foreign key to the Request

    public Request Request { get; set; } // Navigation property

    [Required]
    public string CommentedBy { get; set; }

    [Required]
    [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
    public string Comment { get; set; }

    [Required]
    public DateTime CommentedDate { get; set; }

    
}
