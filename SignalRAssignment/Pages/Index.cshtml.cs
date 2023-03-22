using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SignalRAssignment.Common;
using SignalRAssignment.Models;

namespace SignalRAssignment.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly PizzaStoreContext _context;

        public IndexModel(PizzaStoreContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IList<Models.Product> Product { get; set; } = default!;

        [FromQuery]
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Products != null)
            {
                if (!string.IsNullOrEmpty(SearchString))
                {
                    try
                    {
                        var value = Decimal.Parse(SearchString);
                        Product = await _context.Products
                              .Include(p => p.Category)
                              .Include(p => p.Supplier)
                              .Where(p => p.UnitPrice == value || p.ProductId == (int)value)
                              .ToListAsync();
                    }
                    catch (Exception)
                    {
                        Product = await _context.Products
                               .Include(p => p.Category)
                               .Include(p => p.Supplier)
                               .Where(p => p.ProductName.ToLower().Contains(SearchString.ToLower().Trim()))
                               .ToListAsync();
                    }

                }
                else
                {
                    Product = await _context.Products
                        .Include(p => p.Category)
                        .Include(p => p.Supplier)
                        .ToListAsync();
                }
            }
        }



    }
}
