namespace Ordering.IntegrationTests.FeatureTests.Sample
{
    using Ordering.SharedTestHelpers.Fakes.Sample;
    using Ordering.IntegrationTests.TestUtilities;
    using Ordering.Core.Dtos.Sample;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.JsonPatch;
    using System.Linq;
    using static Ordering.WebApi.Features.Samples.UpdateSample;
    using static TestFixture;

    public class UpdateSampleCommandTests : TestBase
    {
        [Test]
        public async Task UpdateSampleCommand_Updates_Existing_Sample_In_Db()
        {
            // Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            var updatedSampleDto = new FakeSampleForUpdateDto { }.Generate();
            await InsertAsync(fakeSampleOne);

            var sample = await ExecuteDbContextAsync(db => db.Samples.SingleOrDefaultAsync());
            var sampleId = sample.SampleId;

            // Act
            var command = new UpdateSampleCommand(sampleId, updatedSampleDto);
            await SendAsync(command);
            var updatedSample = await ExecuteDbContextAsync(db => db.Samples.Where(s => s.SampleId == sampleId).SingleOrDefaultAsync());

            // Assert
            updatedSample.Should().BeEquivalentTo(updatedSampleDto, options =>
                options.ExcludingMissingMembers());
        }
    }
}