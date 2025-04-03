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
        private readonly IProductService _productService;
        private readonly IToppingService _toppingService;
        private readonly IOrderDetailService _orderDetailService;

        public OrderService(IUnitOfWork unitOfWork, IProductService productService, IToppingService toppingService, IOrderDetailService orderDetailService )
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
            _toppingService = toppingService;
            _orderDetailService = orderDetailService;

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
            return await _unitOfWork.order.Query()
                .Include(o => o.User) 
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        public async Task<List<string>> GetOrderStatuses()
        {
            return await Task.FromResult(new List<string> { "Pending", "Confirmed", "Canceled" });
        }

        public async Task<bool> ConfirmOrderAsync(string phoneNumber, string address, int productId, string toppingIds, int userId)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null)
            {
                return false;  
            }

            List<Topping> toppings = new List<Topping>();
            decimal toppingTotal = 0;

            if (!string.IsNullOrEmpty(toppingIds))
            {
                var toppingIdList = toppingIds.Split(',')
                                              .Select(id =>
                                              {
                                                  return int.TryParse(id, out int result) ? result : (int?)null;
                                              })
                                              .Where(id => id.HasValue)
                                              .Select(id => id.Value)
                                              .ToList();

                toppings = await _toppingService.GetToppingByIdsAsync(toppingIdList);
                toppingTotal = toppings.Sum(t => t.Price);
            }

            decimal totalAmount = product.Price + toppingTotal;

            if (!await _productService.CheckAndUpdateProductStockAsync(product))
            {
                return false; 
            }

            if (!await _toppingService.CheckAndUpdateToppingStockAsync(toppings))
            {
                return false; 
            }

            var order = new Order
            {
                UserId = userId,
                PhoneNumber = phoneNumber,
                Address = address,
                TotalAmount = totalAmount,
                Status = "Pending",
                OrderDate = DateTime.Now
            };

            var orderDetail = new OrderDetail
            {
                ProductId = productId,
                ToppingIds = string.Join(",", toppings.Select(t => t.ToppingId)),
                Quantity = 1,
                SubTotal = product.Price + toppingTotal
            };

            await CreateOrder(order, new List<OrderDetail> { orderDetail });

            return true; 
        }

    }
}



