using Application.Shared.Enums;
using Azure.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Support;

public class Request
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }


    [Required]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
    public string Title { get; set; }

    [Required]
    public RequestType Type { get; set; }

    [Required]
    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string Description { get; set; }

    [Required]
    public string RequestedBy { get; set; } // User who made the request

    [Required]
    public DateTime RequestedDate { get; set; }

    public RequestStatus Status { get; set; } = RequestStatus.Pending;

    public string AssignedTo { get; set; } // Person assigned to handle the request

    public DateTime? CompletedDate { get; set; }
    //public ICollection<RequestComment> Comments { get; set; } = new List<RequestComment>();

    //public ICollection<SQLQueryAttachment> SQLQueries { get; set; } = new List<SQLQueryAttachment>();

    //public ICollection<DataAttachment> DataAttachments { get; set; } = new List<DataAttachment>();
}
