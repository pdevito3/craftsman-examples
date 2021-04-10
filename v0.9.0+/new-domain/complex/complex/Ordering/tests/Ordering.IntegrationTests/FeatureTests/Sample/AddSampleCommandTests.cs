namespace Ordering.IntegrationTests.FeatureTests.Sample
{
    using Ordering.SharedTestHelpers.Fakes.Sample;
    using Ordering.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using static Ordering.WebApi.Features.Samples.AddSample;
    using static TestFixture;

    public class AddSampleCommandTests : TestBase
    {
        [Test]
        public async Task AddSampleCommand_Adds_New_Sample_To_Db()
        {
            // Arrange
            var fakeSampleOne = new FakeSampleForCreationDto { }.Generate();

            // Act
            var command = new AddSampleCommand(fakeSampleOne);
            var sampleReturned = await SendAsync(command);
            var sampleCreated = await ExecuteDbContextAsync(db => db.Samples.SingleOrDefaultAsync());

            // Assert
            sampleReturned.Should().BeEquivalentTo(fakeSampleOne, options =>
                options.ExcludingMissingMembers());
            sampleCreated.Should().BeEquivalentTo(fakeSampleOne, options =>
                options.ExcludingMissingMembers());
        }
    }
}