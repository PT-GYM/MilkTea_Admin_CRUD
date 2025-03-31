using BussinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace MilkTea_Admin_CRUD.Pages.ProductPage
{
    [BindProperties]
    public class ShowModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategory _category;

        public ShowModel(IProductService productService, ICategory category)
        {
            _productService = productService;
            _category = category;
        }

        public IList<Product> Products { get; set; } = new List<Product>();

        public string? SearchKeyword { get; set; }

        public int TotalPages { get; set; }
        public int CurrentPage { get; set; } = 1;

        private const int PageSize = 3;

        public async Task<IActionResult> OnGetAsync(int pageNumber = 1)
        {
            CurrentPage = pageNumber;

            List<Product> allProducts;
            allProducts = (await _productService.GetAllProductsAsync()).ToList();

            int totalRecords = allProducts.Count();
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);

            Products = allProducts
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostSearchAsync()
        {
            return await OnGetAsync();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Authen/Login");
        }
    }
}


        