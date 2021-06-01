namespace Reporting.IntegrationTests.FeatureTests.ReportRequest
{
    using Reporting.SharedTestHelpers.Fakes.ReportRequest;
    using Reporting.IntegrationTests.TestUtilities;
    using Reporting.Core.Dtos.ReportRequest;
    using Reporting.Core.Exceptions;
    using Reporting.WebApi.Features.ReportRequests;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.JsonPatch;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using static TestFixture;

    public class PatchReportRequestCommandTests : TestBase
    {
        [Test]
        public async Task PatchReportRequestCommand_Updates_Existing_ReportRequest_In_Db()
        {
            // Arrange
            var fakeReportRequestOne = new FakeReportRequest { }.Generate();
            await InsertAsync(fakeReportRequestOne);
            var reportRequest = await ExecuteDbContextAsync(db => db.ReportRequests.SingleOrDefaultAsync());
            var reportId = reportRequest.ReportId;

            var patchDoc = new JsonPatchDocument<ReportRequestForUpdateDto>();
            var newValue = "Easily Identified Value For Test";
            patchDoc.Replace(r => r.Provider, newValue);

            // Act
            var command = new PatchReportRequest.PatchReportRequestCommand(reportId, patchDoc);
            await SendAsync(command);
            var updatedReportRequest = await ExecuteDbContextAsync(db => db.ReportRequests.Where(r => r.ReportId == reportId).SingleOrDefaultAsync());

            // Assert
            updatedReportRequest.Provider.Should().Be(newValue);
        }
        
        [Test]
        public async Task PatchReportRequestCommand_Throws_KeyNotFoundException_When_Bad_PK()
        {
            // Arrange
            var badId = Guid.NewGuid();
            var patchDoc = new JsonPatchDocument<ReportRequestForUpdateDto>();

            // Act
            var command = new PatchReportRequest.PatchReportRequestCommand(badId, patchDoc);
            Func<Task> act = () => SendAsync(command);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }

        [Test]
        public async Task PatchReportRequestCommand_Throws_ApiException_When_Null_Patchdoc()
        {
            // Arrange
            var randomId = Guid.NewGuid();

            // Act
            var command = new PatchReportRequest.PatchReportRequestCommand(randomId, null);
            Func<Task> act = () => SendAsync(command);

            // Assert
            act.Should().Throw<ApiException>();
        }
    }
}