using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Services.Data;


public class ChartDataset
{
    public string Label { get; set; }
    public decimal[] Values { get; set; }
    public string BorderColor { get; set; }
    public string BackgroundColor { get; set; }

}
