namespace Billing.IntegrationTests.FeatureTests.Patient
{
    using Billing.SharedTestHelpers.Fakes.Patient;
    using Billing.IntegrationTests.TestUtilities;
    using Billing.Core.Dtos.Patient;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.JsonPatch;
    using System.Linq;
    using Billing.WebApi.Features.Patients;
    using static TestFixture;

    public class UpdatePatientCommandTests : TestBase
    {
        [Test]
        public async Task UpdatePatientCommand_Updates_Existing_Patient_In_Db()
        {
            // Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var updatedPatientDto = new FakePatientForUpdateDto { }.Generate();
            await InsertAsync(fakePatientOne);

            var patient = await ExecuteDbContextAsync(db => db.Patients.SingleOrDefaultAsync());
            var patientId = patient.PatientId;

            // Act
            var command = new UpdatePatient.UpdatePatientCommand(patientId, updatedPatientDto);
            await SendAsync(command);
            var updatedPatient = await ExecuteDbContextAsync(db => db.Patients.Where(p => p.PatientId == patientId).SingleOrDefaultAsync());

            // Assert
            updatedPatient.Should().BeEquivalentTo(updatedPatientDto, options =>
                options.ExcludingMissingMembers());
        }
    }
}