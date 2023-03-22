using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SignalRAssignment.Common;
using SignalRAssignment.Models;

namespace SignalRAssignment.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Models.Account Account { get; set; } = default!;

        private readonly PizzaStoreContext _context;

        public LoginModel(PizzaStoreContext context)
        {
            _context = context;
        }

        public void OnGet() { }

        /// <summary>
        /// This method is called when the user clicks the submit button on the login page.
        /// </summary>
        public IActionResult OnPost(string? returnUrl)
        {
            if (
                Account.UserName == null
                || Account.Password == null
                || _context.Accounts == null
                || Account == null
            )
            {
                return Page();
            }
            var account = _context.Accounts.FirstOrDefault(a => a.UserName == Account.UserName);

            if (account == null)
            {
                ModelState.AddModelError("Account.Username", "Username does not exist");
                return Page();
            }
            if (account.Password != Account.Password)
            {
                ModelState.AddModelError("Account.Password", "Password is incorrect");
                return Page();
            }
            VaSession.Set(HttpContext.Session, "Account", account);
            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToPage("./Index");
            }
            else
            {
                return LocalRedirect(returnUrl);
            }

        }
    }
}
