using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BussinessObject;
using Services.Interface;

namespace MilkTea_Admin_CRUD.Pages.Combos
{
    public class IndexModel : PageModel
    {
        private readonly IComboService _comboService;

        public IndexModel(IComboService comboService)
        {
            _comboService = comboService;
        }

        public IEnumerable<Combo> Combos { get; set; }

        public async Task OnGetAsync()
        {
            // Fetch all combos from the service and pass them to the page.
            Combos = await _comboService.GetAllCombos();
        }

        // Optionally, you can add methods to delete or edit combos if needed.
        public async Task<IActionResult> OnPostDeleteAsync(int comboId)
        {
            var result = await _comboService.DeleteCombo(comboId);
            if (result)
            {
                // Redirect to the same page after deletion to refresh the list.
                return RedirectToPage();
            }

            // Optionally, handle the failure case here.
            return Page();
        }
    }
}
