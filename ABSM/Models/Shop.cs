using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABSM.Models
{
    public class Shop
    {
        public int ShopID { get; set; }

        [Display(Name = "Shop Name")]
        [Required]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^([0-9]){10,11}$", ErrorMessage = "Enter Valid Phone Number")]
        public string Phone { get; set; }

        [Required]
        [RegularExpression("^([0-9]){10,11}$",ErrorMessage ="Enter Valid Mobile Number")]
        public string Mobile { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Address { get; set; }
        public string ImageUrl { get; set; }
        public string About { get; set; }
        
        [RegularExpression(@"/^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$/",ErrorMessage ="Please enter valid Url")]
        public string FacbookLink { get; set; }
       
        [RegularExpression(@"/^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$/", ErrorMessage = "Please enter valid Url")]
        public string TwitterLink { get; set; }

        [Required]
        public string UserName { get; set; }

    }
}