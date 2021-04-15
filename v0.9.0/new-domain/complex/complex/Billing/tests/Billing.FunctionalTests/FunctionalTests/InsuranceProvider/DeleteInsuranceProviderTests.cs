namespace Billing.FunctionalTests.FunctionalTests.InsuranceProvider
{
    using Billing.SharedTestHelpers.Fakes.InsuranceProvider;
    using Billing.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class DeleteInsuranceProviderTests : TestBase
    {
        [Test]
        public async Task Delete_InsuranceProviderReturns_NoContent()
        {
            // Arrange
            var fakeInsuranceProvider = new FakeInsuranceProvider { }.Generate();

            await InsertAsync(fakeInsuranceProvider);

            // Act
            var route = ApiRoutes.InsuranceProviders.Delete.Replace(ApiRoutes.InsuranceProviders.InsuranceProviderId, fakeInsuranceProvider.InsuranceProviderId.ToString());
            var result = await _client.DeleteRequestAsync(route);

            // Assert
            result.StatusCode.Should().Be(204);
        }
    }
}