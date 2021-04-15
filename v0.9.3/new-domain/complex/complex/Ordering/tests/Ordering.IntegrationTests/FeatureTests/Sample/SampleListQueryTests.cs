namespace Ordering.IntegrationTests.FeatureTests.Sample
{
    using Ordering.Core.Dtos.Sample;
    using Ordering.SharedTestHelpers.Fakes.Sample;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using static Ordering.WebApi.Features.Samples.GetSampleList;
    using static TestFixture;

    public class SampleListQueryTests : TestBase
    { 
        
        [Test]
        public async Task SampleListQuery_Returns_Resource_With_Accurate_Props()
        {
            // Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            var queryParameters = new SampleParametersDto();

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            // Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);

            // Assert
            samples.Should().HaveCount(2);
        }
        
        [Test]
        public async Task SampleListQuery_Returns_Expected_Page_Size_And_Number()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            var fakeSampleThree = new FakeSample { }.Generate();
            var queryParameters = new SampleParametersDto() { PageSize = 1, PageNumber = 2 };

            await InsertAsync(fakeSampleOne, fakeSampleTwo, fakeSampleThree);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples.Should().HaveCount(1);
        }
        
        [Test]
        public async Task SampleListQuery_Returns_Sorted_Sample_ExternalId_List_In_Asc_Order()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.ExternalId = "bravo";
            fakeSampleTwo.ExternalId = "alpha";
            var queryParameters = new SampleParametersDto() { SortOrder = "ExternalId" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
            samples
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Returns_Sorted_Sample_ExternalId_List_In_Desc_Order()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.ExternalId = "bravo";
            fakeSampleTwo.ExternalId = "alpha";
            var queryParameters = new SampleParametersDto() { SortOrder = "ExternalId" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
            samples
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Returns_Sorted_Sample_InternalId_List_In_Asc_Order()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.InternalId = "bravo";
            fakeSampleTwo.InternalId = "alpha";
            var queryParameters = new SampleParametersDto() { SortOrder = "InternalId" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
            samples
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Returns_Sorted_Sample_InternalId_List_In_Desc_Order()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.InternalId = "bravo";
            fakeSampleTwo.InternalId = "alpha";
            var queryParameters = new SampleParametersDto() { SortOrder = "InternalId" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
            samples
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Returns_Sorted_Sample_SampleType_List_In_Asc_Order()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.SampleType = "bravo";
            fakeSampleTwo.SampleType = "alpha";
            var queryParameters = new SampleParametersDto() { SortOrder = "SampleType" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
            samples
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Returns_Sorted_Sample_SampleType_List_In_Desc_Order()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.SampleType = "bravo";
            fakeSampleTwo.SampleType = "alpha";
            var queryParameters = new SampleParametersDto() { SortOrder = "SampleType" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
            samples
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Returns_Sorted_Sample_ContainerType_List_In_Asc_Order()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.ContainerType = "bravo";
            fakeSampleTwo.ContainerType = "alpha";
            var queryParameters = new SampleParametersDto() { SortOrder = "ContainerType" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
            samples
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Returns_Sorted_Sample_ContainerType_List_In_Desc_Order()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.ContainerType = "bravo";
            fakeSampleTwo.ContainerType = "alpha";
            var queryParameters = new SampleParametersDto() { SortOrder = "ContainerType" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
            samples
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Returns_Sorted_Sample_CollectionDate_List_In_Asc_Order()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.CollectionDate = DateTime.Now.AddDays(2);
            fakeSampleTwo.CollectionDate = DateTime.Now.AddDays(1);
            var queryParameters = new SampleParametersDto() { SortOrder = "CollectionDate" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
            samples
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Returns_Sorted_Sample_CollectionDate_List_In_Desc_Order()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.CollectionDate = DateTime.Now.AddDays(2);
            fakeSampleTwo.CollectionDate = DateTime.Now.AddDays(1);
            var queryParameters = new SampleParametersDto() { SortOrder = "CollectionDate" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
            samples
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Returns_Sorted_Sample_ArrivalDate_List_In_Asc_Order()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.ArrivalDate = DateTime.Now.AddDays(2);
            fakeSampleTwo.ArrivalDate = DateTime.Now.AddDays(1);
            var queryParameters = new SampleParametersDto() { SortOrder = "ArrivalDate" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
            samples
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Returns_Sorted_Sample_ArrivalDate_List_In_Desc_Order()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.ArrivalDate = DateTime.Now.AddDays(2);
            fakeSampleTwo.ArrivalDate = DateTime.Now.AddDays(1);
            var queryParameters = new SampleParametersDto() { SortOrder = "ArrivalDate" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
            samples
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Returns_Sorted_Sample_Amount_List_In_Asc_Order()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.Amount = 2;
            fakeSampleTwo.Amount = 1;
            var queryParameters = new SampleParametersDto() { SortOrder = "Amount" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
            samples
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Returns_Sorted_Sample_Amount_List_In_Desc_Order()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.Amount = 2;
            fakeSampleTwo.Amount = 1;
            var queryParameters = new SampleParametersDto() { SortOrder = "Amount" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
            samples
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Returns_Sorted_Sample_AmountUnits_List_In_Asc_Order()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.AmountUnits = "bravo";
            fakeSampleTwo.AmountUnits = "alpha";
            var queryParameters = new SampleParametersDto() { SortOrder = "AmountUnits" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
            samples
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Returns_Sorted_Sample_AmountUnits_List_In_Desc_Order()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.AmountUnits = "bravo";
            fakeSampleTwo.AmountUnits = "alpha";
            var queryParameters = new SampleParametersDto() { SortOrder = "AmountUnits" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
            samples
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleOne, options =>
                    options.ExcludingMissingMembers());
        }

        
        [Test]
        public async Task SampleListQuery_Filters_Sample_SampleId()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.SampleId = Guid.NewGuid();
            fakeSampleTwo.SampleId = Guid.NewGuid();
            var queryParameters = new SampleParametersDto() { Filters = $"SampleId == {fakeSampleTwo.SampleId}" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples.Should().HaveCount(1);
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Filters_Sample_ExternalId()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.ExternalId = "alpha";
            fakeSampleTwo.ExternalId = "bravo";
            var queryParameters = new SampleParametersDto() { Filters = $"ExternalId == {fakeSampleTwo.ExternalId}" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples.Should().HaveCount(1);
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Filters_Sample_InternalId()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.InternalId = "alpha";
            fakeSampleTwo.InternalId = "bravo";
            var queryParameters = new SampleParametersDto() { Filters = $"InternalId == {fakeSampleTwo.InternalId}" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples.Should().HaveCount(1);
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Filters_Sample_SampleType()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.SampleType = "alpha";
            fakeSampleTwo.SampleType = "bravo";
            var queryParameters = new SampleParametersDto() { Filters = $"SampleType == {fakeSampleTwo.SampleType}" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples.Should().HaveCount(1);
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Filters_Sample_ContainerType()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.ContainerType = "alpha";
            fakeSampleTwo.ContainerType = "bravo";
            var queryParameters = new SampleParametersDto() { Filters = $"ContainerType == {fakeSampleTwo.ContainerType}" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples.Should().HaveCount(1);
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Filters_Sample_CollectionDate()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.CollectionDate = DateTime.Now.AddDays(1);
            fakeSampleTwo.CollectionDate = DateTime.Parse(DateTime.Now.AddDays(2).ToString("MM/dd/yyyy"));
            var queryParameters = new SampleParametersDto() { Filters = $"CollectionDate == {fakeSampleTwo.CollectionDate}" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples.Should().HaveCount(1);
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Filters_Sample_ArrivalDate()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.ArrivalDate = DateTime.Now.AddDays(1);
            fakeSampleTwo.ArrivalDate = DateTime.Parse(DateTime.Now.AddDays(2).ToString("MM/dd/yyyy"));
            var queryParameters = new SampleParametersDto() { Filters = $"ArrivalDate == {fakeSampleTwo.ArrivalDate}" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples.Should().HaveCount(1);
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Filters_Sample_Amount()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.Amount = 1;
            fakeSampleTwo.Amount = 2;
            var queryParameters = new SampleParametersDto() { Filters = $"Amount == {fakeSampleTwo.Amount}" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples.Should().HaveCount(1);
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleListQuery_Filters_Sample_AmountUnits()
        {
            //Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var fakeSampleTwo = new FakeSample { }.Generate();
            fakeSampleOne.AmountUnits = "alpha";
            fakeSampleTwo.AmountUnits = "bravo";
            var queryParameters = new SampleParametersDto() { Filters = $"AmountUnits == {fakeSampleTwo.AmountUnits}" };

            await InsertAsync(fakeSampleOne, fakeSampleTwo);

            //Act
            var query = new SampleListQuery(queryParameters);
            var samples = await SendAsync(query);
            
            // Assert
            samples.Should().HaveCount(1);
            samples
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeSampleTwo, options =>
                    options.ExcludingMissingMembers());
        }

    } 
}