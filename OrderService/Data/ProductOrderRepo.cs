using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;
using OrderService.SyncDataServices;

namespace OrderService.Data
{
    public class ProductOrderRepo : IProductOrderRepo
    {
        private readonly AppDbContext _context;
        private readonly IProductDataClient _client;

        public ProductOrderRepo(AppDbContext context, IProductDataClient client)
        {
            _context = context;
            _client = client;
        }

        public bool ExternalProductExists(int externalProductId)
        {
            return _context.Products.Any(p => p.ProductId == externalProductId);
        }

        public bool ExternalWalletExists(int externalWalletId)
        {
            return _context.Wallets.Any(w => w.WalletId == externalWalletId);
        }

        public async Task<IEnumerable<Product>> GetAllProduct()
        {
            return await _context.Products.ToListAsync();
        }

        public bool ProductExits(int productId)
        {
            return _context.Products.Any(p => p.ProductId == productId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }


        public async Task CreateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            await _context.Products.AddAsync(product);
        }

        public async Task CreateWallet(Wallet wallet)
        {
            if (wallet == null)
            {
                throw new ArgumentNullException(nameof(wallet));
            }
            await _context.Wallets.AddAsync(wallet);
        }

        public void AddCash(int walletId, decimal cash)
        {
            var wallet = _context.Wallets.FirstOrDefault(w => w.WalletId == walletId);
            if (wallet != null)
            {
                wallet.Cash += cash;
                _context.SaveChanges();
            }
        }

        public async Task<IEnumerable<Wallet>> GetAllWallet()
        {
            return await _context.Wallets.ToListAsync();
        }

        public void TopupCash(int walletId, decimal cash)
        {
            var wallet = _context.Wallets.FirstOrDefault(w => w.WalletId == walletId);
            if (wallet != null)
            {
                wallet.Cash += cash;
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentException($"Wallet with Id '{walletId}' does not exist");
            }
        }

        public bool WalletExists(int walletId)
        {
            return _context.Wallets.Any(w => w.WalletId == walletId);
        }
    }
}