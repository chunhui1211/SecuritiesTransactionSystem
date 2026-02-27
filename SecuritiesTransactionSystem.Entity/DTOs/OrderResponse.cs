using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecuritiesTransactionSystem.Entity.DTOs
{
    public class OrderResponse
    {
        public Guid OrderId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime OrderTime { get; set; }
    }
}
