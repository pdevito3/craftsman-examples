namespace Billing.IntegrationTests.FeatureTests.InsuranceProvider
{
    using Billing.SharedTestHelpers.Fakes.InsuranceProvider;
    using Billing.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using static Billing.WebApi.Features.InsuranceProviders.GetInsuranceProvider;
    using static TestFixture;

    public class InsuranceProviderQueryTests : TestBase
    {
        [Test]
        public async Task InsuranceProviderQuery_Returns_Resource_With_Accurate_Props()
        {
            // Arrange
            var fakeInsuranceProviderOne = new FakeInsuranceProvider { }.Generate();
            await InsertAsync(fakeInsuranceProviderOne);

            // Act
            var query = new InsuranceProviderQuery(fakeInsuranceProviderOne.InsuranceProviderId);
            var insuranceProviders = await SendAsync(query);

            // Assert
            insuranceProviders.Should().BeEquivalentTo(fakeInsuranceProviderOne, options =>
                options.ExcludingMissingMembers());
        }
    }
}