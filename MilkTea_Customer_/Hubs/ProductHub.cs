using Microsoft.AspNetCore.SignalR;

namespace MilkTea_Customer_.Hubs
{
    public class ProductHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}




