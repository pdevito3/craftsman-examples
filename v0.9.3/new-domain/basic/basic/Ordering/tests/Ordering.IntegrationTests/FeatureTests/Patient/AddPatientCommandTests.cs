namespace Ordering.IntegrationTests.FeatureTests.Patient
{
    using Ordering.SharedTestHelpers.Fakes.Patient;
    using Ordering.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using static Ordering.WebApi.Features.Patients.AddPatient;
    using static TestFixture;

    public class AddPatientCommandTests : TestBase
    {
        [Test]
        public async Task AddPatientCommand_Adds_New_Patient_To_Db()
        {
            // Arrange
            var fakePatientOne = new FakePatientForCreationDto { }.Generate();

            // Act
            var command = new AddPatientCommand(fakePatientOne);
            var patientReturned = await SendAsync(command);
            var patientCreated = await ExecuteDbContextAsync(db => db.Patients.SingleOrDefaultAsync());

            // Assert
            patientReturned.Should().BeEquivalentTo(fakePatientOne, options =>
                options.ExcludingMissingMembers());
            patientCreated.Should().BeEquivalentTo(fakePatientOne, options =>
                options.ExcludingMissingMembers());
        }
    }
}