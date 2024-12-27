using Application.Shared.Models;
using Application.Shared.Models.RealTime;

namespace Application.Shared.Services;

public class StateContainer
{
    private string? savedString;

    private Company company;
    private IQueryable<SalesLineRealTime> salesLineRealTime;


    public Company Company
    {
        get => company;
        set
        {
            company = value;
            NotifyStateChanged();
        }
    }

    public IQueryable<SalesLineRealTime> SalesLineRealTime
    {
        get => salesLineRealTime;
        set
        {
            salesLineRealTime = value;
            NotifyStateChanged();
        }
    }

    public string Property
    {
        get => savedString ?? string.Empty;
        set
        {
            savedString = value;
            NotifyStateChanged();
        }
    }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}
