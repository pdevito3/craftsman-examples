namespace Ordering.IntegrationTests.FeatureTests.Sample
{
    using Ordering.SharedTestHelpers.Fakes.Sample;
    using Ordering.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using static Ordering.WebApi.Features.Samples.GetSample;
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
            var query = new SampleQuery(fakeSampleOne.SampleId);
            var samples = await SendAsync(query);

            // Assert
            samples.Should().BeEquivalentTo(fakeSampleOne, options =>
                options.ExcludingMissingMembers());
        }
    }
}