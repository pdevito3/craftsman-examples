
namespace Recipes.Tests.RepositoryTests.Recipe
{
    using Application.Dtos.Recipe;
    using FluentAssertions;
    using Recipes.Tests.Fakes.Recipe;
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

    public class GetRecipeRepositoryTests
    { 
        
        [Fact]
        public void GetRecipe_ParametersMatchExpectedValues()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipe = new FakeRecipe { }.Generate();

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipe);
                context.SaveChanges();

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                //Assert
                var recipeById = service.GetRecipe(fakeRecipe.RecipeId);
                                recipeById.RecipeId.Should().Be(fakeRecipe.RecipeId);
                recipeById.Title.Should().Be(fakeRecipe.Title);
                recipeById.Directions.Should().Be(fakeRecipe.Directions);
                recipeById.RecipeSourceLink.Should().Be(fakeRecipe.RecipeSourceLink);
                recipeById.Description.Should().Be(fakeRecipe.Description);
                recipeById.ImageLink.Should().Be(fakeRecipe.ImageLink);
            }
        }
        
        [Fact]
        public async void GetRecipesAsync_CountMatchesAndContainsEquivalentObjects()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            var fakeRecipeTwo = new FakeRecipe { }.Generate();
            var fakeRecipeThree = new FakeRecipe { }.Generate();

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);
                context.SaveChanges();

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                var recipeRepo = await service.GetRecipesAsync(new RecipeParametersDto());

                //Assert
                recipeRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(3);

                recipeRepo.Should().ContainEquivalentOf(fakeRecipeOne);
                recipeRepo.Should().ContainEquivalentOf(fakeRecipeTwo);
                recipeRepo.Should().ContainEquivalentOf(fakeRecipeThree);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetRecipesAsync_ReturnExpectedPageSize()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            var fakeRecipeTwo = new FakeRecipe { }.Generate();
            var fakeRecipeThree = new FakeRecipe { }.Generate();
            
            // need id's due to default sorting
            fakeRecipeOne.RecipeId = 1;
            fakeRecipeTwo.RecipeId = 2;
            fakeRecipeThree.RecipeId = 3;

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);
                context.SaveChanges();

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                var recipeRepo = await service.GetRecipesAsync(new RecipeParametersDto { PageSize = 2 });

                //Assert
                recipeRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                recipeRepo.Should().ContainEquivalentOf(fakeRecipeOne);
                recipeRepo.Should().ContainEquivalentOf(fakeRecipeTwo);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetRecipesAsync_ReturnExpectedPageNumberAndSize()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            var fakeRecipeTwo = new FakeRecipe { }.Generate();
            var fakeRecipeThree = new FakeRecipe { }.Generate();
            
            // need id's due to default sorting
            fakeRecipeOne.RecipeId = 1;
            fakeRecipeTwo.RecipeId = 2;
            fakeRecipeThree.RecipeId = 3;

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);
                context.SaveChanges();

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                var recipeRepo = await service.GetRecipesAsync(new RecipeParametersDto { PageSize = 1, PageNumber = 2 });

                //Assert
                recipeRepo.Should()
                    .NotBeEmpty()
                    .And.HaveCount(1);

                recipeRepo.Should().ContainEquivalentOf(fakeRecipeTwo);

                context.Database.EnsureDeleted();
            }
        }
        
        [Fact]
        public async void GetRecipesAsync_ListRecipeIdSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            fakeRecipeOne.RecipeId = 2;

            var fakeRecipeTwo = new FakeRecipe { }.Generate();
            fakeRecipeTwo.RecipeId = 1;

            var fakeRecipeThree = new FakeRecipe { }.Generate();
            fakeRecipeThree.RecipeId = 3;

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);
                context.SaveChanges();

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                var recipeRepo = await service.GetRecipesAsync(new RecipeParametersDto { SortOrder = "RecipeId" });

                //Assert
                recipeRepo.Should()
                    .ContainInOrder(fakeRecipeTwo, fakeRecipeOne, fakeRecipeThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetRecipesAsync_ListRecipeIdSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            fakeRecipeOne.RecipeId = 2;

            var fakeRecipeTwo = new FakeRecipe { }.Generate();
            fakeRecipeTwo.RecipeId = 1;

            var fakeRecipeThree = new FakeRecipe { }.Generate();
            fakeRecipeThree.RecipeId = 3;

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);
                context.SaveChanges();

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                var recipeRepo = await service.GetRecipesAsync(new RecipeParametersDto { SortOrder = "-RecipeId" });

                //Assert
                recipeRepo.Should()
                    .ContainInOrder(fakeRecipeThree, fakeRecipeOne, fakeRecipeTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetRecipesAsync_ListTitleSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            fakeRecipeOne.Title = "bravo";

            var fakeRecipeTwo = new FakeRecipe { }.Generate();
            fakeRecipeTwo.Title = "alpha";

            var fakeRecipeThree = new FakeRecipe { }.Generate();
            fakeRecipeThree.Title = "charlie";

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);
                context.SaveChanges();

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                var recipeRepo = await service.GetRecipesAsync(new RecipeParametersDto { SortOrder = "Title" });

                //Assert
                recipeRepo.Should()
                    .ContainInOrder(fakeRecipeTwo, fakeRecipeOne, fakeRecipeThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetRecipesAsync_ListTitleSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            fakeRecipeOne.Title = "bravo";

            var fakeRecipeTwo = new FakeRecipe { }.Generate();
            fakeRecipeTwo.Title = "alpha";

            var fakeRecipeThree = new FakeRecipe { }.Generate();
            fakeRecipeThree.Title = "charlie";

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);
                context.SaveChanges();

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                var recipeRepo = await service.GetRecipesAsync(new RecipeParametersDto { SortOrder = "-Title" });

                //Assert
                recipeRepo.Should()
                    .ContainInOrder(fakeRecipeThree, fakeRecipeOne, fakeRecipeTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetRecipesAsync_ListDirectionsSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            fakeRecipeOne.Directions = "bravo";

            var fakeRecipeTwo = new FakeRecipe { }.Generate();
            fakeRecipeTwo.Directions = "alpha";

            var fakeRecipeThree = new FakeRecipe { }.Generate();
            fakeRecipeThree.Directions = "charlie";

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);
                context.SaveChanges();

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                var recipeRepo = await service.GetRecipesAsync(new RecipeParametersDto { SortOrder = "Directions" });

                //Assert
                recipeRepo.Should()
                    .ContainInOrder(fakeRecipeTwo, fakeRecipeOne, fakeRecipeThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetRecipesAsync_ListDirectionsSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            fakeRecipeOne.Directions = "bravo";

            var fakeRecipeTwo = new FakeRecipe { }.Generate();
            fakeRecipeTwo.Directions = "alpha";

            var fakeRecipeThree = new FakeRecipe { }.Generate();
            fakeRecipeThree.Directions = "charlie";

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);
                context.SaveChanges();

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                var recipeRepo = await service.GetRecipesAsync(new RecipeParametersDto { SortOrder = "-Directions" });

                //Assert
                recipeRepo.Should()
                    .ContainInOrder(fakeRecipeThree, fakeRecipeOne, fakeRecipeTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetRecipesAsync_ListRecipeSourceLinkSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            fakeRecipeOne.RecipeSourceLink = "bravo";

            var fakeRecipeTwo = new FakeRecipe { }.Generate();
            fakeRecipeTwo.RecipeSourceLink = "alpha";

            var fakeRecipeThree = new FakeRecipe { }.Generate();
            fakeRecipeThree.RecipeSourceLink = "charlie";

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);
                context.SaveChanges();

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                var recipeRepo = await service.GetRecipesAsync(new RecipeParametersDto { SortOrder = "RecipeSourceLink" });

                //Assert
                recipeRepo.Should()
                    .ContainInOrder(fakeRecipeTwo, fakeRecipeOne, fakeRecipeThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetRecipesAsync_ListRecipeSourceLinkSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            fakeRecipeOne.RecipeSourceLink = "bravo";

            var fakeRecipeTwo = new FakeRecipe { }.Generate();
            fakeRecipeTwo.RecipeSourceLink = "alpha";

            var fakeRecipeThree = new FakeRecipe { }.Generate();
            fakeRecipeThree.RecipeSourceLink = "charlie";

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);
                context.SaveChanges();

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                var recipeRepo = await service.GetRecipesAsync(new RecipeParametersDto { SortOrder = "-RecipeSourceLink" });

                //Assert
                recipeRepo.Should()
                    .ContainInOrder(fakeRecipeThree, fakeRecipeOne, fakeRecipeTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetRecipesAsync_ListDescriptionSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            fakeRecipeOne.Description = "bravo";

            var fakeRecipeTwo = new FakeRecipe { }.Generate();
            fakeRecipeTwo.Description = "alpha";

            var fakeRecipeThree = new FakeRecipe { }.Generate();
            fakeRecipeThree.Description = "charlie";

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);
                context.SaveChanges();

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                var recipeRepo = await service.GetRecipesAsync(new RecipeParametersDto { SortOrder = "Description" });

                //Assert
                recipeRepo.Should()
                    .ContainInOrder(fakeRecipeTwo, fakeRecipeOne, fakeRecipeThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetRecipesAsync_ListDescriptionSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            fakeRecipeOne.Description = "bravo";

            var fakeRecipeTwo = new FakeRecipe { }.Generate();
            fakeRecipeTwo.Description = "alpha";

            var fakeRecipeThree = new FakeRecipe { }.Generate();
            fakeRecipeThree.Description = "charlie";

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);
                context.SaveChanges();

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                var recipeRepo = await service.GetRecipesAsync(new RecipeParametersDto { SortOrder = "-Description" });

                //Assert
                recipeRepo.Should()
                    .ContainInOrder(fakeRecipeThree, fakeRecipeOne, fakeRecipeTwo);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetRecipesAsync_ListImageLinkSortedInAscOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            fakeRecipeOne.ImageLink = "bravo";

            var fakeRecipeTwo = new FakeRecipe { }.Generate();
            fakeRecipeTwo.ImageLink = "alpha";

            var fakeRecipeThree = new FakeRecipe { }.Generate();
            fakeRecipeThree.ImageLink = "charlie";

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);
                context.SaveChanges();

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                var recipeRepo = await service.GetRecipesAsync(new RecipeParametersDto { SortOrder = "ImageLink" });

                //Assert
                recipeRepo.Should()
                    .ContainInOrder(fakeRecipeTwo, fakeRecipeOne, fakeRecipeThree);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetRecipesAsync_ListImageLinkSortedInDescOrder()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            fakeRecipeOne.ImageLink = "bravo";

            var fakeRecipeTwo = new FakeRecipe { }.Generate();
            fakeRecipeTwo.ImageLink = "alpha";

            var fakeRecipeThree = new FakeRecipe { }.Generate();
            fakeRecipeThree.ImageLink = "charlie";

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);
                context.SaveChanges();

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                var recipeRepo = await service.GetRecipesAsync(new RecipeParametersDto { SortOrder = "-ImageLink" });

                //Assert
                recipeRepo.Should()
                    .ContainInOrder(fakeRecipeThree, fakeRecipeOne, fakeRecipeTwo);

                context.Database.EnsureDeleted();
            }
        }

        
        [Fact]
        public async void GetRecipesAsync_FilterRecipeIdListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            fakeRecipeOne.RecipeId = 1;

            var fakeRecipeTwo = new FakeRecipe { }.Generate();
            fakeRecipeTwo.RecipeId = 2;

            var fakeRecipeThree = new FakeRecipe { }.Generate();
            fakeRecipeThree.RecipeId = 3;

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);
                context.SaveChanges();

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                var recipeRepo = await service.GetRecipesAsync(new RecipeParametersDto { Filters = $"RecipeId == {fakeRecipeTwo.RecipeId}" });

                //Assert
                recipeRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetRecipesAsync_FilterTitleListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            fakeRecipeOne.Title = "alpha";

            var fakeRecipeTwo = new FakeRecipe { }.Generate();
            fakeRecipeTwo.Title = "bravo";

            var fakeRecipeThree = new FakeRecipe { }.Generate();
            fakeRecipeThree.Title = "charlie";

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);
                context.SaveChanges();

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                var recipeRepo = await service.GetRecipesAsync(new RecipeParametersDto { Filters = $"Title == {fakeRecipeTwo.Title}" });

                //Assert
                recipeRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetRecipesAsync_FilterDirectionsListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            fakeRecipeOne.Directions = "alpha";

            var fakeRecipeTwo = new FakeRecipe { }.Generate();
            fakeRecipeTwo.Directions = "bravo";

            var fakeRecipeThree = new FakeRecipe { }.Generate();
            fakeRecipeThree.Directions = "charlie";

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);
                context.SaveChanges();

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                var recipeRepo = await service.GetRecipesAsync(new RecipeParametersDto { Filters = $"Directions == {fakeRecipeTwo.Directions}" });

                //Assert
                recipeRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetRecipesAsync_FilterRecipeSourceLinkListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            fakeRecipeOne.RecipeSourceLink = "alpha";

            var fakeRecipeTwo = new FakeRecipe { }.Generate();
            fakeRecipeTwo.RecipeSourceLink = "bravo";

            var fakeRecipeThree = new FakeRecipe { }.Generate();
            fakeRecipeThree.RecipeSourceLink = "charlie";

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);
                context.SaveChanges();

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                var recipeRepo = await service.GetRecipesAsync(new RecipeParametersDto { Filters = $"RecipeSourceLink == {fakeRecipeTwo.RecipeSourceLink}" });

                //Assert
                recipeRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetRecipesAsync_FilterDescriptionListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            fakeRecipeOne.Description = "alpha";

            var fakeRecipeTwo = new FakeRecipe { }.Generate();
            fakeRecipeTwo.Description = "bravo";

            var fakeRecipeThree = new FakeRecipe { }.Generate();
            fakeRecipeThree.Description = "charlie";

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);
                context.SaveChanges();

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                var recipeRepo = await service.GetRecipesAsync(new RecipeParametersDto { Filters = $"Description == {fakeRecipeTwo.Description}" });

                //Assert
                recipeRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

        [Fact]
        public async void GetRecipesAsync_FilterImageLinkListWithExact()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase(databaseName: $"RecipeDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakeRecipeOne = new FakeRecipe { }.Generate();
            fakeRecipeOne.ImageLink = "alpha";

            var fakeRecipeTwo = new FakeRecipe { }.Generate();
            fakeRecipeTwo.ImageLink = "bravo";

            var fakeRecipeThree = new FakeRecipe { }.Generate();
            fakeRecipeThree.ImageLink = "charlie";

            //Act
            using (var context = new RecipeDbContext(dbOptions))
            {
                context.Recipes.AddRange(fakeRecipeOne, fakeRecipeTwo, fakeRecipeThree);
                context.SaveChanges();

                var service = new RecipeRepository(context, new SieveProcessor(sieveOptions));

                var recipeRepo = await service.GetRecipesAsync(new RecipeParametersDto { Filters = $"ImageLink == {fakeRecipeTwo.ImageLink}" });

                //Assert
                recipeRepo.Should()
                    .HaveCount(1);

                context.Database.EnsureDeleted();
            }
        }

    } 
}