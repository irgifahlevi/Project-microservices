using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.Data
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDbContext _context;

        public ProductRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task Create(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            await _context.Products.AddAsync(product);
        }

        public Task Delete(int id, Product product)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetById(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            return product;
        }

        public async Task<IEnumerable<Product>> GetByName(string name)
        {
            var productsByName = await _context.Products.Where(p => p.Name.Contains(name)).ToListAsync();
            if (productsByName == null || !productsByName.Any())
            {
                throw new Exception("No products found.");
            }
            return productsByName;
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public async Task Update(int id, Product product)
        {
            try
            {
                var existingProduct = await GetById(product.ProductId);
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.Stock = product.Stock;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating product {ex.Message}");
            }
        }
    }
}