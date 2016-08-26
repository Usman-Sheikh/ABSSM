using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABSM.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }

        [Required]
        public string TransactionId { get; set; }
        
        [Required]
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
        [StringLength(10, MinimumLength = 5)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(40)]
        public string Country { get; set; }

        [Required]
        [StringLength(24)]
        public string Phone { get; set; }

        [Required]
        public string Email { get; set; }

        public string  UserId { get; set; }

        [Required]
        public decimal Total { get; set; }

        public string PaymentMode  { get; set; }

        public virtual List<OrderDetail> OrderDetails { get; set; }

        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }
    }
}