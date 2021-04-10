namespace Billing.IntegrationTests.FeatureTests.InsuranceProvider
{
    using Billing.SharedTestHelpers.Fakes.InsuranceProvider;
    using Billing.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using static Billing.WebApi.Features.InsuranceProviders.DeleteInsuranceProvider;
    using static TestFixture;

    public class DeleteInsuranceProviderCommandTests : TestBase
    {
        [Test]
        public async Task DeleteInsuranceProviderCommand_Deletes_InsuranceProvider_From_Db()
        {
            // Arrange
            var fakeInsuranceProviderOne = new FakeInsuranceProvider { }.Generate();
            await InsertAsync(fakeInsuranceProviderOne);
            var insuranceProvider = await ExecuteDbContextAsync(db => db.InsuranceProviders.SingleOrDefaultAsync());
            var insuranceProviderId = insuranceProvider.InsuranceProviderId;

            // Act
            var command = new DeleteInsuranceProviderCommand(insuranceProviderId);
            await SendAsync(command);
            var insuranceProviders = await ExecuteDbContextAsync(db => db.InsuranceProviders.ToListAsync());

            // Assert
            insuranceProviders.Count.Should().Be(0);
        }
    }
}