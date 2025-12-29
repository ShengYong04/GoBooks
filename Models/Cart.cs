using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCBookStore.Models
{
    public class Cart
    {
        public int CartId { get; set; }

        [Required]
        public string UserId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}