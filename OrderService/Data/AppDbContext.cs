using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
    }
}