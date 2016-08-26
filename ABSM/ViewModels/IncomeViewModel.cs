using ABSM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABSM.ViewModels
{
    public class IncomeViewModel
    {
        public IEnumerable<OrderDetail> Items { get; set; }
        public decimal Total { get; set; }
      
    }
}