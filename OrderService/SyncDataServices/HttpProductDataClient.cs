using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using OrderService.Dtos;

namespace OrderService.SyncDataServices
{
    public class HttpProductDataClient : IProductDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpProductDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<IEnumerable<ProductReadDto>> GetAllProducts()
        {
            var response = await _httpClient.GetAsync(_configuration["ProductService"]);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.Write($"{content}");
                var products = JsonSerializer.Deserialize<IEnumerable<ProductReadDto>>(content);
                if (products != null)
                {
                    Console.WriteLine($"{products.Count()} products returned from products Service");
                    return products;
                }
                throw new Exception("No products found");
            }
            else
            {
                throw new Exception("Unable to reach products Service");
            }
        }
    }
}