using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SignalRAssignment.Models;

namespace SignalRAssignment.Pages_Account
{
    public class CreateModel : PageModel
    {
        private readonly PizzaStoreContext _context;

        public CreateModel(PizzaStoreContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Account Account { get; set; } = default!;
        

        
        public async Task<IActionResult> OnPostAsync()
        {
            if (Account.UserName == null || Account.Password == null || Account.FullName == null)
            {
                return Page();
            }
           

            _context.Accounts.Add(Account);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
