namespace Ordering.FunctionalTests.FunctionalTests.Sample
{
    using Ordering.SharedTestHelpers.Fakes.Sample;
    using Ordering.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class GetSampleTests : TestBase
    {
        [Test]
        public async Task Get_Sample_Record_Returns_NoContent()
        {
            // Arrange
            var fakeSample = new FakeSample { }.Generate();

            await InsertAsync(fakeSample);

            // Act
            var route = ApiRoutes.Samples.GetRecord.Replace(ApiRoutes.Samples.SampleId, fakeSample.SampleId.ToString());
            var result = await _client.GetRequestAsync(route);

            // Assert
            result.StatusCode.Should().Be(200);
        }
    }
}