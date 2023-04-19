using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.Dtos;
using OrderService.Models;

namespace OrderService.Data
{
    public interface IOrderRepo
    {
        Task<Order> SaveOrder(Order order);
        Task<OrderReadDto> GetOrder(int id);
        Task<IEnumerable<OrderReadDto>> GetAllOrders();
    }
}