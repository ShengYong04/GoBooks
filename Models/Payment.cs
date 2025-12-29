using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVCBookStore.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        public string UserId { get; set; }

        public string PaymentMethod { get; set; }

        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string Cvv { get; set; }
        public string BillingAddress { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pending";
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }

}