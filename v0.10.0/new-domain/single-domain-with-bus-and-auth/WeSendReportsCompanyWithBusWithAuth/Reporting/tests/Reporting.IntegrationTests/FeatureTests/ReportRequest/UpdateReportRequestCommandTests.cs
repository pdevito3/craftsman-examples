namespace Reporting.IntegrationTests.FeatureTests.ReportRequest
{
    using Reporting.SharedTestHelpers.Fakes.ReportRequest;
    using Reporting.IntegrationTests.TestUtilities;
    using Reporting.Core.Dtos.ReportRequest;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.JsonPatch;
    using System.Linq;
    using Reporting.WebApi.Features.ReportRequests;
    using static TestFixture;

    public class UpdateReportRequestCommandTests : TestBase
    {
        [Test]
        public async Task UpdateReportRequestCommand_Updates_Existing_ReportRequest_In_Db()
        {
            // Arrange
            var fakeReportRequestOne = new FakeReportRequest { }.Generate();
            var updatedReportRequestDto = new FakeReportRequestForUpdateDto { }.Generate();
            await InsertAsync(fakeReportRequestOne);

            var reportRequest = await ExecuteDbContextAsync(db => db.ReportRequests.SingleOrDefaultAsync());
            var reportId = reportRequest.ReportId;

            // Act
            var command = new UpdateReportRequest.UpdateReportRequestCommand(reportId, updatedReportRequestDto);
            await SendAsync(command);
            var updatedReportRequest = await ExecuteDbContextAsync(db => db.ReportRequests.Where(r => r.ReportId == reportId).SingleOrDefaultAsync());

            // Assert
            updatedReportRequest.Should().BeEquivalentTo(updatedReportRequestDto, options =>
                options.ExcludingMissingMembers());
        }
    }
}