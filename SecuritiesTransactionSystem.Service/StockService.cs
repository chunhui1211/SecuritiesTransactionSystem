using SecuritiesTransactionSystem.Entity.Model;
using SecuritiesTransactionSystem.Service.Interface;
using System.Text.Json;

namespace SecuritiesTransactionSystem.Service
{
    public class StockService : IStockService
    {
        private readonly HttpClient _httpClient;
        public StockService(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<Stock?> GetLivePriceAsync(string symbol)
        {
            // 串接 TWSE API
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
