using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.Models;

namespace OrderService.Data
{
    public interface IProductOrderRepo
    {
        Task CreateProduct(Product product);
        Task CreateWallet(Wallet wallet);
        bool WalletExists(int walletId);
        void AddCash(int walletId, decimal cash);
        void TopupCash(int walletId, decimal cash);
        Task<IEnumerable<Product>> GetAllProduct();
        Task<IEnumerable<Wallet>> GetAllWallet();
        // void CreateProduct(Product product);
        bool ProductExits(int productId);
        bool ExternalProductExists(int externalProductId);
        bool ExternalWalletExists(int externalWalletId);
        bool SaveChanges();
        // IEnumerable<Command> GetCommandsForPlatform(int platformId); 
        // Command GetCommand(int platformId, int commandId);
        // void CreateCommand(int platformId, Command command);
    }
}