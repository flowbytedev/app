using Application.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Org;


[PrimaryKey(nameof(CompanyId), nameof(SalesChannelCode))]
public class SalesChannelCluster
{
    public string CompanyId { get; set; }
    public Company? Company { get; set; }
    public string SalesChannelCode { get; set; }
    public SalesChannel? SalesChannel { get; set; }
    public string Department { get; set; } // E.g., Electronics, Grocery, etc.
    public MachineLearningModel Model { get; set; } // Machine learning model used for clustering
    public string ClusterId { get; set; } // Identifier for the cluster
   

}
