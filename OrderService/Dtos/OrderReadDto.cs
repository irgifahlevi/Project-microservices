using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Dtos
{
    public class OrderReadDto
    {
        public int OrderId { get; set; }
        public string ProductName { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public DateTime OrderDate { get; set; }
        public string Username { get; set; }
    }
}