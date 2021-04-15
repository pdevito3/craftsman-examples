
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
    using Microsoft.AspNetCore.JsonPatch;
    using System.Linq;
    using AutoMapper;
    using Bogus;
    using Application.Mappings;
    using System.Text;
    using Application.Wrappers;
    using Recipes.Tests.Helpers;

    [Collection("Sequential")]
    public class DeleteRecipeIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public DeleteRecipeIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task DeleteRecipe204AndFieldsWereSuccessfullyUpdated_WithAuth()
        {
            //Arrange
            var fakeRecipeOne = new FakeRecipe { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<RecipeDbContext> ();
                context.Database.EnsureCreated();

                context.Recipes.RemoveRange(context.Recipes);
                context.Recipes.AddRange(fakeRecipeOne);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            client.AddAuth(new[] {"recipes.read", "recipes.delete"});

            // Act
            // get the value i want to update. assumes I can use sieve for this field. if this is not an option, just use something else
            var getResult = await client.GetAsync($"api/Recipes/?filters=Title=={fakeRecipeOne.Title}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var getResponse = JsonConvert.DeserializeObject<Response<IEnumerable<RecipeDto>>>(getResponseContent);
            var id = getResponse.Data.FirstOrDefault().RecipeId;

            // delete it
            var method = new HttpMethod("DELETE");
            var deleteRequest = new HttpRequestMessage(method, $"api/Recipes/{id}");
            var deleteResult = await client.SendAsync(deleteRequest)
                .ConfigureAwait(false);

            // get it again to confirm updates
            var checkResult = await client.GetAsync($"api/Recipes/{id}")
                .ConfigureAwait(false);
            var checkResponseContent = await checkResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var checkResponse = JsonConvert.DeserializeObject<Response<RecipeDto>>(checkResponseContent);

            // Assert
            deleteResult.StatusCode.Should().Be(204);
            checkResponse.Data.Should().Be(null);
        }
            
        [Fact]
        public async Task DeleteRecordRecipes_Returns_Unauthorized_Without_Valid_Token()
        {
            //Arrange
            var fakeRecipeOne = new FakeRecipe { }.Generate();
            var id = fakeRecipeOne.RecipeId;

            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            // Act
            var method = new HttpMethod("DELETE");
            var deleteRequest = new HttpRequestMessage(method, $"api/Recipes/{id}");
            var deleteResult = await client.SendAsync(deleteRequest)
                .ConfigureAwait(false);

            // Assert
            deleteResult.StatusCode.Should().Be(401);
        }
            
        [Fact]
        public async Task DeleteRecordRecipe_Returns_Forbidden_Without_Proper_Scope()
        {
            //Arrange
            var fakeRecipeOne = new FakeRecipe { }.Generate();
            var id = fakeRecipeOne.RecipeId;

            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            client.AddAuth(new[] { "" });

            // Act
            var method = new HttpMethod("DELETE");
            var deleteRequest = new HttpRequestMessage(method, $"api/Recipes/{id}");
            var deleteResult = await client.SendAsync(deleteRequest)
                .ConfigureAwait(false);

            // Assert
            deleteResult.StatusCode.Should().Be(403);
        }
    } 
}