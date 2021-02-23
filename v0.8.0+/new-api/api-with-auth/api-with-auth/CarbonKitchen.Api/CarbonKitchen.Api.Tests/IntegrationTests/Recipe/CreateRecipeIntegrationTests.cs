
namespace CarbonKitchen.Api.Tests.IntegrationTests.Recipe
{
    using Application.Dtos.Recipe;
    using FluentAssertions;
    using CarbonKitchen.Api.Tests.Fakes.Recipe;
    using Microsoft.AspNetCore.Mvc.Testing;
    using System.Threading.Tasks;
    using Xunit;
    using Newtonsoft.Json;
    using System.Net.Http;
    using WebApi;
    using System.Collections.Generic;
    using Application.Wrappers;
    using CarbonKitchen.Api.Tests.Helpers;

    [Collection("Sequential")]
    public class CreateRecipeIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public CreateRecipeIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task PostRecipeReturnsSuccessCodeAndResourceWithAccurateFields_WithAuth()
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var fakeRecipe = new FakeRecipeDto().Generate();

            client.AddAuth(new[] {"recipes.add"});

            // Act
            var httpResponse = await client.PostAsJsonAsync("api/Recipes", fakeRecipe)
                .ConfigureAwait(false);

            // Assert
            httpResponse.EnsureSuccessStatusCode();

            var resultDto = JsonConvert.DeserializeObject<Response<RecipeDto>>(await httpResponse.Content.ReadAsStringAsync()
                .ConfigureAwait(false));

            httpResponse.StatusCode.Should().Be(201);
            resultDto.Data.Title.Should().Be(fakeRecipe.Title);
            resultDto.Data.Directions.Should().Be(fakeRecipe.Directions);
            resultDto.Data.RecipeSourceLink.Should().Be(fakeRecipe.RecipeSourceLink);
            resultDto.Data.Description.Should().Be(fakeRecipe.Description);
            resultDto.Data.ImageLink.Should().Be(fakeRecipe.ImageLink);
        }
            
        [Fact]
        public async Task PostRecipes_Returns_Unauthorized_Without_Valid_Token()
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var fakeRecipe = new FakeRecipeDto().Generate();

            // Act
            var httpResponse = await client.PostAsJsonAsync("api/Recipes", fakeRecipe)
                .ConfigureAwait(false);

            // Assert
            httpResponse.StatusCode.Should().Be(401);
        }
            
        [Fact]
        public async Task PostRecipe_Returns_Forbidden_Without_Proper_Scope()
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var fakeRecipe = new FakeRecipeDto().Generate();

            client.AddAuth(new[] { "" });

            // Act
            var httpResponse = await client.PostAsJsonAsync("api/Recipes", fakeRecipe)
                .ConfigureAwait(false);

            // Assert
            httpResponse.StatusCode.Should().Be(403);
        }
    } 
}