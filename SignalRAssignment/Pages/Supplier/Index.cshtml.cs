using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SignalRAssignment.Common;
using SignalRAssignment.Models;

namespace SignalRAssignment.Pages_Supplier
{
    [StaffPermission]
    public class IndexModel : PageModel
    {
        private readonly PizzaStoreContext _context;

        public IndexModel(PizzaStoreContext context)
        {
            _context = context;
        }

        public IList<Supplier> Supplier { get;set; } = default!;

        [FromQuery]
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        public async Task OnGetAsync()
        {
            if (_context.Suppliers != null)
            {
                if (!string.IsNullOrEmpty(SearchString))
                {
                    try
                    {
                        var value = Decimal.Parse(SearchString);
                        Supplier = await _context.Suppliers
                            .Where(s => s.SupplierId == (int)value)
                            .ToListAsync();
                    }
                    catch (Exception)
                    {
                        Supplier = await _context.Suppliers
                            .Where(s => s.CompanyName.ToLower().Contains(SearchString.ToLower().Trim()) || 
                            s.Address.ToLower().Contains(SearchString.ToLower().Trim()))
                            .ToListAsync();
                    }

                }
                else
                {
                    Supplier = await _context.Suppliers
                        .ToListAsync();
                }
            }
        }
    }
}
