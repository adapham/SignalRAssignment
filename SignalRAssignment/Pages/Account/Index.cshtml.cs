using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SignalRAssignment.Models;

namespace SignalRAssignment.Pages_Account
{
    public class IndexModel : PageModel
    {
        private readonly SignalRAssignment.Models.PizzaStoreContext _context;

        public IndexModel(SignalRAssignment.Models.PizzaStoreContext context)
        {
            _context = context;
        }

        public IList<Account> Account { get; set; } = default!;
        [FromQuery]
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        public async Task OnGetAsync()
        {
            if (_context.Accounts != null)
            {
                if (!string.IsNullOrEmpty(SearchString))
                {
                    try
                    {
                        var value = Decimal.Parse(SearchString);
                        Account = await _context.Accounts
                              .Where(a => a.Type == (int)value)
                              .ToListAsync();
                    }
                    catch (Exception)
                    {
                        Account = await _context.Accounts
                               .Where(a => a.UserName.ToLower().Contains(SearchString.ToLower().Trim()) ||
                               a.FullName.ToLower().Contains(SearchString.ToLower().Trim()))
                               .ToListAsync();
                    }

                }
                else
                {
                    Account = await _context.Accounts
                        .ToListAsync();
                }
            }
        }
    }
}
