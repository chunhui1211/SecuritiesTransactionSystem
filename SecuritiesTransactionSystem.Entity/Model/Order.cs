using SecuritiesTransactionSystem.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecuritiesTransactionSystem.Entity.Model
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Symbol { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public SideType Side { get; set; }
        public DateTime CreatedTime { get; set; } = DateTime.Now;
    }
}
