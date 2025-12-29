using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCBookStore.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public string UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        public bool IsCancelled { get; set; } = false;

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}