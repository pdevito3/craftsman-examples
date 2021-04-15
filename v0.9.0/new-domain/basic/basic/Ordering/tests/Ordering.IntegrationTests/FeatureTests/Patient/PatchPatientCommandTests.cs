namespace Ordering.IntegrationTests.FeatureTests.Patient
{
    using Ordering.SharedTestHelpers.Fakes.Patient;
    using Ordering.IntegrationTests.TestUtilities;
    using Ordering.Core.Dtos.Patient;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.JsonPatch;
    using System.Linq;
    using static Ordering.WebApi.Features.Patients.PatchPatient;
    using static TestFixture;

    public class PatchPatientCommandTests : TestBase
    {
        [Test]
        public async Task PatchPatientCommand_Updates_Existing_Patient_In_Db()
        {
            // Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            await InsertAsync(fakePatientOne);
            var patient = await ExecuteDbContextAsync(db => db.Patients.SingleOrDefaultAsync());
            var patientId = patient.PatientId;

            var patchDoc = new JsonPatchDocument<PatientForUpdateDto>();
            var newValue = "Easily Identified Value For Test";
            patchDoc.Replace(p => p.ExternalId, newValue);

            // Act
            var command = new PatchPatientCommand(patientId, patchDoc);
            await SendAsync(command);
            var updatedPatient = await ExecuteDbContextAsync(db => db.Patients.Where(p => p.PatientId == patientId).SingleOrDefaultAsync());

            // Assert
            updatedPatient.ExternalId.Should().Be(newValue);
        }
    }
}