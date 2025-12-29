using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCBookStore.Models
{
    public class ProductReview
    {
        [Key]
        public int ReviewId { get; set; }
        public int BookId { get; set; }
        public string UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public virtual Book Book { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}