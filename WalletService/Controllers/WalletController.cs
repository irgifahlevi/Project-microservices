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

        // Get wallet by id
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

        // Get wallet by name
        [HttpGet("name/{name}", Name = "GetWalletByName")]
        public async Task<ActionResult<IEnumerable<ReadWalletDto>>> GetProductByName(string name)
        {
            try
            {
                var walletByName = await _walletRepo.GetWalletByName(name);
                var walletReadDtoList = _mapper.Map<IEnumerable<ReadWalletDto>>(walletByName);
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

        // Update wallet user
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWallet(int id, UpdateWalletDto updateWalletDto)
        {
            try
            {
                var walletModel = _mapper.Map<Wallet>(updateWalletDto);
                walletModel.WalletId = id;
                await _walletRepo.UpdateWallet(id, walletModel);
                _walletRepo.SaveChanges();
                var readWalletDto = _mapper.Map<ReadWalletDto>(walletModel);
                return Ok(readWalletDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("{id}/topup")]
        public async Task<IActionResult> TopUp(int id, TopupWalletDto topupWalletDto)
        {
            try
            {
                var walletModel = _mapper.Map<Wallet>(topupWalletDto);
                walletModel.WalletId = id;
                await _walletRepo.TopUp(id, walletModel);
                _walletRepo.SaveChanges();
                var walletReadDto = _mapper.Map<ReadWalletDto>(walletModel);

                return Ok("Payment topup successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}