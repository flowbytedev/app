using Application.Shared.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Support;

[PrimaryKey(nameof(DataFileId), nameof(RequestId))]
public class DataAttachement
{
    public string DataFileId { get; set; }

    public DataFile? DataFile { get; set; }

    public int RequestId { get; set; }
    public Request? Request { get; set; }

}