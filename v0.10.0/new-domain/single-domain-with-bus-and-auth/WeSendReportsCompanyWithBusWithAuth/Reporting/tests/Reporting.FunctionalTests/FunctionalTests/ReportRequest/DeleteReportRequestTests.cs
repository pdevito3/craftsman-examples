namespace Reporting.FunctionalTests.FunctionalTests.ReportRequest
{
    using Reporting.SharedTestHelpers.Fakes.ReportRequest;
    using Reporting.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class DeleteReportRequestTests : TestBase
    {
        [Test]
        public async Task Delete_ReportRequestReturns_NoContent_WithAuth()
        {
            // Arrange
            var fakeReportRequest = new FakeReportRequest { }.Generate();

            _client.AddAuth(new[] {"reportrequest.delete"});

            await InsertAsync(fakeReportRequest);

            // Act
            var route = ApiRoutes.ReportRequests.Delete.Replace(ApiRoutes.ReportRequests.ReportId, fakeReportRequest.ReportId.ToString());
            var result = await _client.DeleteRequestAsync(route);

            // Assert
            result.StatusCode.Should().Be(204);
        }
            
        [Test]
        public async Task Delete_ReportRequest_Returns_Unauthorized_Without_Valid_Token()
        {
            // Arrange
            var fakeReportRequest = new FakeReportRequest { }.Generate();

            await InsertAsync(fakeReportRequest);

            // Act
            var route = ApiRoutes.ReportRequests.Delete.Replace(ApiRoutes.ReportRequests.ReportId, fakeReportRequest.ReportId.ToString());
            var result = await _client.DeleteRequestAsync(route);

            // Assert
            result.StatusCode.Should().Be(401);
        }
            
        [Test]
        public async Task Delete_ReportRequest_Returns_Forbidden_Without_Proper_Scope()
        {
            // Arrange
            var fakeReportRequest = new FakeReportRequest { }.Generate();
            _client.AddAuth();

            await InsertAsync(fakeReportRequest);

            // Act
            var route = ApiRoutes.ReportRequests.Delete.Replace(ApiRoutes.ReportRequests.ReportId, fakeReportRequest.ReportId.ToString());
            var result = await _client.DeleteRequestAsync(route);

            // Assert
            result.StatusCode.Should().Be(403);
        }
    }
}