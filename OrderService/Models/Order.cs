using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public DateOnly OrderDate { get; set; }
        public string Username { get; set; }
    }
}