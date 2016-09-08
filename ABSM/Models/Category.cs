using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABSM.Models
{
    public class Category
    {   
        public int CategoryID { get; set; }

        [Display(Name = "Category Name")]
        public string Name { get; set; }
        public Shop Shop { get; set; }
        public int ShopID { get; set; }
    }
}