//using BussinessObject;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Services.Interface;
//using Services.Service;

//namespace MilkTea_Admin_CRUD.Pages.OrderPage
//{
//    public class OrderDetailModel : PageModel
//    {
//        private readonly IOrderDetailService _orderDetailService;
//        private readonly IToppingService _toppingService;

//        public OrderDetailModel(IOrderDetailService orderDetailService, IToppingService toppingService)
//        {
//            _orderDetailService = orderDetailService;
//            _toppingService = toppingService;
//        }

//        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
//        public Dictionary<int, string> ToppingDetails { get; set; } = new Dictionary<int, string>();

//        public async Task<IActionResult> OnGetAsync(int orderId)
//        {
//            if (orderId == 0)
//            {
//                return NotFound();
//            }

//            OrderDetails = await _orderDetailService.GetOrderDetailsByOrderId(orderId);

//            if (OrderDetails == null || OrderDetails.Count == 0)
//            {
//                return NotFound();
//            }

//            foreach (var detail in OrderDetails)
//            {
//                if (!string.IsNullOrEmpty(detail.ToppingIds))
//                {
//                    var toppingIdList = detail.ToppingIds.Split(',')
//                        .Select(id => int.TryParse(id, out int tid) ? tid : (int?)null)
//                        .Where(tid => tid.HasValue)
//                        .Select(tid => tid.Value)
//                    .ToList();

//                    var toppings = await _toppingService.GetToppingByIdsAsync(toppingIdList);
//                    var toppingDetails = string.Join(", ", toppings.Select(t => $"{t.Name} ({t.Price:N0} VND)"));

//                    ToppingDetails[detail.OrderDetailId] = toppingDetails;
//                }
//                else
//                {
//                    ToppingDetails[detail.OrderDetailId] = "No topping added";
//                }
//            }

//            return Page();
//        }
//    }
//}

//public class UpdateOrderStatusRequest
//{
//    public int OrderId { get; set; }
//    public string NewStatus { get; set; }
//}



using BussinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interface;

namespace MilkTea_Admin_CRUD.Pages.OrderPage
{
    public class OrderDetailModel : PageModel
    {
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailModel(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        public List<OrderDetail> OrderDetails { get; set; } = new();
        public Dictionary<int, string> ToppingDetails { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            if (orderId <= 0) return NotFound();

            OrderDetails = await _orderDetailService.GetOrderDetailsByOrderId(orderId);
            if (OrderDetails == null || OrderDetails.Count == 0) return NotFound();

            foreach (var detail in OrderDetails)
            {
                if (detail.Toppings != null && detail.Toppings.Any())
                {
                    var toppingInfo = string.Join(", ", detail.Toppings.Select(t => $"{t.Name} ({t.Price:N0} VND)"));
                    ToppingDetails[detail.OrderDetailId] = toppingInfo;
                }
                else
                {
                    ToppingDetails[detail.OrderDetailId] = "No topping added";
                }
            }

            return Page();
        }
    }
}
