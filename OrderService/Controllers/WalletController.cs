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
    public class WalletController : ControllerBase
    {
        private readonly IProductOrderRepo _productOrderRepo;
        private readonly IMapper _mapper;

        public WalletController(IProductOrderRepo productOrderRepo, IMapper mapper)
        {
            _productOrderRepo = productOrderRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWallet()
        {
            var wallets = await _productOrderRepo.GetAllWallet();
            var walletReadDtoList = _mapper.Map<IEnumerable<ReadWalletDto>>(wallets);
            return Ok(walletReadDtoList);
        }
    }
}