using BussinessObject;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis;
using MilkTea_Customer_.Hubs;
using Services.Interface;

namespace MilkTea_Customer_.Service
{
    public class HubService : IHubService
    {
        private readonly IHubContext<ProductHub> _hubContext;

        public HubService(IHubContext<ProductHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task NotiProCreated(Product product)
        {
            await _hubContext.Clients.All.SendAsync("ProductCreated", product);
        }

        public async Task NotiProDeleted(int productId)
        {
            await _hubContext.Clients.All.SendAsync("ProductDeleted", productId);
        }

        public async Task NotiProUpdated(Product product)
        {
            await _hubContext.Clients.All.SendAsync("ProductUpdated", product);
        }
    }
}
