using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCBookStore.Models
{
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(200)]
        public string Subject { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}