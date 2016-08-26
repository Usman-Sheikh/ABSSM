using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABSM.ViewModels
{
    public class CheckoutViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        [StringLength(160)]
        public string FullName { get; set; }

        
        [Required]
        [StringLength(70, MinimumLength = 3)]
        public string Address { get; set; }

        [Required]
        [StringLength(40)]
        public string City { get; set; }

        [Required]
        [StringLength(40)]
        public string State { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        [StringLength(10, MinimumLength = 5)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(40)]
        public string Country { get; set; }

        [Required]
        [StringLength(24)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

       
        public string Email { get; set; }

        public decimal Total { get; set; }

        [Required]
        public string CardNumber { get; set; }

        [Required]
        public string Cvv { get; set; }

        [Required]
        public string Month { get; set; }

        [Required]
        public string Year { get; set; }
    }
}