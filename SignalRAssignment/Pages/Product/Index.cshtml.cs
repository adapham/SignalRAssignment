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
    public class IndexModel : PageModel
    {
        private readonly PizzaStoreContext _context;

        public IndexModel(PizzaStoreContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; } = default!;
        public const int ITEMS_PAGE = 5;
        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentPage { get; set; }
        public int countPages { get; set; }
        public Func<int?, string> generateUrl { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Products != null)
            {
                int total = await _context.Products.CountAsync();
                countPages = (int)Math.Ceiling((double)total / ITEMS_PAGE);
                if (currentPage < 1)
                {
                    currentPage = 1;
                }
                Product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                 .Skip(ITEMS_PAGE * (currentPage - 1))
                 .Take(ITEMS_PAGE)
                .ToListAsync();
            }
        }
    }
}
