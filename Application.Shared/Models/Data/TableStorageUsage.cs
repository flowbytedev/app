using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models.Data;

[Table("table_storage_usage", Schema = "data")]
[Keyless]
public class TableStorageUsage
{
    public DateTime? Date {  get; set; }
    public string? Host { get; set; }
    public string? DatabaseName { get; set; }
    public string? Schema { get; set; }
    public string? TableName { get; set; }
                 
    public int? RowCount { get; set; }
    public decimal? UsedSpaceMb { get; set; }
    public decimal? AllocatedSpaceMb { get; set; }


    
}



public class TableStorageUsageByMonth
{
    public string? YearMonth { get; set; }

    public string? Host { get; set; }
    public string? DatabaseName { get; set; }
    public string? Schema { get; set; }
    public string? TableName { get; set; }

    public long? RowCount { get; set; }
    public decimal? UsedSpaceMb { get; set; }
    public decimal? AllocatedSpaceMb { get; set; }
}


public class DatabaseStorageUsageByMonth
{
    public string? YearMonth { get; set; }

    public string? Host { get; set; }
    public string? DatabaseName { get; set; }

    public long? RowCount { get; set; }
    public decimal? UsedSpaceMb { get; set; }
    public decimal? AllocatedSpaceMb { get; set; }
}



public static class UsageGrouper
{
    public static List<DatabaseStorageUsageByMonth> GroupDatabaseUsageByMonth(List<TableStorageUsage> usages)
    {
        var groupedData = usages
            .GroupBy(u => new
            {
                // If Date is null, assign a default value such as "Unknown" for grouping.
                YearMonth = u.Date.HasValue ? u.Date.Value.ToString("yyyy-MM-dd") : "Unknown",
                u.Host,
                u.DatabaseName
            })
            .Select(g => new DatabaseStorageUsageByMonth
            {
                YearMonth = g.Key.YearMonth,
                Host = g.Key.Host,
                DatabaseName = g.Key.DatabaseName,
                // Sum the values, treating null as zero.
                RowCount = g.Sum(u => (long)(u.RowCount ?? 0)),
                UsedSpaceMb = g.Sum(u => u.UsedSpaceMb ?? 0),
                AllocatedSpaceMb = g.Sum(u => u.AllocatedSpaceMb ?? 0)
            })
            .ToList();

        return groupedData;
    }
}
