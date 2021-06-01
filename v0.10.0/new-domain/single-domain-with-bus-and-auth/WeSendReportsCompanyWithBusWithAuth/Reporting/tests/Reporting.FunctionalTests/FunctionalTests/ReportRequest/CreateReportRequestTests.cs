namespace Reporting.FunctionalTests.FunctionalTests.ReportRequest
{
    using Reporting.SharedTestHelpers.Fakes.ReportRequest;
    using Reporting.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class CreateReportRequestTests : TestBase
    {
        [Test]
        public async Task Create_ReportRequest_Returns_Created()
        {
            // Arrange
            var fakeReportRequest = new FakeReportRequestForCreationDto { }.Generate();

            // Act
            var route = ApiRoutes.ReportRequests.Create;
            var result = await _client.PostJsonRequestAsync(route, fakeReportRequest);

            // Assert
            result.StatusCode.Should().Be(201);
        }
    }
}