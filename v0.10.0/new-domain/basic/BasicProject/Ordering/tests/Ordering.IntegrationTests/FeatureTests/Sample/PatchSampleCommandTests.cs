namespace Ordering.IntegrationTests.FeatureTests.Sample
{
    using Ordering.SharedTestHelpers.Fakes.Sample;
    using Ordering.IntegrationTests.TestUtilities;
    using Ordering.Core.Dtos.Sample;
    using Ordering.Core.Exceptions;
    using Ordering.WebApi.Features.Samples;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.JsonPatch;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using static TestFixture;

    public class PatchSampleCommandTests : TestBase
    {
        [Test]
        public async Task PatchSampleCommand_Updates_Existing_Sample_In_Db()
        {
            // Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            await InsertAsync(fakeSampleOne);
            var sample = await ExecuteDbContextAsync(db => db.Samples.SingleOrDefaultAsync());
            var sampleId = sample.SampleId;

            var patchDoc = new JsonPatchDocument<SampleForUpdateDto>();
            var newValue = "Easily Identified Value For Test";
            patchDoc.Replace(s => s.ExternalId, newValue);

            // Act
            var command = new PatchSample.PatchSampleCommand(sampleId, patchDoc);
            await SendAsync(command);
            var updatedSample = await ExecuteDbContextAsync(db => db.Samples.Where(s => s.SampleId == sampleId).SingleOrDefaultAsync());

            // Assert
            updatedSample.ExternalId.Should().Be(newValue);
        }
        
        [Test]
        public async Task PatchSampleCommand_Throws_KeyNotFoundException_When_Bad_PK()
        {
            // Arrange
            var badId = Guid.NewGuid();
            var patchDoc = new JsonPatchDocument<SampleForUpdateDto>();

            // Act
            var command = new PatchSample.PatchSampleCommand(badId, patchDoc);
            Func<Task> act = () => SendAsync(command);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }

        [Test]
        public async Task PatchSampleCommand_Throws_ApiException_When_Null_Patchdoc()
        {
            // Arrange
            var randomId = Guid.NewGuid();

            // Act
            var command = new PatchSample.PatchSampleCommand(randomId, null);
            Func<Task> act = () => SendAsync(command);

            // Assert
            act.Should().Throw<ApiException>();
        }
    }
}