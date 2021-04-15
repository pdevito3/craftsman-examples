namespace Ordering.FunctionalTests.FunctionalTests.Sample
{
    using Ordering.SharedTestHelpers.Fakes.Sample;
    using Ordering.Core.Dtos.Sample;
    using Ordering.FunctionalTests.TestUtilities;
    using Microsoft.AspNetCore.JsonPatch;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class PartialSampleUpdateTests : TestBase
    {
        [Test]
        public async Task Patch_Sample_Returns_NoContent()
        {
            // Arrange
            var fakeSample = new FakeSample { }.Generate();
            var patchDoc = new JsonPatchDocument<SampleForUpdateDto>();
            patchDoc.Replace(s => s.ExternalId, "Easily Identified Value For Test");

            await InsertAsync(fakeSample);

            // Act
            var route = ApiRoutes.Samples.Patch.Replace(ApiRoutes.Samples.SampleId, fakeSample.SampleId.ToString());
            var result = await _client.PatchJsonRequestAsync(route, patchDoc);

            // Assert
            result.StatusCode.Should().Be(204);
        }
    }
}