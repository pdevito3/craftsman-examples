namespace Billing.FunctionalTests.FunctionalTests.InsuranceProvider
{
    using Billing.SharedTestHelpers.Fakes.InsuranceProvider;
    using Billing.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class GetInsuranceProviderListTests : TestBase
    {
        [Test]
        public async Task Get_InsuranceProvider_List_Returns_NoContent()
        {
            // Arrange
            // N/A

            // Act
            var result = await _client.GetRequestAsync(ApiRoutes.InsuranceProviders.GetList);

            // Assert
            result.StatusCode.Should().Be(200);
        }
    }
}