
namespace CarbonKitchen.Api.Tests.RepositoryTests.Recipe
{
    using Application.Dtos.Recipe;
    using FluentAssertions;
    using CarbonKitchen.Api.Tests.Fakes.Recipe;
    using Infrastructure.Persistence.Contexts;
    using Infrastructure.Persistence.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Sieve.Models;
    using Sieve.Services;
    using System;
    using System.Linq;
    using Xunit;
    using Application.Interfaces;
    using Moq;

    public class DeleteRecipeRepositoryTests
    { 
        
        [Fact]
        public void DeleteRecipe_ReturnsProperCount()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<CarbonKitchenDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            var fakeRecipeTwo = new FakeRecipe { }.Generate();
            var fakeRecipeThree = new FakeRecipe { }.Generate();

            //Act
            using (var context = new CarbonKitchenDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));
                service.DeleteRecipe(fakeRecipeTwo);

                context.SaveChanges();

                //Assert
                var recipeList = context.Recipes.ToList();

                recipeList.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                recipeList.Should().ContainEquivalentOf(fakeRecipeOne);
                recipeList.Should().ContainEquivalentOf(fakeRecipeThree);
                Assert.DoesNotContain(recipeList, r => r == fakeRecipeTwo);

                context.Database.EnsureDeleted();
            }
        }
    } 
}