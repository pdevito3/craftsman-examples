namespace Ordering.IntegrationTests.FeatureTests.Sample
{
    using Ordering.SharedTestHelpers.Fakes.Sample;
    using Ordering.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ordering.WebApi.Features.Samples;
    using static TestFixture;

    public class SampleQueryTests : TestBase
    {
        [Test]
        public async Task SampleQuery_Returns_Resource_With_Accurate_Props()
        {
            // Arrange
            var fakeSampleOne = new FakeSample { }.Generate();
            await InsertAsync(fakeSampleOne);

            // Act
            var query = new GetSample.SampleQuery(fakeSampleOne.SampleId);
            var samples = await SendAsync(query);

            // Assert
            samples.Should().BeEquivalentTo(fakeSampleOne, options =>
                options.ExcludingMissingMembers());
        }

        [Test]
        public async Task SampleQuery_Throws_KeyNotFoundException_When_Record_Does_Not_Exist()
        {
            // Arrange
            var badId = Guid.NewGuid();

            // Act
            var query = new GetSample.SampleQuery(badId);
            Func<Task> act = () => SendAsync(query);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
    }
}