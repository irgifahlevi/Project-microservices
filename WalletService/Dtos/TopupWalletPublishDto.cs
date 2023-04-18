using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WalletService.Dtos
{
    public class TopupWalletPublishDto
    {
        public decimal Cash { get; set; }
        public string Event { get; set; } = string.Empty;
    }
}