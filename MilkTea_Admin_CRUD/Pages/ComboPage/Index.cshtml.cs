using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject;
using Repository;
using Services;

namespace MilkTea_Admin_CRUD.Pages.ComboPage
{
    public class IndexModel : PageModel
    {
        private readonly IComboService _comboService;

        public IndexModel(IComboService comboService)
        {
            _comboService = comboService;
        }

        public IList<Combo> Combos { get; set; } = default!;

        public string? SearchKeyword { get; set; }

        public int TotalPages { get; set; }
        public int CurrentPage { get; set; } = 1;

        private const int PageSize = 3;

        public async Task<IActionResult> OnGetAsync(int pageNumber = 1)
        {
            CurrentPage = pageNumber;

            List<Combo> allCombos;
            allCombos = (await _comboService.GetAllCombosAsync()).ToList();

            int totalRecords = allCombos.Count();
            TotalPages = (int)Math.Ceiling(totalRecords / (double)PageSize);

            Combos = allCombos
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostSearchAsync()
        {
            return await OnGetAsync();
        }
    }
}
