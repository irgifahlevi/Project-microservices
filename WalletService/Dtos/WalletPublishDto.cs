using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalletService.Dtos
{
    public class WalletPublishDto
    {
        public int WalletId { get; set; }
        public string Username { get; set; }
        public decimal Cash { get; set; }
        public string Event { get; set; } = string.Empty;
    }
}