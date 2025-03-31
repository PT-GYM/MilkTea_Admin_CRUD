using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using MilkTea_Admin_CRUD.Hubs;
using Services.Interface;

namespace MilkTea_Admin_CRUD.Pages.ProductPage
{
    public class DeleteModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly IHubContext<ProductHub> _hubContext;

        public DeleteModel(IProductService productService, IHubContext<ProductHub> hubContext)
        {
            _productService = productService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public int ProductId { get;  set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ProductId = id;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID không hợp lệ.");
            }

            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound("Sản phẩm không tồn tại.");
            }

            try
            {
                await _productService.DeleteProductAsync(id);

                // Gửi tín hiệu SignalR cho tất cả client
                await _hubContext.Clients.All.SendAsync("ProductDeleted", id);

                return RedirectToPage("./Show");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Xóa sản phẩm thất bại: " + ex.Message);
                return Page();
            }
        }
    }
}


