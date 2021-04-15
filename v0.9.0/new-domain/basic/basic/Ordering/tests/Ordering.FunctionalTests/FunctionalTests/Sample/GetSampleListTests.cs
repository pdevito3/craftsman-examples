namespace Ordering.FunctionalTests.FunctionalTests.Sample
{
    using Ordering.SharedTestHelpers.Fakes.Sample;
    using Ordering.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class GetSampleListTests : TestBase
    {
        [Test]
        public async Task Get_Sample_List_Returns_NoContent()
        {
            // Arrange
            // N/A

            // Act
            var result = await _client.GetRequestAsync(ApiRoutes.Samples.GetList);

            // Assert
            result.StatusCode.Should().Be(200);
        }
    }
}