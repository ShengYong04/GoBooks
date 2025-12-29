using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCBookStore.Models
{
    public class Genre
    {
        [Key]
        public int GenreId { get; set; }

        [Required]
        [Display(Name = "Genre Name")]
        public string GenreName { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<Book> Books { get; set; }
    }
}