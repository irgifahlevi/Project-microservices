using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductService.Dtos;

namespace ProductService.AsyncDataService
{
    public interface IMessageClient
    {
        void PublishNewProduct(ProductPublishDto productPublishDto);
    }
}