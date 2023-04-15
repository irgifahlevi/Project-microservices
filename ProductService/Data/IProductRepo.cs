using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductService.Models;

namespace ProductService.Data
{
    public interface IProductRepo
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetById(int id);
        Task<IEnumerable<Product>> GetByName(string name);
        Task Create(Product product);
        Task Update(int id, Product product);
        Task DeleteProduct(int id);
        bool SaveChanges();
    }
}