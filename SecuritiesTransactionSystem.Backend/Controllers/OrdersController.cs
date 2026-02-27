using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecuritiesTransactionSystem.Entity.DTOs;
using SecuritiesTransactionSystem.Service.Interface;

namespace SecuritiesTransactionSystem.Backend.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// 建立委託單
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest order)
        {
            var response = await _orderService.CreateOrderAsync(order);
            return Ok(response);
        }

        /// <summary>
        /// 依照編號取得委託單
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _orderService.GetByIdAsync(id);
            return order == null ? NotFound() : Ok(order);
        }
    }
}
