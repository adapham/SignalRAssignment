using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Q2.Hubs;
using SignalRAssignment.Common;
using SignalRAssignment.Models;

namespace SignalRAssignment.Pages_Category
{
    [StaffPermission]
    public class EditModel : PageModel
    {
        private readonly PizzaStoreContext _context;
        private readonly IHubContext<FoodStoreHub> foodStoreHub;

        public EditModel(PizzaStoreContext context, IHubContext<FoodStoreHub> foodStoreHub)
        {
            _context = context;
            this.foodStoreHub = foodStoreHub;
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }
            Category = category;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await foodStoreHub.Clients.All.SendAsync("LoadCategory");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(Category.CategoryId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
    }
}
