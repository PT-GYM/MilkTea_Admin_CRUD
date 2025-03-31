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

namespace MilkTea_Admin_CRUD.Pages.ComboPage
{
    public class CreateModel : PageModel
    {
        private readonly IComboService _comboService;
        private readonly IProductService _productService;
        private readonly IToppingService _toppingService;

        public CreateModel(IComboService comboService, IProductService productService, IToppingService toppingService)
        {
            _comboService = comboService;
            _productService = productService;
            _toppingService = toppingService;
        }

        [BindProperty]
        public Combo Combo { get; set; } = new Combo();

        [BindProperty]
        public IFormFile ComboImage { get; set; } // Lưu ảnh tải lên

        [BindProperty]
        public List<int> SelectedProductId { get; set; } = new List<int>();

        [BindProperty]
        public List<int> SelectedToppingId { get; set; } = new List<int>();


        public List<Product> Products { get; set; }
        public List<Topping> Toppings { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Products = (await _productService.GetAllProductsAsync()).ToList();
            Toppings = (await _toppingService.GetAllToppingsAsync()).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Products = (await _productService.GetAllProductsAsync()).ToList();
                Toppings = (await _toppingService.GetAllToppingsAsync()).ToList();
                return Page();
            }

            // Xử lý ảnh tải lên
            if (ComboImage != null)
            {
                var fileName = Path.GetFileName(ComboImage.FileName);
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/combos");
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory); // Tạo thư mục nếu chưa tồn tại
                }

                var filePath = Path.Combine(directory, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ComboImage.CopyToAsync(stream);
                }

                // Lưu đường dẫn ảnh vào database
                Combo.ImageUrl = "/images/combos/" + fileName;
            }

            // Lưu danh sách sản phẩm và topping
            Combo.Products = new List<Product>();
            foreach (var productId in SelectedProductId)
            {
                var product = await _productService.GetProductByIdAsync(productId);
                if (product != null)
                {
                    Combo.Products.Add(product);
                }
            }

            Combo.Toppings = new List<Topping>();
            foreach (var toppingId in SelectedToppingId)
            {
                var topping = await _toppingService.GetToppingByIdAsync(toppingId);
                if (topping != null)
                {
                    Combo.Toppings.Add(topping);
                }
            }

            await _comboService.AddComboAsync(Combo);
            return RedirectToPage("/ComboPage/Index");
        }

    }
}
