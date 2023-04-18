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



        // public async Task CreateProduct()
        // {
        //     var products = await _client.GetAllProducts();
        //     foreach (var item in products)
        //     {
        //         _context.Add(new Product
        //         {
        //             ProductId = item.ProductId,
        //             Name = item.Name,
        //             Price = item.Price,
        //             Stock = item.Stock
        //         });
        //     }
        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"Could not save changes to the database: {ex.Message}");
        //     }
        // }

        public bool ExternalProductExists(int externalProductId)
        {
            return _context.Products.Any(p => p.ProductId == externalProductId);
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
    }
}