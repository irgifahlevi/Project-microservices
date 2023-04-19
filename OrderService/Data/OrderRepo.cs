using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderService.Dtos;
using OrderService.Models;

namespace OrderService.Data
{
    public class OrderRepo : IOrderRepo
    {
        private AppDbContext _context;

        public OrderRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderReadDto>> GetAllOrders()
        {
            var orders = await _context.Orders
            .Include(o => o.Product) // menyertakan detail produk
            .Include(o => o.Wallet) // menyertakan detail wallet
            .ToListAsync();

            // Mapping data order
            return orders.Select(o => new OrderReadDto
            {
                OrderId = o.OrderId,
                ProductName = o.Product.Name,
                Price = o.Product.Price * o.Qty,
                Qty = o.Qty,
                OrderDate = o.OrderDate,
                Username = o.Wallet.Username
            });
        }

        public async Task<OrderReadDto> GetOrder(int id)
        {
            var order = await _context.Orders
            .Include(o => o.Product) // menyertakan detail produk
            .Include(o => o.Wallet) // menyertakan detail wallet
            .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                throw new Exception("Belum ada data order");
            }

            var readOrderDto = new OrderReadDto
            {
                OrderId = order.OrderId,
                ProductName = order.Product.Name,
                Price = order.Product.Price * order.Qty,
                Qty = order.Qty,
                OrderDate = order.OrderDate,
                Username = order.Wallet.Username
            };

            return readOrderDto;
        }

        public async Task<Order> SaveOrder(Order order)
        {
            // Cek stok produk
            var product = await _context.Products.FindAsync(order.ProductId);
            if (product == null || product.Stock < order.Qty)
            {
                throw new InvalidOperationException("Produk kosong, stock tidak mencukupi.");
            }

            // Kurangi stok produk
            product.Stock -= order.Qty;

            // Cek cash pada wallet
            var wallet = await _context.Wallets.FindAsync(order.WalletId);
            if (wallet == null || wallet.Cash < (order.Qty * product.Price))
            {
                throw new InvalidOperationException("Cash anda tidak cukup.");
            }

            // Kurangi cash pada wallet
            wallet.Cash -= (order.Qty * product.Price);

            // Tambahkan pesanan dan simpan perubahan
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }
    }
}