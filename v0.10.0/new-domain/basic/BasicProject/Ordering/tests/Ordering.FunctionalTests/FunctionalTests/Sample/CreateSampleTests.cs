namespace Ordering.FunctionalTests.FunctionalTests.Sample
{
    using Ordering.SharedTestHelpers.Fakes.Sample;
    using Ordering.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class CreateSampleTests : TestBase
    {
        [Test]
        public async Task Create_Sample_Returns_Created()
        {
            // Arrange
            var fakeSample = new FakeSampleForCreationDto { }.Generate();

            // Act
            var route = ApiRoutes.Samples.Create;
            var result = await _client.PostJsonRequestAsync(route, fakeSample);

            // Assert
            result.StatusCode.Should().Be(201);
        }
    }
}