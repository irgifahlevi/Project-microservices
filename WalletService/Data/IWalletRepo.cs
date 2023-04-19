using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletService.Models;

namespace WalletService.Data
{
    public interface IWalletRepo
    {
        Task<IEnumerable<Wallet>> GetAllWallet();
        Task<Wallet> GetWalletById(int id);
        Task<IEnumerable<Wallet>> GetWalletByName(string name);
        Task CreateWallet(Wallet wallet);
        Task UpdateWallet(int id, Wallet wallet);
        Task<bool> TopUp(int id, Wallet wallet);
        Task<Wallet> Order(int id);
        bool SaveChanges();
    }
}