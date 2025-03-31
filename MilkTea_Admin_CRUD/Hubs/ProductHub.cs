using Microsoft.AspNetCore.SignalR;

namespace MilkTea_Admin_CRUD.Hubs
{
    public class ProductHub : Hub
    {
        public async Task NotifyDelete(int productId)
        {
            await Clients.All.SendAsync("ReloadALL", productId);
            Console.WriteLine($"Đã gửi tín hiệu SignalR ProductDeleted cho ID: {productId}");
        }
    }
}




