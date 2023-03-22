using System;
using System.Collections.Generic;

namespace SignalRAssignment.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public double Freight { get; set; }
        public string ShipAddress { get; set; } = null!;

        public virtual Customer Customer { get; set; } = null!;
    }
}
