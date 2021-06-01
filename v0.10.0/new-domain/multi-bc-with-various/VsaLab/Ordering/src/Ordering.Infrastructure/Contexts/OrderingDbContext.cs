namespace Ordering.Infrastructure.Contexts
{
    using Ordering.Core.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System.Threading;
    using System.Threading.Tasks;

    public class OrderingDbContext : DbContext
    {
        public OrderingDbContext(
            DbContextOptions<OrderingDbContext> options) : base(options)
        {
        }

        #region DbSet Region - Do Not Delete

        public DbSet<Patient> Patients { get; set; }
        #endregion DbSet Region - Do Not Delete



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>().Property(p => p.PatientId).ValueGeneratedNever();
        }
    }
}