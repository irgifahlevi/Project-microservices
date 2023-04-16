using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletService.Models;

namespace WalletService.Data
{
    public class WalletRepo : IWalletRepo
    {
        public Task CreateWallet(Wallet wallet)
        {
            throw new NotImplementedException();
        }

        public Task DeleteWallet(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Wallet>> GetAllWallet()
        {
            throw new NotImplementedException();
        }

        public Task<Wallet> GetWalletById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Wallet>> GetWalletByName(string name)
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task UpdateWallet(int id, Wallet wallet)
        {
            throw new NotImplementedException();
        }
    }
}