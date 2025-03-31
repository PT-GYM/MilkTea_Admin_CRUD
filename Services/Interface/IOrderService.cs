using BussinessObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IOrderService
    {
        Task CreateOrder(Order order, List<OrderDetail> orderDetails);  
        Task<Order> GetOrderById(int orderId);  
        Task<List<Order>> GetOrdersByUserId(int userId);  
        Task UpdateOrderStatus(int orderId, string status);  
    }
}
