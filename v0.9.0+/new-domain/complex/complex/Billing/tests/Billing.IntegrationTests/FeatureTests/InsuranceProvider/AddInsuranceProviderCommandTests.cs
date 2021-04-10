namespace Billing.IntegrationTests.FeatureTests.InsuranceProvider
{
    using Billing.SharedTestHelpers.Fakes.InsuranceProvider;
    using Billing.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using static Billing.WebApi.Features.InsuranceProviders.AddInsuranceProvider;
    using static TestFixture;

    public class AddInsuranceProviderCommandTests : TestBase
    {
        [Test]
        public async Task AddInsuranceProviderCommand_Adds_New_InsuranceProvider_To_Db()
        {
            // Arrange
            var fakeInsuranceProviderOne = new FakeInsuranceProviderForCreationDto { }.Generate();

            // Act
            var command = new AddInsuranceProviderCommand(fakeInsuranceProviderOne);
            var insuranceProviderReturned = await SendAsync(command);
            var insuranceProviderCreated = await ExecuteDbContextAsync(db => db.InsuranceProviders.SingleOrDefaultAsync());

            // Assert
            insuranceProviderReturned.Should().BeEquivalentTo(fakeInsuranceProviderOne, options =>
                options.ExcludingMissingMembers());
            insuranceProviderCreated.Should().BeEquivalentTo(fakeInsuranceProviderOne, options =>
                options.ExcludingMissingMembers());
        }
    }
}