using SecuritiesTransactionSystem.Entity.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecuritiesTransactionSystem.Repository.Interface
{
    public interface IOrderRepository
    {
        Task<Order> AddAsync(Order order);
        Task<Order?> GetByIdAsync(Guid id);
        Task<IEnumerable<Order>> GetBySymbolAsync(string symbol);
    }
}
