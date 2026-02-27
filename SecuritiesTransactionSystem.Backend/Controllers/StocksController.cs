using Microsoft.AspNetCore.Mvc;
using SecuritiesTransactionSystem.Entity.Model;
using SecuritiesTransactionSystem.Service.Interface;

namespace SecuritiesTransactionSystem.Backend.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly ILogger<StocksController> _logger;
        private readonly IStockService _stockService;

        private static readonly List<Stock> _mockStocks = new()
        {
            new Stock { Symbol = "2330", Name = "台積電" },
            new Stock { Symbol = "2317", Name = "鴻海" }
        };

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

        /// <summary>
        /// 關鍵字搜尋股票
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Search([FromQuery] string? symbol, [FromQuery] string? keyword)
        {
            var query = _mockStocks.AsQueryable();
            if (!string.IsNullOrEmpty(symbol)) query = query.Where(s => s.Symbol == symbol);
            if (!string.IsNullOrEmpty(keyword)) query = query.Where(s => s.Name.Contains(keyword));
            return Ok(query.ToList());
        }
    }
}
