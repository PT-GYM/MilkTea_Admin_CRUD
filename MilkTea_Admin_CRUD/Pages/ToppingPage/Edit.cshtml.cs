using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BussinessObject;
using Repository;
using Services.Interface;

namespace MilkTea_Admin_CRUD.Pages.ToppingPage
{
    public class EditModel : PageModel
    {
        private readonly IToppingService _toppingService;
        
        public EditModel(IToppingService toppingService)
        {
            _toppingService = toppingService;
        }

        [BindProperty]
        public Topping Topping { get; set; }

        [BindProperty]
        public IFormFile? ToppingImage { get; set; } // Declare ProductImage as nullable

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Topping = await _toppingService.GetToppingByIdAsync(id);

            if (Topping == null)
            {
                return NotFound();
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Get existing topping to retain image if no new image is uploaded
            var existingTopping = await _toppingService.GetToppingByIdAsync(Topping.ToppingId);
            if (existingTopping == null)
            {
                return NotFound(); // Handle if topping does not exist
            }

            // Handle uploaded image
            if (ToppingImage != null) // If there is a new image, update it
            {
                var fileName = Path.GetFileName(ToppingImage.FileName);
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/toppings");

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var filePath = Path.Combine(directory, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ToppingImage.CopyToAsync(stream);
                }

                // Update new image URL
                Topping.ImageUrl = "/images/toppings/" + fileName;
            }
            else
            {
                // Retain old image if no new image is uploaded
                Topping.ImageUrl = existingTopping.ImageUrl;
            }

            await _toppingService.UpdateToppingAsync(Topping);
            return RedirectToPage("./Index");
        }
    }
}
