using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCBookStore.Models
{
    public class Author
    {
        public int AuthorID { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public ICollection<Book> Books { get; set; }
    }
}