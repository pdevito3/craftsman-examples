namespace Infrastructure.Persistence.Contexts
{
    using Application.Interfaces;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System.Threading;
    using System.Threading.Tasks;

    public class LabDbContext : DbContext
    {
        public LabDbContext(
            DbContextOptions<LabDbContext> options) : base(options) 
        {
        }

        #region DbSet Region - Do Not Delete
        public DbSet<Patient> Patients { get; set; }
        #endregion
    }
}