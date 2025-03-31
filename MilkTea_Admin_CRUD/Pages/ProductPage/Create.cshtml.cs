using BussinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Interface;

namespace MilkTea_Admin_CRUD.Pages.ProductPage
{
    public class CreateModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public CreateModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }

        [BindProperty]
        public IFormFile ProductImage { get; set; } // L?u ?nh t?i lên

        public async Task<IActionResult> OnGetAsync()
        {
            Categories = (await _categoryService.GetCategories()).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Categories = (await _categoryService.GetCategories()).ToList();
                return Page();
            }

            // X? lý ?nh t?i lên
            if (ProductImage != null)
            {
                var fileName = Path.GetFileName(ProductImage.FileName);
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory); // T?o th? m?c n?u ch?a t?n t?i
                }

                var filePath = Path.Combine(directory, fileName);


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProductImage.CopyToAsync(stream);
                }

                // L?u ???ng d?n ?nh vào database
                Product.ImageUrl = "/images/products/" + fileName;
            }

            await _productService.AddProductAsync(Product);
            return RedirectToPage("/ProductPage/Show");
        }
    }
}

