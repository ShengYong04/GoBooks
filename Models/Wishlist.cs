using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCBookStore.Models
{
    public class Wishlist
    {
        public int WishlistId { get; set; }
        public string UserId { get; set; }
        public int BookId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public virtual ApplicationUser User { get; set; }
        public virtual Book Book { get; set; }
    }
}