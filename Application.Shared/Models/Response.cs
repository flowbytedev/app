using Application.Shared.Models;

namespace Application.Models;


public class Response<T>
{
    public T Items { get; set; }
    public Dictionary<string, string> Filters { get; set; }
    
    public DataState DataState { get; set; }

    public int TotalItems { get; set; }


    public void SetTotalItems(int totalItems)
    {
        TotalItems = totalItems;
    }

}