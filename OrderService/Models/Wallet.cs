using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Models
{
    public class Wallet
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public decimal Cash { get; set; }
    }
}