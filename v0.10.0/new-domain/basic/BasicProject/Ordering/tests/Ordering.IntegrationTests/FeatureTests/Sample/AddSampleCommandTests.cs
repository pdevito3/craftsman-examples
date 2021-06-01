namespace Ordering.IntegrationTests.FeatureTests.Sample
{
    using Ordering.SharedTestHelpers.Fakes.Sample;
    using Ordering.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Ordering.WebApi.Features.Samples;
    using static TestFixture;
    using System;
    using Ordering.Core.Exceptions;

    public class AddSampleCommandTests : TestBase
    {
        [Test]
        public async Task AddSampleCommand_Adds_New_Sample_To_Db()
        {
            // Arrange
            var fakeSampleOne = new FakeSampleForCreationDto { }.Generate();

            // Act
            var command = new AddSample.AddSampleCommand(fakeSampleOne);
            var sampleReturned = await SendAsync(command);
            var sampleCreated = await ExecuteDbContextAsync(db => db.Samples.SingleOrDefaultAsync());

            // Assert
            sampleReturned.Should().BeEquivalentTo(fakeSampleOne, options =>
                options.ExcludingMissingMembers());
            sampleCreated.Should().BeEquivalentTo(fakeSampleOne, options =>
                options.ExcludingMissingMembers());
        }

        [Test]
        public async Task AddSampleCommand_Throws_Conflict_When_PK_Guid_Exists()
        {
            // Arrange
            var FakeSample = new FakeSample { }.Generate();
            var conflictRecord = new FakeSampleForCreationDto { }.Generate();
            conflictRecord.SampleId = FakeSample.SampleId;

            await InsertAsync(FakeSample);

            // Act
            var command = new AddSample.AddSampleCommand(conflictRecord);
            Func<Task> act = () => SendAsync(command);

            // Assert
            act.Should().Throw<ConflictException>();
        }
    }
}