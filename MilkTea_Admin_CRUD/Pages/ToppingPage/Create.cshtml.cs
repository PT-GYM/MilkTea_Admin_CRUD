using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BussinessObject;
using Repository;
using Services;

namespace MilkTea_Admin_CRUD.Pages.ToppingPage
{
    public class CreateModel : PageModel
    {
        private readonly IToppingService _toppingService;
        public CreateModel(IToppingService toppingService)
        {
            _toppingService = toppingService;
        }

        [BindProperty]
        public Topping topping { get; set; }

        [BindProperty]
        public IFormFile ToppingImage { get; set; } // L?u ?nh t?i lên

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Xử lý ảnh tải lên
            if (ToppingImage != null)
            {
                var fileName = Path.GetFileName(ToppingImage.FileName);
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/toppings");
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory); // Tạo thư mục nếu chưa tồn tại
                }

                var filePath = Path.Combine(directory, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ToppingImage.CopyToAsync(stream);
                }

                // Lưu đường dẫn ảnh vào database
                topping.ImageUrl = "/images/toppings/" + fileName;
            }

            await _toppingService.AddToppingAsync(topping);
            return RedirectToPage("/ToppingPage/Index");
        }
    }
}
