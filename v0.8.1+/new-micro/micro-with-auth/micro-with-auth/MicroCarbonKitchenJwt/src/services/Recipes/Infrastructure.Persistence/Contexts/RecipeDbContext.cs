namespace Infrastructure.Persistence.Contexts
{
    using Application.Interfaces;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using System.Threading;
    using System.Threading.Tasks;

    public class RecipeDbContext : DbContext
    {
        public RecipeDbContext(
            DbContextOptions<RecipeDbContext> options) : base(options) 
        {
        }

        #region DbSet Region - Do Not Delete
        public DbSet<Recipe> Recipes { get; set; }
        #endregion
    }
}