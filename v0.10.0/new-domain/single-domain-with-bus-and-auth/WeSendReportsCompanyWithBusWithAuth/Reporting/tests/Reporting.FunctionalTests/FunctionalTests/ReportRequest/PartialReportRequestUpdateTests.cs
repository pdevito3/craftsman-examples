namespace Reporting.FunctionalTests.FunctionalTests.ReportRequest
{
    using Reporting.SharedTestHelpers.Fakes.ReportRequest;
    using Reporting.Core.Dtos.ReportRequest;
    using Reporting.FunctionalTests.TestUtilities;
    using Microsoft.AspNetCore.JsonPatch;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class PartialReportRequestUpdateTests : TestBase
    {
        [Test]
        public async Task Patch_ReportRequest_Returns_NoContent()
        {
            // Arrange
            var fakeReportRequest = new FakeReportRequest { }.Generate();
            var patchDoc = new JsonPatchDocument<ReportRequestForUpdateDto>();
            patchDoc.Replace(r => r.Provider, "Easily Identified Value For Test");

            await InsertAsync(fakeReportRequest);

            // Act
            var route = ApiRoutes.ReportRequests.Patch.Replace(ApiRoutes.ReportRequests.ReportId, fakeReportRequest.ReportId.ToString());
            var result = await _client.PatchJsonRequestAsync(route, patchDoc);

            // Assert
            result.StatusCode.Should().Be(204);
        }
    }
}