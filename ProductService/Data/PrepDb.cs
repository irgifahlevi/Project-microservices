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
                    Name = "Dotnet Core",
                    Description = "Microsoft",
                    Price = 2000,
                    Stock = 5
                },
                new Product()
                {
                    Name = "SQL Server Express",
                    Description = "Microsoft",
                    Price = 2000,
                    Stock = 5
                },
                new Product()
                {
                    Name = "Kubernetes",
                    Description = "Cloud Native Computing Foundation",
                    Price = 2000,
                    Stock = 4
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