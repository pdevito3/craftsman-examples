namespace Infrastructure.Persistence.Seeders
{

    using AutoBogus;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using System.Linq;

    public static class RecipeSeeder
    {
        public static void SeedSampleRecipeData(RecipeDbContext context)
        {
            if (!context.Recipes.Any())
            {
                context.Recipes.Add(new AutoFaker<Recipe>());
                context.Recipes.Add(new AutoFaker<Recipe>());
                context.Recipes.Add(new AutoFaker<Recipe>());

                context.SaveChanges();
            }
        }
    }
}