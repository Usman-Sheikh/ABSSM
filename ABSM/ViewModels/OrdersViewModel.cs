using ABSM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABSM.ViewModels
{
    public class OrdersViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }


    }
}