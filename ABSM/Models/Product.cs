using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABSM.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Display(Name = "Product Name")]
        [Required]
        public string Name { get; set; }
        [Required]
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        [Required]
        [RegularExpression(@"^\d*\.?\d+$", ErrorMessage = "Please enter postive numbers")]
        public decimal Price { get; set; }
       
        public string ImageUrl { get; set; }
        [Required]
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }

        [Display(Name = "Long Description")]
        [Required]
        public string LongDescription { get; set; }

        [Required]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Please enter postive numbers")]
        public int Quantity { get; set; }
        public Shop Shop { get; set; }
        [Required]
        public int ShopID { get; set; }
    }
}