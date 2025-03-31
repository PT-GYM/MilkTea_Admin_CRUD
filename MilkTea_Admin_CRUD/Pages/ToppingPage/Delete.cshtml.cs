using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BussinessObject;
using Repository;
using Microsoft.AspNetCore.SignalR;
using MilkTea_Admin_CRUD.Hubs;
using Services.Interface;

namespace MilkTea_Admin_CRUD.Pages.ToppingPage
{
    public class DeleteModel : PageModel
    {
        private readonly IToppingService _toppingService;
        private readonly IHubContext<ProductHub> _hubContext;

        public DeleteModel(IToppingService toppingService, IHubContext<ProductHub> hubContext)
        {
            _toppingService = toppingService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public int ToppingId { get; set; }

        [BindProperty]
        public Topping Topping { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Topping = await _toppingService.GetToppingByIdAsync(id.Value);

            if (Topping == null)
            {
                return NotFound();
            }

            ToppingId = id.Value;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topping = await _toppingService.GetToppingByIdAsync(id.Value);
            if (topping != null)
            {
                await _toppingService.DeleteToppingAsync(id.Value);

                // Send SignalR message to all clients
                await _hubContext.Clients.All.SendAsync("ToppingDeleted", id.Value);
            }

            return RedirectToPage("./Index");
        }
    }
}
