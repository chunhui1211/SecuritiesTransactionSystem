using SecuritiesTransactionSystem.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecuritiesTransactionSystem.Service.Interface
{
    public interface IStockService
    {
        /// <summary>
        /// 取得單一股票即時資訊 (串接外部 API)
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        Task<Stock?> GetLivePriceAsync(string symbol);
    }
}
