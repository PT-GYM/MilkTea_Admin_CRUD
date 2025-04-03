using BussinessObject;
using Microsoft.EntityFrameworkCore;
using Repository.UnitOfWorks;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Service
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateOrder(Order order, List<OrderDetail> orderDetails)
        {
            await _unitOfWork.order.AddAsync(order); 
            await _unitOfWork.SaveAsync(); 
            
            foreach (var detail in orderDetails)
            {
                detail.OrderId = order.OrderId; 
                await _unitOfWork.orderDetail.AddAsync(detail); 
            }

            await _unitOfWork.SaveAsync(); 
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            return await _unitOfWork.order.GetByIdAsync(orderId);
        }

        public async Task<List<Order>> GetOrdersByUserId(int userId)
        {
            return (await _unitOfWork.order.GetAsync(o => o.UserId == userId)).ToList();

        }

        public async Task UpdateOrderStatus(int orderId, string status)
        {
            var order = await _unitOfWork.order.GetByIdAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                _unitOfWork.order.Update(order);  
                await _unitOfWork.SaveAsync();  
            }
        }

        public async Task<List<Order>> GetAllOrdersSortedByDate()
        {
            return (await _unitOfWork.order.GetAsync(null))
                .OrderByDescending(o => o.OrderDate) 
                .ToList();
        }

        public async Task<List<string>> GetOrderStatuses()
        {
            return await Task.FromResult(new List<string> { "Pending", "Confirmed", "Canceled" });
        }

    }
}
