namespace Billing.IntegrationTests.FeatureTests.InsuranceProvider
{
    using Billing.Core.Dtos.InsuranceProvider;
    using Billing.SharedTestHelpers.Fakes.InsuranceProvider;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using static Billing.WebApi.Features.InsuranceProviders.GetInsuranceProviderList;
    using static TestFixture;

    public class InsuranceProviderListQueryTests : TestBase
    { 
        
        [Test]
        public async Task InsuranceProviderListQuery_Returns_Resource_With_Accurate_Props()
        {
            // Arrange
            var fakeInsuranceProviderOne = new FakeInsuranceProvider { }.Generate();
            var fakeInsuranceProviderTwo = new FakeInsuranceProvider { }.Generate();
            var queryParameters = new InsuranceProviderParametersDto();

            await InsertAsync(fakeInsuranceProviderOne, fakeInsuranceProviderTwo);

            // Act
            var query = new InsuranceProviderListQuery(queryParameters);
            var insuranceProviders = await SendAsync(query);

            // Assert
            insuranceProviders.Should().HaveCount(2);
        }
        
        [Test]
        public async Task InsuranceProviderListQuery_Returns_Expected_Page_Size_And_Number()
        {
            //Arrange
            var fakeInsuranceProviderOne = new FakeInsuranceProvider { }.Generate();
            var fakeInsuranceProviderTwo = new FakeInsuranceProvider { }.Generate();
            var fakeInsuranceProviderThree = new FakeInsuranceProvider { }.Generate();
            var queryParameters = new InsuranceProviderParametersDto() { PageSize = 1, PageNumber = 2 };

            await InsertAsync(fakeInsuranceProviderOne, fakeInsuranceProviderTwo, fakeInsuranceProviderThree);

            //Act
            var query = new InsuranceProviderListQuery(queryParameters);
            var insuranceProviders = await SendAsync(query);
            
            // Assert
            insuranceProviders.Should().HaveCount(1);
        }
        
        [Test]
        public async Task InsuranceProviderListQuery_Returns_Sorted_InsuranceProvider_ProviderName_List_In_Asc_Order()
        {
            //Arrange
            var fakeInsuranceProviderOne = new FakeInsuranceProvider { }.Generate();
            var fakeInsuranceProviderTwo = new FakeInsuranceProvider { }.Generate();
            fakeInsuranceProviderOne.ProviderName = "bravo";
            fakeInsuranceProviderTwo.ProviderName = "alpha";
            var queryParameters = new InsuranceProviderParametersDto() { SortOrder = "ProviderName" };

            await InsertAsync(fakeInsuranceProviderOne, fakeInsuranceProviderTwo);

            //Act
            var query = new InsuranceProviderListQuery(queryParameters);
            var insuranceProviders = await SendAsync(query);
            
            // Assert
            insuranceProviders
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeInsuranceProviderTwo, options =>
                    options.ExcludingMissingMembers());
            insuranceProviders
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeInsuranceProviderOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task InsuranceProviderListQuery_Returns_Sorted_InsuranceProvider_ProviderName_List_In_Desc_Order()
        {
            //Arrange
            var fakeInsuranceProviderOne = new FakeInsuranceProvider { }.Generate();
            var fakeInsuranceProviderTwo = new FakeInsuranceProvider { }.Generate();
            fakeInsuranceProviderOne.ProviderName = "bravo";
            fakeInsuranceProviderTwo.ProviderName = "alpha";
            var queryParameters = new InsuranceProviderParametersDto() { SortOrder = "ProviderName" };

            await InsertAsync(fakeInsuranceProviderOne, fakeInsuranceProviderTwo);

            //Act
            var query = new InsuranceProviderListQuery(queryParameters);
            var insuranceProviders = await SendAsync(query);
            
            // Assert
            insuranceProviders
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeInsuranceProviderTwo, options =>
                    options.ExcludingMissingMembers());
            insuranceProviders
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeInsuranceProviderOne, options =>
                    options.ExcludingMissingMembers());
        }

        
        [Test]
        public async Task InsuranceProviderListQuery_Filters_InsuranceProvider_InsuranceProviderId()
        {
            //Arrange
            var fakeInsuranceProviderOne = new FakeInsuranceProvider { }.Generate();
            var fakeInsuranceProviderTwo = new FakeInsuranceProvider { }.Generate();
            fakeInsuranceProviderOne.InsuranceProviderId = Guid.NewGuid();
            fakeInsuranceProviderTwo.InsuranceProviderId = Guid.NewGuid();
            var queryParameters = new InsuranceProviderParametersDto() { Filters = $"InsuranceProviderId == {fakeInsuranceProviderTwo.InsuranceProviderId}" };

            await InsertAsync(fakeInsuranceProviderOne, fakeInsuranceProviderTwo);

            //Act
            var query = new InsuranceProviderListQuery(queryParameters);
            var insuranceProviders = await SendAsync(query);
            
            // Assert
            insuranceProviders.Should().HaveCount(1);
            insuranceProviders
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeInsuranceProviderTwo, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task InsuranceProviderListQuery_Filters_InsuranceProvider_ProviderName()
        {
            //Arrange
            var fakeInsuranceProviderOne = new FakeInsuranceProvider { }.Generate();
            var fakeInsuranceProviderTwo = new FakeInsuranceProvider { }.Generate();
            fakeInsuranceProviderOne.ProviderName = "alpha";
            fakeInsuranceProviderTwo.ProviderName = "bravo";
            var queryParameters = new InsuranceProviderParametersDto() { Filters = $"ProviderName == {fakeInsuranceProviderTwo.ProviderName}" };

            await InsertAsync(fakeInsuranceProviderOne, fakeInsuranceProviderTwo);

            //Act
            var query = new InsuranceProviderListQuery(queryParameters);
            var insuranceProviders = await SendAsync(query);
            
            // Assert
            insuranceProviders.Should().HaveCount(1);
            insuranceProviders
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakeInsuranceProviderTwo, options =>
                    options.ExcludingMissingMembers());
        }

    } 
}