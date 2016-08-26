using ABSM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABSM.ViewModels
{
    public class CategoryProductViewModel
    {
        public IEnumerable<Category> categories { get; set; }
        public IEnumerable<Product> products { get; set; }

    }
}