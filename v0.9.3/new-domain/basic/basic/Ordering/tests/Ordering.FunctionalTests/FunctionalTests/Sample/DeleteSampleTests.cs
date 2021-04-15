namespace Ordering.FunctionalTests.FunctionalTests.Sample
{
    using Ordering.SharedTestHelpers.Fakes.Sample;
    using Ordering.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class DeleteSampleTests : TestBase
    {
        [Test]
        public async Task Delete_SampleReturns_NoContent()
        {
            // Arrange
            var fakeSample = new FakeSample { }.Generate();

            await InsertAsync(fakeSample);

            // Act
            var route = ApiRoutes.Samples.Delete.Replace(ApiRoutes.Samples.SampleId, fakeSample.SampleId.ToString());
            var result = await _client.DeleteRequestAsync(route);

            // Assert
            result.StatusCode.Should().Be(204);
        }
    }
}