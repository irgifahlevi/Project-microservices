using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WalletService.Data;
using WalletService.Dtos;

namespace WalletService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletRepo _walletRepo;
        private readonly IMapper _mapper;

        public WalletController(IWalletRepo walletRepo, IMapper mapper)
        {
            _walletRepo = walletRepo;
            _mapper = mapper;
        }

        [HttpGet(Name = "GetAllWallet")]
        public async Task<IActionResult> GetAllWallet()
        {
            var wallet = await _walletRepo.GetAllWallet();
            var walletReadDtoList = _mapper.Map<IEnumerable<ReadWalletDto>>(wallet);
            return Ok(walletReadDtoList);
        }
    }
}