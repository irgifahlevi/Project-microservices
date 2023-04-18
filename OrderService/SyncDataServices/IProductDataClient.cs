using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.Dtos;

namespace OrderService.SyncDataServices
{
    public interface IProductDataClient
    {
        Task<IEnumerable<ProductReadDto>> GetAllProducts();
    }
}