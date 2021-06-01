namespace Reporting.FunctionalTests.FunctionalTests.ReportRequest
{
    using Reporting.SharedTestHelpers.Fakes.ReportRequest;
    using Reporting.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class UpdateReportRequestRecordTests : TestBase
    {
        [Test]
        public async Task Put_ReportRequest_Returns_NoContent()
        {
            // Arrange
            var fakeReportRequest = new FakeReportRequest { }.Generate();
            var updatedReportRequestDto = new FakeReportRequestForUpdateDto { }.Generate();

            await InsertAsync(fakeReportRequest);

            // Act
            var route = ApiRoutes.ReportRequests.Put.Replace(ApiRoutes.ReportRequests.ReportId, fakeReportRequest.ReportId.ToString());
            var result = await _client.PutJsonRequestAsync(route, updatedReportRequestDto);

            // Assert
            result.StatusCode.Should().Be(204);
        }
    }
}