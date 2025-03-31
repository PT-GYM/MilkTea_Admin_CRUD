using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject;
using Repository;

namespace MilkTea_Admin_CRUD.Pages.ComboPage
{
    public class DeleteModel : PageModel
    {
        private readonly Repository.MilkTeaShopContext _context;

        public DeleteModel(Repository.MilkTeaShopContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Combo Combo { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var combo = await _context.Combos.FirstOrDefaultAsync(m => m.ComboId == id);

            if (combo == null)
            {
                return NotFound();
            }
            else
            {
                Combo = combo;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var combo = await _context.Combos.FindAsync(id);
            if (combo != null)
            {
                Combo = combo;
                _context.Combos.Remove(Combo);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
