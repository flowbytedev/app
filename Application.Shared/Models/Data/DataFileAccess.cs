using Application.Shared.Models.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Data;


public enum AccessType
{
    Owner,
    Read,
    Write,
    NoDownload
}


public class DataFileAccessInput
{
    public string Email { get; set; }
    public AccessType AccessType { get; set; }
    public string? DataFileId { get; set; }
}


[PrimaryKey(nameof(DataFileId), nameof(ApplicationUserId))]
public class DataFileAccess
{
    public string? DataFileId { get; set; }
    public DataFile? DataFile { get; set; }

    public string? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }

    public AccessType AccessType { get; set; }



    public DataFileAccess(string dataFileId, string applicationUserId, AccessType accessType)
    {
        DataFileId = dataFileId;
        ApplicationUserId = applicationUserId;
        AccessType = accessType;

    }
}
