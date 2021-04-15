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
    using static Billing.WebApi.Features.InsuranceProviders.UpdateInsuranceProvider;
    using static TestFixture;

    public class UpdateInsuranceProviderCommandTests : TestBase
    {
        [Test]
        public async Task UpdateInsuranceProviderCommand_Updates_Existing_InsuranceProvider_In_Db()
        {
            // Arrange
            var fakeInsuranceProviderOne = new FakeInsuranceProvider { }.Generate();
            var updatedInsuranceProviderDto = new FakeInsuranceProviderForUpdateDto { }.Generate();
            await InsertAsync(fakeInsuranceProviderOne);

            var insuranceProvider = await ExecuteDbContextAsync(db => db.InsuranceProviders.SingleOrDefaultAsync());
            var insuranceProviderId = insuranceProvider.InsuranceProviderId;

            // Act
            var command = new UpdateInsuranceProviderCommand(insuranceProviderId, updatedInsuranceProviderDto);
            await SendAsync(command);
            var updatedInsuranceProvider = await ExecuteDbContextAsync(db => db.InsuranceProviders.Where(i => i.InsuranceProviderId == insuranceProviderId).SingleOrDefaultAsync());

            // Assert
            updatedInsuranceProvider.Should().BeEquivalentTo(updatedInsuranceProviderDto, options =>
                options.ExcludingMissingMembers());
        }
    }
}