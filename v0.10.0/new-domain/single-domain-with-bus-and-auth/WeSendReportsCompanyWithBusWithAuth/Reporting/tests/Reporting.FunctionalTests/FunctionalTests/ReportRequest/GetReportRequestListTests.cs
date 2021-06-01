namespace Reporting.FunctionalTests.FunctionalTests.ReportRequest
{
    using Reporting.SharedTestHelpers.Fakes.ReportRequest;
    using Reporting.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class GetReportRequestListTests : TestBase
    {
        [Test]
        public async Task Get_ReportRequest_List_Returns_NoContent()
        {
            // Arrange
            // N/A

            // Act
            var result = await _client.GetRequestAsync(ApiRoutes.ReportRequests.GetList);

            // Assert
            result.StatusCode.Should().Be(200);
        }
    }
}