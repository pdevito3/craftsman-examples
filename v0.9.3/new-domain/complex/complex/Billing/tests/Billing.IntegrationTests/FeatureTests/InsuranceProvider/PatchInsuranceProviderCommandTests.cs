namespace Billing.IntegrationTests.FeatureTests.InsuranceProvider
{
    using Billing.SharedTestHelpers.Fakes.InsuranceProvider;
    using Billing.IntegrationTests.TestUtilities;
    using Billing.Core.Dtos.InsuranceProvider;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.JsonPatch;
    using System.Linq;
    using static Billing.WebApi.Features.InsuranceProviders.PatchInsuranceProvider;
    using static TestFixture;

    public class PatchInsuranceProviderCommandTests : TestBase
    {
        [Test]
        public async Task PatchInsuranceProviderCommand_Updates_Existing_InsuranceProvider_In_Db()
        {
            // Arrange
            var fakeInsuranceProviderOne = new FakeInsuranceProvider { }.Generate();
            await InsertAsync(fakeInsuranceProviderOne);
            var insuranceProvider = await ExecuteDbContextAsync(db => db.InsuranceProviders.SingleOrDefaultAsync());
            var insuranceProviderId = insuranceProvider.InsuranceProviderId;

            var patchDoc = new JsonPatchDocument<InsuranceProviderForUpdateDto>();
            var newValue = "Easily Identified Value For Test";
            patchDoc.Replace(i => i.ProviderName, newValue);

            // Act
            var command = new PatchInsuranceProviderCommand(insuranceProviderId, patchDoc);
            await SendAsync(command);
            var updatedInsuranceProvider = await ExecuteDbContextAsync(db => db.InsuranceProviders.Where(i => i.InsuranceProviderId == insuranceProviderId).SingleOrDefaultAsync());

            // Assert
            updatedInsuranceProvider.ProviderName.Should().Be(newValue);
        }
    }
}