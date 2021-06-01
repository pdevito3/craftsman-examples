namespace Ordering.IntegrationTests.FeatureTests.Sample
{
    using Ordering.SharedTestHelpers.Fakes.Sample;
    using Ordering.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System;
    using System.Threading.Tasks;
    using Ordering.WebApi.Features.Samples;
    using static TestFixture;

    public class DeleteSampleCommandTests : TestBase
    {
        [Test]
        public async Task DeleteSampleCommand_Deletes_Sample_From_Db()
        {
            // Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            await InsertAsync(fakeSampleOne);
            var sample = await ExecuteDbContextAsync(db => db.Samples.SingleOrDefaultAsync());
            var sampleId = sample.SampleId;

            // Act
            var command = new DeleteSample.DeleteSampleCommand(sampleId);
            await SendAsync(command);
            var samples = await ExecuteDbContextAsync(db => db.Samples.ToListAsync());

            // Assert
            samples.Count.Should().Be(0);
        }

        [Test]
        public async Task DeleteSampleCommand_Throws_KeyNotFoundException_When_Record_Does_Not_Exist()
        {
            // Arrange
            var badId = Guid.NewGuid();

            // Act
            var command = new DeleteSample.DeleteSampleCommand(badId);
            Func<Task> act = () => SendAsync(command);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
    }
}