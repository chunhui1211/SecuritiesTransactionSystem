using Microsoft.EntityFrameworkCore;
using SecuritiesTransactionSystem.Entity.Model;
using System.Collections.Generic;

namespace SecuritiesTransactionSystem.Repository.Data
{
    public class TradingDbContext: DbContext
    {
        public TradingDbContext(DbContextOptions<TradingDbContext> options) : base(options) { }
        public DbSet<Order> Orders => Set<Order>();
    }
}
