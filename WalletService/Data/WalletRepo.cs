using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WalletService.Models;

namespace WalletService.Data
{
    public class WalletRepo : IWalletRepo
    {
        private readonly AppDbContext _context;

        public WalletRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateWallet(Wallet wallet)
        {
            if (wallet == null)
            {
                throw new ArgumentNullException(nameof(wallet));
            }
            await _context.Wallets.AddAsync(wallet);
        }

        public Task DeleteWallet(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Wallet>> GetAllWallet()
        {
            return await _context.Wallets.ToListAsync();
        }

        public async Task<Wallet> GetWalletById(int id)
        {
            try
            {
                var walletById = await _context.Wallets.FirstOrDefaultAsync(p => p.WalletId == id);
                if (walletById == null)
                {
                    throw new Exception($"wallet id {id} not found");
                }
                return walletById;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error {ex.Message}");
            }
        }

        public async Task<IEnumerable<Wallet>> GetWalletByName(string name)
        {
            try
            {
                var walletByName = await _context.Wallets.Where(p => p.FullName.Contains(name)).ToListAsync();
                if (walletByName == null || !walletByName.Any())
                {
                    throw new Exception($"wallet {name} not found");
                }
                return walletByName;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error {ex.Message}");
            }
        }

        public Task<Wallet> Order(int id)
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public async Task<bool> TopUp(int id, Wallet wallet)
        {
            try
            {
                var existingWallet = await GetWalletById(wallet.WalletId);

                if (existingWallet == null)
                {
                    throw new Exception("Wallet id not found");
                }

                decimal topupWallet = existingWallet.Cash + wallet.Cash;
                existingWallet.Cash = topupWallet;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error {ex.Message}");
            }
        }

        public async Task UpdateWallet(int id, Wallet wallet)
        {
            try
            {
                var existingWallet = await GetWalletById(wallet.WalletId);
                existingWallet.Username = wallet.Username;
                existingWallet.FullName = wallet.FullName;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error {ex.Message}");
            }
        }
    }
}