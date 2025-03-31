using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BussinessObject;

namespace MilkTea_Admin_CRUD.Pages.ComboPage
{
    public class CreateModel : PageModel
    {
        private readonly BussinessObject.MilkTeaShopContext _context;

        public CreateModel(BussinessObject.MilkTeaShopContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["Product1Id"] = new SelectList(_context.Products, "ProductId", "Name");
        ViewData["Product2Id"] = new SelectList(_context.Products, "ProductId", "Name");
        ViewData["Topping1Id"] = new SelectList(_context.Toppings, "ToppingId", "Name");
        ViewData["Topping2Id"] = new SelectList(_context.Toppings, "ToppingId", "Name");
            return Page();
        }

        [BindProperty]
        public Combo Combo { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Combos.Add(Combo);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
