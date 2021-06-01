namespace Reporting.IntegrationTests.FeatureTests.ReportRequest
{
    using Reporting.SharedTestHelpers.Fakes.ReportRequest;
    using Reporting.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Reporting.WebApi.Features.ReportRequests;
    using static TestFixture;

    public class ReportRequestQueryTests : TestBase
    {
        [Test]
        public async Task ReportRequestQuery_Returns_Resource_With_Accurate_Props()
        {
            // Arrange
            var fakeReportRequestOne = new FakeReportRequest { }.Generate();
            await InsertAsync(fakeReportRequestOne);

            // Act
            var query = new GetReportRequest.ReportRequestQuery(fakeReportRequestOne.ReportId);
            var reportRequests = await SendAsync(query);

            // Assert
            reportRequests.Should().BeEquivalentTo(fakeReportRequestOne, options =>
                options.ExcludingMissingMembers());
        }

        [Test]
        public async Task ReportRequestQuery_Throws_KeyNotFoundException_When_Record_Does_Not_Exist()
        {
            // Arrange
            var badId = Guid.NewGuid();

            // Act
            var query = new GetReportRequest.ReportRequestQuery(badId);
            Func<Task> act = () => SendAsync(query);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
    }
}