using Application.Shared.Models.RealTime;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;


namespace Application.Hubs;

public class DataHub : Hub
{
    // This method is called by the API to send data to all connected clients
    public async Task SendRealTimeSalesToClients(List<SalesLineRealTime> data)
    {
        await Clients.All.SendAsync("ReceiveData", data);
    }
}
