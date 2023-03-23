using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SignalRAssignment.Common;
using SignalRAssignment.Models;

namespace SignalRAssignment.Pages_Category
{
    [StaffPermission]
    public class IndexModel : PageModel
    {
        private readonly PizzaStoreContext _context;

        public IndexModel(PizzaStoreContext context)
        {
            _context = context;
        }

        public IList<Category> Category { get; set; } = default!;
        [FromQuery]
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        public async Task OnGetAsync()
        {          
            if (_context.Categories != null)
            {
                if (!string.IsNullOrEmpty(SearchString))
                {
                    try
                    {
                        var value = Decimal.Parse(SearchString);
                        Category = await _context.Categories
                              .Where(c => c.CategoryId == (int)value)
                              .ToListAsync();
                    }
                    catch (Exception)
                    {
                        Category = await _context.Categories
                               .Where(c => c.CategoryName.ToLower().Contains(SearchString.ToLower().Trim()))
                               .ToListAsync();
                    }

                }
                else
                {
                    Category = await _context.Categories
                        .ToListAsync();
                }
            }
        }



    }
}
