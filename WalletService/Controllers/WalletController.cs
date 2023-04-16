using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WalletService.Data;
using WalletService.Dtos;
using WalletService.Models;

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

        // Menampilkan semua wallet
        [HttpGet(Name = "GetAllWallet")]
        public async Task<IActionResult> GetAllWallet()
        {
            var wallet = await _walletRepo.GetAllWallet();
            var walletReadDtoList = _mapper.Map<IEnumerable<ReadWalletDto>>(wallet);
            return Ok(walletReadDtoList);
        }

        [HttpGet("{id}", Name = "GetWalletById")]
        public async Task<IActionResult> GetWalletById(int id)
        {
            try
            {
                var wallet = await _walletRepo.GetWalletById(id);
                var walletReadDtoList = _mapper.Map<ReadWalletDto>(wallet);
                return Ok(walletReadDtoList);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Create a new wallet
        [HttpPost]
        public async Task<IActionResult> CreateWallet(CreateWalletDto createWalletDto)
        {
            var walletModel = _mapper.Map<Wallet>(createWalletDto);
            await _walletRepo.CreateWallet(walletModel);
            _walletRepo.SaveChanges();
            var walletReadDto = _mapper.Map<ReadWalletDto>(walletModel);

            return Ok(walletReadDto);
        }



    }
}