using MVCBookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCBookStore.ViewModels
{
    public class HomeViewModel
    {
        public List<Book> Books { get; set; }
        public List<Genre> Genres { get; set; }
        public int? GenreId { get; set; }
        public string SortOrder { get; set; }
        public string Search { get; set; }

        // Cart
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}