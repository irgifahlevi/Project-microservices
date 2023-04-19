using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductService.Models;

namespace ProductService.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateAsyncScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
            if (!context.Products.Any())
            {
                Console.WriteLine("--> Seeding Data...");
                context.Products.AddRange(
                new Product()
                {
                    Name = "Sabun",
                    Description = "Sabun cuci piring",
                    Price = 1000,
                    Stock = 5
                });
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> Sudah ada data..");
            }
        }
    }
}