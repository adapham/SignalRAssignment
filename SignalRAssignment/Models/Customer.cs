using System;
using System.Collections.Generic;

namespace SignalRAssignment.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public string Password { get; set; } = null!;
        public string ContactName { get; set; } = null!;
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public int CustomerId { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
