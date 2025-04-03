using BussinessObject;
using Repository.UnitOfWorks;
using Services.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Service
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddOrderDetail(OrderDetail orderDetail)
        {
            await _unitOfWork.orderDetail.AddAsync(orderDetail);  
            await _unitOfWork.SaveAsync();  
        }

        public async Task<List<OrderDetail>> GetOrderDetailsByOrderId(int orderId)
        {
            var orderDetails = await _unitOfWork.orderDetail.GetAsync(
                od => od.OrderId == orderId,
                includeProperties: "Product"
            );

            foreach (var detail in orderDetails)
            {
                if (!string.IsNullOrEmpty(detail.ToppingIds))
                {
                    var toppingIds = detail.ToppingIds.Split(',')
                        .Select(id => int.TryParse(id, out int tid) ? tid : (int?)null)
                        .Where(tid => tid.HasValue)
                        .Select(tid => tid.Value)
                        .ToList();

                    var toppings = await _unitOfWork.topping.GetAsync(t => toppingIds.Contains(t.ToppingId));
                    detail.Toppings = toppings.ToList(); 
                }
            }

            return orderDetails.ToList();
        }

        public async Task<Dictionary<string, string>> GetToppingDetailsByOrderIdAsync(int orderId)
        {
            var orderDetails = await _unitOfWork.orderDetail.GetAsync(
                od => od.OrderId == orderId,
                includeProperties: "Product"
            );

            var toppingDetailsDict = new Dictionary<string, string>();

            foreach (var detail in orderDetails)
            {
                if (!string.IsNullOrEmpty(detail.ToppingIds))
                {
                    var toppingIdList = detail.ToppingIds.Split(',')
                        .Select(id => int.TryParse(id, out int tid) ? tid : (int?)null)
                        .Where(id => id.HasValue)
                        .Select(id => id.Value)
                        .ToList();

                    var toppings = await _unitOfWork.topping.GetAsync(t => toppingIdList.Contains(t.ToppingId));
                    var toppingDetails = string.Join(", ", toppings.Select(t => $"{t.Name} ({t.Price:N0} VND)"));

                    toppingDetailsDict[detail.ToppingIds] = toppingDetails;
                }
            }

            return toppingDetailsDict;
        }
    }
}
