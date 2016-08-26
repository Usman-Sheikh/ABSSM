using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABSM.Models
{
    public class Category
    {   
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public Shop Shop { get; set; }
        public int ShopID { get; set; }
    }
}