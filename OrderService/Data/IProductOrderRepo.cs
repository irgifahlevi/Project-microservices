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
        Task<IEnumerable<Product>> GetAllProduct();
        // void CreateProduct(Product product);
        bool ProductExits(int productId);
        bool ExternalProductExists(int externalProductId);
        bool SaveChanges();
        // IEnumerable<Command> GetCommandsForPlatform(int platformId); 
        // Command GetCommand(int platformId, int commandId);
        // void CreateCommand(int platformId, Command command);
    }
}