namespace Reporting.Infrastructure.Contexts
{
    using Reporting.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System.Threading;
    using System.Threading.Tasks;

    public class ReportingDbContext : DbContext
    {
        public ReportingDbContext(
            DbContextOptions<ReportingDbContext> options) : base(options)
        {
        }

        #region DbSet Region - Do Not Delete

        public DbSet<ReportRequest> ReportRequests { get; set; }
        #endregion DbSet Region - Do Not Delete



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReportRequest>().Property(p => p.ReportId).ValueGeneratedNever();
        }
    }
}