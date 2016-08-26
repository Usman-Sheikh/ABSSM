using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABSM.Models
{
    public class Complain
    {
        public int ComplainID { get; set; }
        [Required]
        public string Email { get; set; }
        public string TransactionId { get; set; }
        public string Phone { get; set; }
        [Required]
        public string Message { get; set; }
        public int ShopID { get; set; }
        public virtual Shop Shop { get; set; }
    }
}