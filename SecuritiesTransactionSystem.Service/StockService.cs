using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SecuritiesTransactionSystem.Domain.Config;
using SecuritiesTransactionSystem.Domain.DTOs;
using SecuritiesTransactionSystem.Entity.Exceptions;
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
        private static readonly List<Stock> _mockStocks = new()
        {
            new Stock { Symbol = "2330", Name = "台積電" },
            new Stock { Symbol = "2317", Name = "鴻海" },
            new Stock { Symbol = "2454", Name = "聯發科" },
            new Stock { Symbol = "2308", Name = "台達電" },
            new Stock { Symbol = "2382", Name = "廣達" },
            new Stock { Symbol = "2881", Name = "富邦金" },
            new Stock { Symbol = "2882", Name = "國泰金" },
            new Stock { Symbol = "2412", Name = "中華電" },
            new Stock { Symbol = "1301", Name = "台塑" },
            new Stock { Symbol = "2603", Name = "長榮" },
            new Stock { Symbol = "0050", Name = "元大台灣50" },
            new Stock { Symbol = "0056", Name = "元大高股息" },
            new Stock { Symbol = "00878", Name = "國泰永續高股息" },
            new Stock { Symbol = "3008", Name = "大立光" },
            new Stock { Symbol = "2357", Name = "華碩" }
        };

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

        public async Task<PagedResult<Stock>> SearckStockAsync(StockSearchRequest request)
        {
            var query = _mockStocks.AsQueryable();

            if (!string.IsNullOrEmpty(request.Symbol))
            {
                query = query.Where(s => s.Symbol == request.Symbol);
            }

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(s => s.Symbol.Contains(request.Keyword) || s.Name.Contains(request.Keyword));
            }

            var totalCount = query.Count();

            var items = query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

            return new PagedResult<Stock>
            {
                Items = items,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
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

            if (!doc.RootElement.TryGetProperty("msgArray", out var msgArray) || msgArray.GetArrayLength() == 0)
            {
                throw new BusinessException($"找不到股票代號: {symbol}，請確認輸入是否正確", 400);
            }

            var first = msgArray[0];

            var priceStr = first.GetProperty("o").GetString()?.Trim();

            if (string.IsNullOrEmpty(priceStr) || priceStr == "-")
            {
                throw new BusinessException($"找不到股票代號 '{symbol}' 或該股票目前無成交資訊", 400);
            }

            if (!decimal.TryParse(priceStr, out decimal lastPrice))
            {
                throw new BusinessException($"股票代號 '{symbol}' 價格格式錯誤", 400);
            }

            return new Stock
            {
                Symbol = symbol,
                Name = first.GetProperty("n").GetString() ?? "未知名稱",
                LastPrice = lastPrice
            };
        }
    }
}
