using System.Text.Json;
using BussinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interface;

namespace MilkTea_Admin_CRUD.Pages.OrderPage
{
    public class IndexModel : PageModel
    {
        private readonly IOrderService _orderService;

        public IndexModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public List<Order> Orders { get; set; } = new List<Order>();

        public List<string> OrderStatuses { get; set; } = new List<string>();

        [BindProperty]
        public int OrderId { get; set; }

        [BindProperty]
        public string NewStatus { get; set; }

        public async Task OnGetAsync()
        {
            Orders = await _orderService.GetAllOrdersSortedByDate();
            OrderStatuses = await _orderService.GetOrderStatuses(); 
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var order = await _orderService.GetOrderById(OrderId);
            if (order == null) return RedirectToPage("Index");

            if (order.Status != NewStatus)
            {
                await _orderService.UpdateOrderStatus(OrderId, NewStatus);
            }

            return RedirectToPage("Index");
        }
    }

}