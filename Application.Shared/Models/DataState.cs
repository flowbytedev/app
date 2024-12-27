using Application.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Models;

public class DataState<T> where T : class
{
    public int Page { get; set; } = 0;

    public int PageSize { get; set; } = 1000;
    public string? SortLabel { get; set; }
    public SortDirection SortDirection { get; set; } = SortDirection.Ascending;
    public T? Filter { get; set; }
}
