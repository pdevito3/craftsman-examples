namespace Infrastructure.Persistence.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Dtos.Recipe;
    using Application.Interfaces.Recipe;
    using Application.Wrappers;
    using Domain.Entities;
    using Infrastructure.Persistence.Contexts;
    using Microsoft.EntityFrameworkCore;
    using Sieve.Models;
    using Sieve.Services;

    public class RecipeRepository : IRecipeRepository
    {
        private RecipeDbContext _context;
        private readonly SieveProcessor _sieveProcessor;

        public RecipeRepository(RecipeDbContext context,
            SieveProcessor sieveProcessor)
        {
            _context = context
                ?? throw new ArgumentNullException(nameof(context));
            _sieveProcessor = sieveProcessor ??
                throw new ArgumentNullException(nameof(sieveProcessor));
        }

        public async Task<PagedList<Recipe>> GetRecipesAsync(RecipeParametersDto recipeParameters)
        {
            if (recipeParameters == null)
            {
                throw new ArgumentNullException(nameof(recipeParameters));
            }

            var collection = _context.Recipes
                as IQueryable<Recipe>; // TODO: AsNoTracking() should increase performance, but will break the sort tests. need to investigate

            var sieveModel = new SieveModel
            {
                Sorts = recipeParameters.SortOrder ?? "RecipeId",
                Filters = recipeParameters.Filters
            };

            collection = _sieveProcessor.Apply(sieveModel, collection);

            return await PagedList<Recipe>.CreateAsync(collection,
                recipeParameters.PageNumber,
                recipeParameters.PageSize);
        }

        public async Task<Recipe> GetRecipeAsync(int recipeId)
        {
            // include marker -- requires return _context.Recipes as it's own line with no extra text -- do not delete this comment
            return await _context.Recipes
                .FirstOrDefaultAsync(r => r.RecipeId == recipeId);
        }

        public Recipe GetRecipe(int recipeId)
        {
            // include marker -- requires return _context.Recipes as it's own line with no extra text -- do not delete this comment
            return _context.Recipes
                .FirstOrDefault(r => r.RecipeId == recipeId);
        }

        public async Task AddRecipe(Recipe recipe)
        {
            if (recipe == null)
            {
                throw new ArgumentNullException(nameof(Recipe));
            }

            await _context.Recipes.AddAsync(recipe);
        }

        public void DeleteRecipe(Recipe recipe)
        {
            if (recipe == null)
            {
                throw new ArgumentNullException(nameof(Recipe));
            }

            _context.Recipes.Remove(recipe);
        }

        public void UpdateRecipe(Recipe recipe)
        {
            // no implementation for now
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}