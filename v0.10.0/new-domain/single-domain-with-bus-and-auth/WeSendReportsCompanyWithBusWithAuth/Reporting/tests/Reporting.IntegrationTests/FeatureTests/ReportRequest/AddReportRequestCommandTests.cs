namespace Reporting.IntegrationTests.FeatureTests.ReportRequest
{
    using Reporting.SharedTestHelpers.Fakes.ReportRequest;
    using Reporting.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Reporting.WebApi.Features.ReportRequests;
    using static TestFixture;
    using System;
    using Reporting.Core.Exceptions;

    public class AddReportRequestCommandTests : TestBase
    {
        [Test]
        public async Task AddReportRequestCommand_Adds_New_ReportRequest_To_Db()
        {
            // Arrange
            var fakeReportRequestOne = new FakeReportRequestForCreationDto { }.Generate();

            // Act
            var command = new AddReportRequest.AddReportRequestCommand(fakeReportRequestOne);
            var reportRequestReturned = await SendAsync(command);
            var reportRequestCreated = await ExecuteDbContextAsync(db => db.ReportRequests.SingleOrDefaultAsync());

            // Assert
            reportRequestReturned.Should().BeEquivalentTo(fakeReportRequestOne, options =>
                options.ExcludingMissingMembers());
            reportRequestCreated.Should().BeEquivalentTo(fakeReportRequestOne, options =>
                options.ExcludingMissingMembers());
        }

        [Test]
        public async Task AddReportRequestCommand_Throws_Conflict_When_PK_Guid_Exists()
        {
            // Arrange
            var FakeReportRequest = new FakeReportRequest { }.Generate();
            var conflictRecord = new FakeReportRequestForCreationDto { }.Generate();
            conflictRecord.ReportId = FakeReportRequest.ReportId;

            await InsertAsync(FakeReportRequest);

            // Act
            var command = new AddReportRequest.AddReportRequestCommand(conflictRecord);
            Func<Task> act = () => SendAsync(command);

            // Assert
            act.Should().Throw<ConflictException>();
        }
    }
}