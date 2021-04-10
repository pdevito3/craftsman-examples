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
    using static Ordering.WebApi.Features.Samples.PatchSample;
    using static TestFixture;

    public class PatchSampleCommandTests : TestBase
    {
        [Test]
        public async Task PatchSampleCommand_Updates_Existing_Sample_In_Db()
        {
            // Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            await InsertAsync(fakeSampleOne);
            var sample = await ExecuteDbContextAsync(db => db.Samples.SingleOrDefaultAsync());
            var sampleId = sample.SampleId;

            var patchDoc = new JsonPatchDocument<SampleForUpdateDto>();
            var newValue = "Easily Identified Value For Test";
            patchDoc.Replace(s => s.ExternalId, newValue);

            // Act
            var command = new PatchSampleCommand(sampleId, patchDoc);
            await SendAsync(command);
            var updatedSample = await ExecuteDbContextAsync(db => db.Samples.Where(s => s.SampleId == sampleId).SingleOrDefaultAsync());

            // Assert
            updatedSample.ExternalId.Should().Be(newValue);
        }
    }
}