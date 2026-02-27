using SecuritiesTransactionSystem.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecuritiesTransactionSystem.Entity.DTOs
{
    public class CreateOrderRequest
    {
        public string Symbol { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public SideType Side { get; set; }
    }
}
