namespace Billing.FunctionalTests.FunctionalTests.InsuranceProvider
{
    using Billing.SharedTestHelpers.Fakes.InsuranceProvider;
    using Billing.Core.Dtos.InsuranceProvider;
    using Billing.FunctionalTests.TestUtilities;
    using Microsoft.AspNetCore.JsonPatch;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class PartialInsuranceProviderUpdateTests : TestBase
    {
        [Test]
        public async Task Patch_InsuranceProvider_Returns_NoContent()
        {
            // Arrange
            var fakeInsuranceProvider = new FakeInsuranceProvider { }.Generate();
            var patchDoc = new JsonPatchDocument<InsuranceProviderForUpdateDto>();
            patchDoc.Replace(i => i.ProviderName, "Easily Identified Value For Test");

            await InsertAsync(fakeInsuranceProvider);

            // Act
            var route = ApiRoutes.InsuranceProviders.Patch.Replace(ApiRoutes.InsuranceProviders.InsuranceProviderId, fakeInsuranceProvider.InsuranceProviderId.ToString());
            var result = await _client.PatchJsonRequestAsync(route, patchDoc);

            // Assert
            result.StatusCode.Should().Be(204);
        }
    }
}