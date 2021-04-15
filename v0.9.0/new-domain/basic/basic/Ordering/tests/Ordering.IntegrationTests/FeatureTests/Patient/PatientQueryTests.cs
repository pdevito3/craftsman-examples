namespace Ordering.IntegrationTests.FeatureTests.Patient
{
    using Ordering.SharedTestHelpers.Fakes.Patient;
    using Ordering.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using static Ordering.WebApi.Features.Patients.GetPatient;
    using static TestFixture;

    public class PatientQueryTests : TestBase
    {
        [Test]
        public async Task PatientQuery_Returns_Resource_With_Accurate_Props()
        {
            // Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            await InsertAsync(fakePatientOne);

            // Act
            var query = new PatientQuery(fakePatientOne.PatientId);
            var patients = await SendAsync(query);

            // Assert
            patients.Should().BeEquivalentTo(fakePatientOne, options =>
                options.ExcludingMissingMembers());
        }
    }
}