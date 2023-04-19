using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Dtos
{
    public class TopupWalletPublishDto
    {
        public int WalletId { get; set; }
        public decimal Cash { get; set; }
        public string Event { get; set; }
    }
}