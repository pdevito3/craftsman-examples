namespace Application.Interfaces.Recipe
{
    using System;
    using Application.Dtos.Recipe;
    using Application.Wrappers;
    using System.Threading.Tasks;
    using Domain.Entities;

    public interface IRecipeRepository
    {
        Task<PagedList<Recipe>> GetRecipesAsync(RecipeParametersDto RecipeParameters);
        Task<Recipe> GetRecipeAsync(int RecipeId);
        Recipe GetRecipe(int RecipeId);
        Task AddRecipe(Recipe recipe);
        void DeleteRecipe(Recipe recipe);
        void UpdateRecipe(Recipe recipe);
        bool Save();
        Task<bool> SaveAsync();
    }
}