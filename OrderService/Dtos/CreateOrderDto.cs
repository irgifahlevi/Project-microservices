using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Dtos
{
    public class CreateOrderDto
    {
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public int WalletId { get; set; }

    }
}