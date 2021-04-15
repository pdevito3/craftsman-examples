namespace Billing.Infrastructure.Contexts
{
    using Billing.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System.Threading;
    using System.Threading.Tasks;

    public class BillingDbContext : DbContext
    {
        public BillingDbContext(
            DbContextOptions<BillingDbContext> options) : base(options) 
        {
        }

        #region DbSet Region - Do Not Delete
        public DbSet<InsuranceProvider> InsuranceProviders { get; set; }
        #endregion
    }
}