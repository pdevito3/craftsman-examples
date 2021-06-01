namespace Reporting.IntegrationTests.FeatureTests.ReportRequest
{
    using Reporting.SharedTestHelpers.Fakes.ReportRequest;
    using Reporting.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System;
    using System.Threading.Tasks;
    using Reporting.WebApi.Features.ReportRequests;
    using static TestFixture;

    public class DeleteReportRequestCommandTests : TestBase
    {
        [Test]
        public async Task DeleteReportRequestCommand_Deletes_ReportRequest_From_Db()
        {
            // Arrange
            var fakeReportRequestOne = new FakeReportRequest { }.Generate();
            await InsertAsync(fakeReportRequestOne);
            var reportRequest = await ExecuteDbContextAsync(db => db.ReportRequests.SingleOrDefaultAsync());
            var reportId = reportRequest.ReportId;

            // Act
            var command = new DeleteReportRequest.DeleteReportRequestCommand(reportId);
            await SendAsync(command);
            var reportRequests = await ExecuteDbContextAsync(db => db.ReportRequests.ToListAsync());

            // Assert
            reportRequests.Count.Should().Be(0);
        }

        [Test]
        public async Task DeleteReportRequestCommand_Throws_KeyNotFoundException_When_Record_Does_Not_Exist()
        {
            // Arrange
            var badId = Guid.NewGuid();

            // Act
            var command = new DeleteReportRequest.DeleteReportRequestCommand(badId);
            Func<Task> act = () => SendAsync(command);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
    }
}