using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABSM.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int Quantity { get; set; }
        public Shop Shop { get; set; }
        public int ShopID { get; set; }
    }
}