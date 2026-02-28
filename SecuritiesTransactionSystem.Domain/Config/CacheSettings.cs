using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecuritiesTransactionSystem.Domain.Config
{
    public class CacheSettings
    {
        public int StockPriceExpirationSeconds { get; set; }
        public int StockPriceSlidingExpirationSeconds { get; set; }
    }
}
