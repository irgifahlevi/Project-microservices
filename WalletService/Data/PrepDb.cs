using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletService.Models;

namespace WalletService.Data
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
            if (!context.Wallets.Any())
            {
                Console.WriteLine("--> Seeding Data...");
                context.Wallets.AddRange(
                new Wallet()
                {
                    Username = "Mamansuper",
                    FullName = "Maman Firmansah",
                    Cash = 2000
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