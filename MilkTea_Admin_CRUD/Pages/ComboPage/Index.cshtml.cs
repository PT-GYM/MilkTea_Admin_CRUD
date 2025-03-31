using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject;

namespace MilkTea_Admin_CRUD.Pages.ComboPage
{
    public class IndexModel : PageModel
    {
        private readonly BussinessObject.MilkTeaShopContext _context;

        public IndexModel(BussinessObject.MilkTeaShopContext context)
        {
            _context = context;
        }

        public IList<Combo> Combo { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Combo = await _context.Combos
                .Include(c => c.Product1)
                .Include(c => c.Product2)
                .Include(c => c.Topping1)
                .Include(c => c.Topping2).ToListAsync();
        }
    }
}
