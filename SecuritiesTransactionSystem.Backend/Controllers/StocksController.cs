using Microsoft.AspNetCore.Mvc;
using SecuritiesTransactionSystem.Service.Interface;

namespace SecuritiesTransactionSystem.Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly ILogger<StocksController> _logger;
        private readonly IStockService _stockService;

        public StocksController(ILogger<StocksController> logger, IStockService stockService)
        {
            _logger = logger;
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
    }
}
