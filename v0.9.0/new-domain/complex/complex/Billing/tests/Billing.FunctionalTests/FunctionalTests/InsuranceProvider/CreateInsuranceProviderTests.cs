namespace Billing.FunctionalTests.FunctionalTests.InsuranceProvider
{
    using Billing.SharedTestHelpers.Fakes.InsuranceProvider;
    using Billing.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class CreateInsuranceProviderTests : TestBase
    {
        [Test]
        public async Task Create_InsuranceProvider_Returns_Created()
        {
            // Arrange
            var fakeInsuranceProvider = new FakeInsuranceProvider { }.Generate();

            await InsertAsync(fakeInsuranceProvider);

            // Act
            var route = ApiRoutes.InsuranceProviders.Create;
            var result = await _client.PostJsonRequestAsync(route, fakeInsuranceProvider);

            // Assert
            result.StatusCode.Should().Be(201);
        }
    }
}