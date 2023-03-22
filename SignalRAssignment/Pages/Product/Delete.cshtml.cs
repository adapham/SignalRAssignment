using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SignalRAssignment.Common;
using SignalRAssignment.Models;

namespace SignalRAssignment.Pages_Product
{
     [StaffPermission]
    public class DeleteModel : PageModel
    {
        private readonly PizzaStoreContext _context;

        public DeleteModel(PizzaStoreContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                try
                {
                    Product = product;
                    _context.Products.Remove(Product);
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    if (
                        e.InnerException != null
                        && e.InnerException.Message.Contains(
                            "The DELETE statement conflicted with the REFERENCE constraint"
                        )
                    )
                    {
                        ModelState.AddModelError(
                            string.Empty,
                            "Cannot delete product because it is referenced by an order."
                        );
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, e.Message);
                    }
                    return Page();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
