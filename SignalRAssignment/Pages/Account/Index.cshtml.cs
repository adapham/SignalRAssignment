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

        public IList<Account> Account { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Accounts != null)
            {
                Account = await _context.Accounts.ToListAsync();
            }
        }
    }
}
