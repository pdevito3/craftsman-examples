
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
    public class UpdateRecipeIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public UpdateRecipeIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task PatchRecipe204AndFieldsWereSuccessfullyUpdated_WithAuth()
        {
            //Arrange
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RecipeProfile>();
            }).CreateMapper();

            var lookupVal = "Easily Identified Value For Test"; // don't know the id at this scope, so need to have another value to lookup
            var fakeRecipeOne = new FakeRecipe { }.Generate();
            
            var expectedFinalObject = mapper.Map<RecipeDto>(fakeRecipeOne);
            expectedFinalObject.Title = lookupVal;

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

            client.AddAuth(new[] {"recipes.read", "recipes.update"});

            var patchDoc = new JsonPatchDocument<RecipeForUpdateDto>();
            patchDoc.Replace(r => r.Title, lookupVal);
            var serializedRecipeToUpdate = JsonConvert.SerializeObject(patchDoc);

            // Act
            // get the value i want to update. assumes I can use sieve for this field. if this is not an option, just use something else
            var getResult = await client.GetAsync($"api/Recipes/?filters=Title=={fakeRecipeOne.Title}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var getResponse = JsonConvert.DeserializeObject<Response<IEnumerable<RecipeDto>>>(getResponseContent);
            var id = getResponse.Data.FirstOrDefault().RecipeId;

            // patch it
            var method = new HttpMethod("PATCH");
            var patchRequest = new HttpRequestMessage(method, $"api/Recipes/{id}")
            {
                Content = new StringContent(serializedRecipeToUpdate,
                    Encoding.Unicode, "application/json")
            };
            var patchResult = await client.SendAsync(patchRequest)
                .ConfigureAwait(false);

            // get it again to confirm updates
            var checkResult = await client.GetAsync($"api/Recipes/{id}")
                .ConfigureAwait(false);
            var checkResponseContent = await checkResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var checkResponse = JsonConvert.DeserializeObject<Response<RecipeDto>>(checkResponseContent);

            // Assert
            patchResult.StatusCode.Should().Be(204);
            checkResponse.Should().BeEquivalentTo(expectedFinalObject, options =>
                options.ExcludingMissingMembers());
        }
        
        [Fact]
        public async Task PutRecipeReturnsBodyAndFieldsWereSuccessfullyUpdated_WithAuth()
        {
            //Arrange
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RecipeProfile>();
            }).CreateMapper();

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            var expectedFinalObject = mapper.Map<RecipeDto>(fakeRecipeOne);
            expectedFinalObject.Title = "Easily Identified Value For Test";

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

            client.AddAuth(new[] {"recipes.read", "recipes.update"});

            var serializedRecipeToUpdate = JsonConvert.SerializeObject(expectedFinalObject);

            // Act
            // get the value i want to update. assumes I can use sieve for this field. if this is not an option, just use something else
            var getResult = await client.GetAsync($"api/Recipes/?filters=Title=={fakeRecipeOne.Title}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var getResponse = JsonConvert.DeserializeObject<Response<IEnumerable<RecipeDto>>>(getResponseContent);
            var id = getResponse?.Data.FirstOrDefault().RecipeId;

            // put it
            var putResult = await client.PutAsJsonAsync($"api/Recipes/{id}", expectedFinalObject)
                .ConfigureAwait(false);

            // get it again to confirm updates
            var checkResult = await client.GetAsync($"api/Recipes/{id}")
                .ConfigureAwait(false);
            var checkResponseContent = await checkResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var checkResponse = JsonConvert.DeserializeObject<Response<RecipeDto>>(checkResponseContent);

            // Assert
            putResult.StatusCode.Should().Be(204);
            checkResponse.Should().BeEquivalentTo(expectedFinalObject, options =>
                options.ExcludingMissingMembers());
        }
            
        [Fact]
        public async Task UpdateRecordRecipes_Returns_Unauthorized_Without_Valid_Token()
        {
            //Arrange
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RecipeProfile>();
            }).CreateMapper();

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            var expectedFinalObject = mapper.Map<RecipeDto>(fakeRecipeOne);
            var id = fakeRecipeOne.RecipeId;

            var patchDoc = new JsonPatchDocument<RecipeForUpdateDto>();
            patchDoc.Replace(r => r.Title, "");
            var serializedRecipeToUpdate = JsonConvert.SerializeObject(patchDoc);

            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            // Act
            var putResult = await client.PutAsJsonAsync($"api/Recipes/{id}", expectedFinalObject)
                .ConfigureAwait(false);

            // Assert
            putResult.StatusCode.Should().Be(401);
        }
            
        [Fact]
        public async Task UpdateRecordRecipe_Returns_Forbidden_Without_Proper_Scope()
        {
            //Arrange
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RecipeProfile>();
            }).CreateMapper();

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            var expectedFinalObject = mapper.Map<RecipeDto>(fakeRecipeOne);
            var id = fakeRecipeOne.RecipeId;

            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            client.AddAuth(new[] { "" });

            // Act
            var putResult = await client.PutAsJsonAsync($"api/Recipes/{id}", expectedFinalObject)
                .ConfigureAwait(false);

            // Assert
            putResult.StatusCode.Should().Be(403);
        }
            
        [Fact]
        public async Task UpdatePartialRecipes_Returns_Unauthorized_Without_Valid_Token()
        {
            //Arrange
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RecipeProfile>();
            }).CreateMapper();

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            var expectedFinalObject = mapper.Map<RecipeDto>(fakeRecipeOne);
            var id = fakeRecipeOne.RecipeId;

            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var patchDoc = new JsonPatchDocument<RecipeForUpdateDto>();
            patchDoc.Replace(r => r.Title, "");
            var serializedRecipeToUpdate = JsonConvert.SerializeObject(patchDoc);

            // Act
            var method = new HttpMethod("PATCH");
            var patchRequest = new HttpRequestMessage(method, $"api/Recipes/{id}")
            {
                Content = new StringContent(serializedRecipeToUpdate,
                    Encoding.Unicode, "application/json")
            };
            var patchResult = await client.SendAsync(patchRequest)
                .ConfigureAwait(false);

            // Assert
            patchResult.StatusCode.Should().Be(401);
        }
            
        [Fact]
        public async Task UpdatePartialRecipe_Returns_Forbidden_Without_Proper_Scope()
        {
            //Arrange
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<RecipeProfile>();
            }).CreateMapper();

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            var expectedFinalObject = mapper.Map<RecipeDto>(fakeRecipeOne);
            var id = fakeRecipeOne.RecipeId;

            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            client.AddAuth(new[] { "" });

            // Act
            var patchResult = await client.PutAsJsonAsync($"api/Recipes/{id}", expectedFinalObject)
                .ConfigureAwait(false);

            // Assert
            patchResult.StatusCode.Should().Be(403);
        }
    } 
}