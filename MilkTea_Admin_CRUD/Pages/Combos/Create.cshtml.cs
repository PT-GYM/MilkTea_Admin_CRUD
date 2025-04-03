using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BussinessObject;
using Services.Interface;

namespace MilkTea_Admin_CRUD.Pages.Combos
{
    public class CreateModel : PageModel
    {
        private readonly IComboService _comboService;

        public CreateModel(IComboService comboService)
        {
            _comboService = comboService;
        }

        [BindProperty]
        public Combo Combo { get; set; }

        public void OnGet()
        {
           
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
                var result = await _comboService.AddCombo(Combo);
                if (result)
                {
                    return RedirectToPage("Index");  
                }
                else
                {
                    ModelState.AddModelError("", "Không thể thêm combo.");
                }
            

            return Page();
        }

    }
}
