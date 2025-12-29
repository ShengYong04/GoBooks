using MVCBookStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace MVCBookStore.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Address Line 1 is required")]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2 (Optional)")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "Postcode is required")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Invalid Postcode (e.g. 50450)")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        public string State { get; set; }
    }
}