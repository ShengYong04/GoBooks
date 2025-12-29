using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCBookStore.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }

        public virtual Order Order { get; set; }
        public virtual Book Book { get; set; }
    }
}