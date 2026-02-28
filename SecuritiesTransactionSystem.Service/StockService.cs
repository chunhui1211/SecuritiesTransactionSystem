using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SecuritiesTransactionSystem.Domain.Config;
using SecuritiesTransactionSystem.Entity.Model;
using SecuritiesTransactionSystem.Service.Interface;
using System.Text.Json;

namespace SecuritiesTransactionSystem.Service
{
    public class StockService : IStockService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheOptions;
        public StockService(HttpClient httpClient, IMemoryCache cache, IOptions<CacheSettings> cacheSettings)
        {
            _httpClient = httpClient;
            _cache = cache;
            var settings = cacheSettings.Value;
            _cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(settings.StockPriceExpirationSeconds))
                .SetSlidingExpiration(TimeSpan.FromSeconds(settings.StockPriceSlidingExpirationSeconds))
                .SetPriority(CacheItemPriority.Normal);
        }

        public async Task<Stock?> GetLivePriceAsync(string symbol)
        {
            string cacheKey = $"Stock_{symbol}";
            if (_cache.TryGetValue(cacheKey, out Stock? cachedStock))
            {
                return cachedStock;
            }

            var stock = await FetchStockFromTwseAsync(symbol);

            if (stock != null)
            {
                _cache.Set(cacheKey, stock, _cacheOptions);
            }

            return stock;
        }

        /// <summary>
        /// 串接 TWSE API
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        private async Task<Stock?> FetchStockFromTwseAsync(string symbol)
        {
            var url = $"https://mis.twse.com.tw/stock/api/getStockInfo.jsp?ex_ch=tse_{symbol}.tw";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            var msgArray = doc.RootElement.GetProperty("msgArray");

            if (msgArray.GetArrayLength() == 0) return null;

            var first = msgArray[0];
            return new Stock
            {
                Symbol = symbol,
                Name = first.GetProperty("n").GetString() ?? "",
                LastPrice = decimal.Parse(first.GetProperty("z").GetString() ?? "0")
            };
        }
    }
}
