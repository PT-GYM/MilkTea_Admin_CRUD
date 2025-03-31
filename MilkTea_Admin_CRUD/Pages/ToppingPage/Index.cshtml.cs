using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject;
using Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Services;

namespace MilkTea_Admin_CRUD.Pages.ToppingPage
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly IToppingService _toppingService;
        private readonly ICategory _category;

        public IndexModel(IToppingService toppingService, ICategory category)
        {
            _toppingService = toppingService;
            _category = category;
        }

        public IList<Topping> Toppings { get; set; } = new List<Topping>();

        public string? SearchKeyword { get; set; }

        public int TotalPages { get; set; }
        public int CurrentPage { get; set; } = 1;

        private const int PageSize = 3;

        public async Task<IActionResult> OnGetAsync(int pageNumber = 1)
        {
            CurrentPage = pageNumber;

            List<Topping> allToppings;
            allToppings = (await _toppingService.GetAllToppingsAsync()).ToList();

            int totalRecords = allToppings.Count();
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);

            Toppings = allToppings
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
