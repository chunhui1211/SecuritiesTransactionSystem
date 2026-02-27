using Microsoft.EntityFrameworkCore;

namespace SecuritiesTransactionSystem.Repository
{
    public class TradingDbContext : DbContext
    {
        public TradingDbContext(DbContextOptions<TradingDbContext> options) : base(options) { }
    }
}
