using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WalletService.Models
{
    public class Wallet
    {
        [Key]
        public int WalletId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public decimal Cash { get; set; }
    }
}