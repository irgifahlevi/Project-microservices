using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public ProductController(IProductRepo productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }

        // Menampilkan semua produk
        [HttpGet]
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

            return CreatedAtRoute(nameof(GetProductById),
                new { Id = readProductDto.ProductId }, readProductDto);
        }
    }
}