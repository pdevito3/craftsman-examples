namespace Billing.FunctionalTests.FunctionalTests.InsuranceProvider
{
    using Billing.SharedTestHelpers.Fakes.InsuranceProvider;
    using Billing.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class UpdateInsuranceProviderRecordTests : TestBase
    {
        [Test]
        public async Task Put_InsuranceProvider_Returns_NoContent()
        {
            // Arrange
            var fakeInsuranceProvider = new FakeInsuranceProvider { }.Generate();
            var updatedInsuranceProviderDto = new FakeInsuranceProviderForUpdateDto { }.Generate();

            await InsertAsync(fakeInsuranceProvider);

            // Act
            var route = ApiRoutes.InsuranceProviders.Put.Replace(ApiRoutes.InsuranceProviders.InsuranceProviderId, fakeInsuranceProvider.InsuranceProviderId.ToString());
            var result = await _client.PutJsonRequestAsync(route, updatedInsuranceProviderDto);

            // Assert
            result.StatusCode.Should().Be(204);
        }
    }
}