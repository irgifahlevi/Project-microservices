using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductService.AsyncDataService;
using ProductService.Data;
using ProductService.Dtos;
using ProductService.Models;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepo _productRepo;
        private readonly IMapper _mapper;
        private readonly IMessageClient _messageClient;

        public ProductController(IProductRepo productRepo, IMapper mapper,
        IMessageClient messageClient)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _messageClient = messageClient;
        }

        // Menampilkan semua produk
        [HttpGet(Name = "GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            var products = await _productRepo.GetAll();
            var productsReadDtoList = _mapper.Map<IEnumerable<ReadProductDto>>(products);
            return Ok(productsReadDtoList);
        }

        // Get by Id
        [HttpGet("{id}", Name = "GetProductById")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productRepo.GetById(id);
            if (product != null)
            {
                var readProductDto = _mapper.Map<ReadProductDto>(product);
                return Ok(readProductDto);
            }
            return NotFound();
        }

        // Get product by name
        [HttpGet("name/{name}", Name = "GetProductByName")]
        public async Task<ActionResult<IEnumerable<ReadProductDto>>> GetProductByName(string name)
        {
            var productsByName = await _productRepo.GetByName(name);
            if (productsByName != null)
            {
                var readProductDtos = _mapper.Map<IEnumerable<ReadProductDto>>(productsByName);
                return Ok(readProductDtos);
            }
            return NotFound();
        }

        // Update product
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProductDto updateProductDto)
        {
            try
            {
                var product = _mapper.Map<Product>(updateProductDto);
                product.ProductId = id;
                await _productRepo.Update(id, product);
                _productRepo.SaveChanges();
                var readProductDto = _mapper.Map<ReadProductDto>(product);
                return Ok(readProductDto);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Create product
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto createProductDto)
        {
            var productModel = _mapper.Map<Product>(createProductDto);
            await _productRepo.Create(productModel);
            _productRepo.SaveChanges();
            var readProductDto = _mapper.Map<ReadProductDto>(productModel);

            try
            {
                //send async message    
                var productPublishedDto = _mapper.Map<ProductPublishDto>(readProductDto);
                productPublishedDto.Event = "Product_NewPublished";
                _messageClient.PublishNewProduct(productPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetProductById),
                new { Id = readProductDto.ProductId }, readProductDto);
        }


        // Update product
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productRepo.DeleteProduct(id);
                return RedirectToAction(nameof(GetAllProduct));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}