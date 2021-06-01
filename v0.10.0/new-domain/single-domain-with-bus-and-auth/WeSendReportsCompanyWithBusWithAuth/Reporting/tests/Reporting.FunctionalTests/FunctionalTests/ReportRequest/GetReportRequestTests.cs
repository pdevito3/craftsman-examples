namespace Reporting.FunctionalTests.FunctionalTests.ReportRequest
{
    using Reporting.SharedTestHelpers.Fakes.ReportRequest;
    using Reporting.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class GetReportRequestTests : TestBase
    {
        [Test]
        public async Task Get_ReportRequest_Record_Returns_NoContent()
        {
            // Arrange
            var fakeReportRequest = new FakeReportRequest { }.Generate();

            await InsertAsync(fakeReportRequest);

            // Act
            var route = ApiRoutes.ReportRequests.GetRecord.Replace(ApiRoutes.ReportRequests.ReportId, fakeReportRequest.ReportId.ToString());
            var result = await _client.GetRequestAsync(route);

            // Assert
            result.StatusCode.Should().Be(200);
        }
    }
}