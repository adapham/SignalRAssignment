using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SignalRAssignment.Common;
using SignalRAssignment.Models;

namespace SignalRAssignment.Purchase
{
    [MemberPermission]
    public class IndexModel : PageModel
    {
        private readonly PizzaStoreContext _context;

        public IndexModel(PizzaStoreContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get; set; } = default!;
        public List<Purchase> purchases { get; set; } = default!;
        public List<Product> products { get; set; } = default!;

        Dictionary<int,String> pairs = new Dictionary<int, String>();

        public async Task OnGetAsync()
        {
            if (_context.Orders != null)
            {
                Order = await _context.Orders
                    .Include(i => i.OrderDetails)
                    .Include(o => o.Account)
                    .ToListAsync();

            }
            foreach (var order in Order)
            {

                List<OrderDetail> orderDetail = _context.OrderDetails
                     .Where(x => x.OrderId == order.OrderId).ToList();
                foreach (var o in orderDetail)
            {

                    //pairs.Add(o.OrderId,)

                    //var purchase = new Purchase()
                    //{
                    //    OrderId=order.OrderId,
                    //    OrderDate=order.OrderDate,
                    //    ShippedDate=order.ShippedDate,
                    //    ProductImage= 
                    //}
                    //purchases.Add(o);
                }

            }

            }
        public class Purchase{
            public int OrderId { get; set;}
            public DateTime OrderDate { get; set;}
            public DateTime ShippedDate { get; set;}
            public string ProductImage { get; set;}
        }
    }
   
   

    
}
