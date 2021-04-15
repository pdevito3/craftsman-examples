namespace Ordering.FunctionalTests.FunctionalTests.Sample
{
    using Ordering.SharedTestHelpers.Fakes.Sample;
    using Ordering.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class UpdateSampleRecordTests : TestBase
    {
        [Test]
        public async Task Put_Sample_Returns_NoContent()
        {
            // Arrange
            var fakeSample = new FakeSample { }.Generate();
            var updatedSampleDto = new FakeSampleForUpdateDto { }.Generate();

            await InsertAsync(fakeSample);

            // Act
            var route = ApiRoutes.Samples.Put.Replace(ApiRoutes.Samples.SampleId, fakeSample.SampleId.ToString());
            var result = await _client.PutJsonRequestAsync(route, updatedSampleDto);

            // Assert
            result.StatusCode.Should().Be(204);
        }
    }
}