namespace Ordering.FunctionalTests.FunctionalTests.HealthChecks
{
    using Ordering.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class HealthCheckTests : TestBase
    {
        [Test]
        public async Task Health_Check_Returns_Ok()
        {
            // Arrange
            // N/A

            // Act
            var result = await _client.GetRequestAsync(ApiRoutes.Health);

            // Assert
            result.StatusCode.Should().Be(200);
        }
    }
}