using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Dtos;
using OrderService.Models;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IMapper _mapper;

        public OrderController(IOrderRepo orderRepo, IMapper mapper)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderReadDto>>> GetAllOrdersWithDetails()
        {
            try
            {
                var orders = await _orderRepo.GetAllOrders();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult<OrderReadDto>> GetOrder(int id)
        {
            var orderDto = await _orderRepo.GetOrder(id);

            if (orderDto == null)
            {
                return NotFound();
            }
            return Ok(orderDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto createOrderDto)
        {

            try
            {
                var order = _mapper.Map<Order>(createOrderDto);
                order.OrderDate = DateTime.Now;
                var createdOrder = await _orderRepo.SaveOrder(order);
                var orderDto = _mapper.Map<OrderReadDto>(createdOrder);
                var orderCreateDto = await _orderRepo.GetOrder(createdOrder.OrderId);
                return CreatedAtRoute(nameof(GetOrder),
                 new { id = createdOrder.OrderId }, orderCreateDto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}