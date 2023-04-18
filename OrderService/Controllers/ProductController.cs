using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Dtos;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductOrderRepo _productOrderRepo;
        private readonly IMapper _mapper;

        public ProductController(IProductOrderRepo productOrderRepo, IMapper mapper)
        {
            _productOrderRepo = productOrderRepo;
            _mapper = mapper;
        }

        [HttpPost("Sync")]
        public async Task<ActionResult> SyncPlatforms()
        {
            try
            {
                // await _productOrderRepo.CreateProduct();
                return Ok("Platforms Synced");
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not sync platforms: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productOrderRepo.GetAllProduct();
            var walletReadDtoList = _mapper.Map<IEnumerable<ProductReadDto>>(products);
            return Ok(walletReadDtoList);
        }
    }
}