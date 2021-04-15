
namespace Recipes.Tests.IntegrationTests.Recipe
{
    using Application.Dtos.Recipe;
    using FluentAssertions;
    using Recipes.Tests.Fakes.Recipe;
    using Microsoft.AspNetCore.Mvc.Testing;
    using System.Threading.Tasks;
    using Xunit;
    using Newtonsoft.Json;
    using System.Net.Http;
    using WebApi;
    using System.Collections.Generic;
    using Infrastructure.Persistence.Contexts;
    using Microsoft.Extensions.DependencyInjection;
    using Application.Wrappers;
    using Recipes.Tests.Helpers;

    [Collection("Sequential")]
    public class GetRecipeIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public GetRecipeIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task GetRecipes_ReturnsSuccessCodeAndResourceWithAccurateFields_WithAuth()
        {
            var fakeRecipeOne = new FakeRecipe { }.Generate();
            var fakeRecipeTwo = new FakeRecipe { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<RecipeDbContext>();
                context.Database.EnsureCreated();

                //context.Recipes.RemoveRange(context.Recipes);
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            client.AddAuth(new[] {"recipes.read"});

            var result = await client.GetAsync("api/Recipes")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response<IEnumerable<RecipeDto>>>(responseContent)?.Data;

            // Assert
            result.StatusCode.Should().Be(200);
            response.Should().ContainEquivalentOf(fakeRecipeOne, options =>
                options.ExcludingMissingMembers());
            response.Should().ContainEquivalentOf(fakeRecipeTwo, options =>
                options.ExcludingMissingMembers());
        }
        
        [Fact]
        public async Task GetRecipe_ReturnsSuccessCodeAndResourceWithAccurateFields_WithAuth()
        {
            var fakeRecipeOne = new FakeRecipe { }.Generate();
            var fakeRecipeTwo = new FakeRecipe { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<RecipeDbContext>();
                context.Database.EnsureCreated();

                //context.Recipes.RemoveRange(context.Recipes);
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            client.AddAuth(new[] {"recipes.read"});

            var result = await client.GetAsync($"api/Recipes/{fakeRecipeOne.RecipeId}")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response<RecipeDto>>(responseContent)?.Data;

            // Assert
            result.StatusCode.Should().Be(200);
            response.Should().BeEquivalentTo(fakeRecipeOne, options =>
                options.ExcludingMissingMembers());
        }
            
        [Fact]
        public async Task GetRecipes_Returns_Unauthorized_Without_Valid_Token()
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync("api/Recipes")
                .ConfigureAwait(false);

            // Assert
            result.StatusCode.Should().Be(401);
        }
            
        [Fact]
        public async Task GetRecipes_Returns_Forbidden_Without_Proper_Scope()
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            client.AddAuth(new[] { "" });

            var result = await client.GetAsync("api/Recipes")
                .ConfigureAwait(false);

            // Assert
            result.StatusCode.Should().Be(403);
        }
            
        [Fact]
        public async Task GetRecipe_Returns_Unauthorized_Without_Valid_Token()
        {
            var fakeRecipeOne = new FakeRecipe { }.Generate();
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync($"api/Recipes/{fakeRecipeOne.RecipeId}")
                .ConfigureAwait(false);

            // Assert
            result.StatusCode.Should().Be(401);
        }
            
        [Fact]
        public async Task GetRecipe_Returns_Forbidden_Without_Proper_Scope()
        {
            var fakeRecipeOne = new FakeRecipe { }.Generate();
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            client.AddAuth(new[] { "" });

            var result = await client.GetAsync($"api/Recipes/{fakeRecipeOne.RecipeId}")
                .ConfigureAwait(false);

            // Assert
            result.StatusCode.Should().Be(403);
        }
    } 
}