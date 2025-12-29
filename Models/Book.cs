using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCBookStore.Models
{
    public class Book
    {
        public int BookId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be 0 or greater.")]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // --- ISBN Field ---
        [Required]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "ISBN must be between 10 and 13 characters.")]
        public string ISBN { get; set; }

        // --- Genre Relation ---
        [Required]
        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }

        // --- Quantity Field ---
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be 0 or greater.")]
        public int Quantity { get; set; } = 0;

        // Navigation Properties
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
