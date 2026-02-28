using Microsoft.AspNetCore.Mvc;
using SecuritiesTransactionSystem.Domain.DTOs;
using SecuritiesTransactionSystem.Service.Interface;

namespace SecuritiesTransactionSystem.Backend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StocksController(IStockService stockService)
        {
            _stockService = stockService;
        }

        /// <summary>
        /// 取得單一股票即時資訊 (串接外部 API)
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        [HttpGet("{symbol}")]
        public async Task<IActionResult> GetStock(string symbol)
        {
            var stock = await _stockService.GetLivePriceAsync(symbol);
            return stock == null ? NotFound() : Ok(stock);
        }

        /// <summary>
        /// 關鍵字搜尋股票
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] StockSearchRequest request)
        {
            var stock = await _stockService.SearckStockAsync(request);
            return stock == null ? NotFound() : Ok(stock);
        }
    }
}
