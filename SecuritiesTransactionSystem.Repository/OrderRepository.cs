using SecuritiesTransactionSystem.Entity.Model;
using SecuritiesTransactionSystem.Repository.Data;
using SecuritiesTransactionSystem.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecuritiesTransactionSystem.Repository
{
    public class OrderRepository: IOrderRepository
    {
        private readonly TradingDbContext _context;

        public OrderRepository(TradingDbContext context)
        {
            _context = context;
        }

        public async Task<Order> AddAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _context.Orders.FindAsync(id);
        }
    }
}
