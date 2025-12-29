using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCBookStore.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }

        public int CartId { get; set; }

        public int BookId { get; set; }

        [Range(1, 999)]
        public int Quantity { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual Book Book { get; set; }
    }
}