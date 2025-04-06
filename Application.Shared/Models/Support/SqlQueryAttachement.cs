using Application.Shared.Models.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Support;


[PrimaryKey(nameof(SqlQueryId), nameof(RequestId))]
public class SqlQueryAttachement
{
    public int SqlQueryId { get; set; }

    public QueryDetail? SqlQuery { get; set; }

    public int RequestId { get; set; }
    public Request? Request { get; set; }

}
