using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SignalRAssignment.Common;
using Microsoft.AspNetCore.Http;
using SignalRAssignment.Models;

namespace SignalRAssignment.Pages.Cart
{
    
    public class IndexModel : PageModel
    {
        private readonly PizzaStoreContext _context;
        public CartCRUD? cartCRUD;
        public List<CartItem> items;
        [BindProperty]
        public Models.Order Order { get; set; }
        public IndexModel(PizzaStoreContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method is used to add a product to the cart post request.
        /// </summary>
        /// <param name="productId">The id of the product to add to the cart.</param>
        /// <param name="quantity">The quantity of the product to add to the cart.</param>
        public IActionResult OnPostCartProcess([FromBody] CartProcessParam data)
        {
            try
            {
                cartCRUD = new CartCRUD(_context, HttpContext.Session);
                int productId = data.ProductId;
                int quantity = data.Quantity;
                switch (data.Action)
                {
                    case "add":
                        cartCRUD.AddToCart(productId, quantity);
                        break;
                }
                var res = new { success = true, totalQty = cartCRUD.GetTotalQty() };
                return new JsonResult(res);
            }
            catch (Exception e)
            {
                var res = new { success = false, message = e.Message };
                return new JsonResult(res);
            }
        }

        /// <summary>
        /// This method is used to get the cart.
        /// </summary>
        public void OnGet(int id, string handler)
        {
            cartCRUD = new CartCRUD(_context, HttpContext.Session);
            items= (List<CartItem>)cartCRUD.GetCart().CartItems;

            bool isLogged = false;
            Console.WriteLine(value: "Size: "+cartCRUD.GetCart().CartItems.Count);
            Console.WriteLine(value:"items: "+ items.Count );
            ViewData["listItem"] = cartCRUD.GetCart().CartItems;
            ViewData["count"] = cartCRUD.GetCart().Count;
            ViewData["Cart"] = cartCRUD.GetCart();

            if (VaSession.Get<Models.Account>(HttpContext.Session, "Account") != null)
            {
                isLogged = true;
               
                ViewData["IsLogged"] = isLogged;

            }
            switch (handler)
            {
                case "increment":
                    cartCRUD.UpdateQuantity(id, isAdd: true);
                    break;
                case "decrement":
                    cartCRUD.UpdateQuantity(id, isRemove: true);
                    break;
                case "delete":
                cartCRUD.RemoveFromCart(id);
                    break;
                case "clear":
                    cartCRUD.ClearCart();
                    break;

            }
            ViewData["IsLogged"] = isLogged;

        }


        /// <summary>
        /// This method is used to process the order.
        /// </summary>
       
        public IActionResult OnPost(Models.Order order, String idUseName)
        {

            order.OrderDate = DateTime.Now;
            var account = VaSession.Get<Models.Account>(HttpContext.Session, "Account");
            order.CustomerId = account.AccountId;
            Console.WriteLine(value: "POst: "+items.Count);
            foreach (var product in items)
            {

                var orderDetail = new OrderDetail()
                {
                    UnitPrice= (double)product.Product.UnitPrice,
                    Order = order,
                    Quantity = product.Quantity,

                };
                _context.OrderDetails.Add(orderDetail);
            }
            try
            {
                _context.Add(order);
                _context.SaveChanges();
                return RedirectToPage("/Index");
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                return Page();
            }
        }
    }
}
