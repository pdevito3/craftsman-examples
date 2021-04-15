namespace Ordering.IntegrationTests.FeatureTests.Patient
{
    using Ordering.SharedTestHelpers.Fakes.Patient;
    using Ordering.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using static Ordering.WebApi.Features.Patients.DeletePatient;
    using static TestFixture;

    public class DeletePatientCommandTests : TestBase
    {
        [Test]
        public async Task DeletePatientCommand_Deletes_Patient_From_Db()
        {
            // Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            await InsertAsync(fakePatientOne);
            var patient = await ExecuteDbContextAsync(db => db.Patients.SingleOrDefaultAsync());
            var patientId = patient.PatientId;

            // Act
            var command = new DeletePatientCommand(patientId);
            await SendAsync(command);
            var patients = await ExecuteDbContextAsync(db => db.Patients.ToListAsync());

            // Assert
            patients.Count.Should().Be(0);
        }
    }
}