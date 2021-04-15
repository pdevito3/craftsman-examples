namespace Billing.FunctionalTests.FunctionalTests.InsuranceProvider
{
    using Billing.SharedTestHelpers.Fakes.InsuranceProvider;
    using Billing.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class GetInsuranceProviderTests : TestBase
    {
        [Test]
        public async Task Get_InsuranceProvider_Record_Returns_NoContent()
        {
            // Arrange
            var fakeInsuranceProvider = new FakeInsuranceProvider { }.Generate();

            await InsertAsync(fakeInsuranceProvider);

            // Act
            var route = ApiRoutes.InsuranceProviders.GetRecord.Replace(ApiRoutes.InsuranceProviders.InsuranceProviderId, fakeInsuranceProvider.InsuranceProviderId.ToString());
            var result = await _client.GetRequestAsync(route);

            // Assert
            result.StatusCode.Should().Be(200);
        }
    }
}