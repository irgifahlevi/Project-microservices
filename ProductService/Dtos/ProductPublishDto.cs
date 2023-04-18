using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Dtos
{
    public class ProductPublishDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string Event { get; set; } = string.Empty;
    }
}