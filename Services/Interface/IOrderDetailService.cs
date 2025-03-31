using BussinessObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IOrderDetailService
    {
        Task AddOrderDetail(OrderDetail orderDetail); 
        Task<List<OrderDetail>> GetOrderDetailsByOrderId(int orderId);  
    }
}
