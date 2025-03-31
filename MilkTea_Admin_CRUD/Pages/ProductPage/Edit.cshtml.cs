using BussinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace MilkTea_Admin_CRUD.Pages.ProductPage
{
    public class EditModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategory _categoryService;

        public EditModel(IProductService productService, ICategory categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
            Product = new Product(); // Initialize Product
            Categories = new List<Category>(); // Initialize Categories
        }

        [BindProperty]
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }

        [BindProperty]
        public IFormFile? ProductImage { get; set; } // Declare ProductImage as nullable

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Categories = (await _categoryService.GetCategories()).ToList();
            Product = await _productService.GetProductByIdAsync(id);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Categories = (await _categoryService.GetCategories()).ToList();
                return Page();
            }

            // Get existing product to retain image if no new image is uploaded
            var existingProduct = await _productService.GetProductByIdAsync(Product.ProductId);
            if (existingProduct == null)
            {
                return NotFound(); // Handle if product does not exist
            }

            // Handle uploaded image
            if (ProductImage != null) // If there is a new image, update it
            {
                var fileName = Path.GetFileName(ProductImage.FileName);
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var filePath = Path.Combine(directory, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProductImage.CopyToAsync(stream);
                }

                // Update new image URL
                Product.ImageUrl = "/images/products/" + fileName;
            }
            else
            {
                // Retain old image if no new image is uploaded
                Product.ImageUrl = existingProduct.ImageUrl;
            }

            await _productService.UpdateProductAsync(Product);
            return RedirectToPage("./Show");
        }
    }
}
