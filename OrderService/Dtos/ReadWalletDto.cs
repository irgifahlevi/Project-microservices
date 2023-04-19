using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Dtos
{
    public class ReadWalletDto
    {
        public int WalletId { get; set; }
        public string Username { get; set; }
        public decimal Cash { get; set; }
    }
}