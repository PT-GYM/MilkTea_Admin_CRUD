﻿using BussinessObject;
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
        Task<List<Order>> GetAllOrdersSortedByDate();
        Task<List<string>> GetOrderStatuses();
        Task<bool> ConfirmOrderAsync(string phoneNumber, string address, int productId, string toppingIds, int userId);

    }
}
