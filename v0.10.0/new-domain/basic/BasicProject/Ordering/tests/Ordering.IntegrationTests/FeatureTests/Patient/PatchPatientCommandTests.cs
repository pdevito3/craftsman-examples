namespace Ordering.IntegrationTests.FeatureTests.Patient
{
    using Ordering.SharedTestHelpers.Fakes.Patient;
    using Ordering.IntegrationTests.TestUtilities;
    using Ordering.Core.Dtos.Patient;
    using Ordering.Core.Exceptions;
    using Ordering.WebApi.Features.Patients;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.JsonPatch;
    using System;
    using System.Linq;
    using System.Collections.Generic;
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
            var command = new PatchPatient.PatchPatientCommand(patientId, patchDoc);
            await SendAsync(command);
            var updatedPatient = await ExecuteDbContextAsync(db => db.Patients.Where(p => p.PatientId == patientId).SingleOrDefaultAsync());

            // Assert
            updatedPatient.ExternalId.Should().Be(newValue);
        }
        
        [Test]
        public async Task PatchPatientCommand_Throws_KeyNotFoundException_When_Bad_PK()
        {
            // Arrange
            var badId = Guid.NewGuid();
            var patchDoc = new JsonPatchDocument<PatientForUpdateDto>();

            // Act
            var command = new PatchPatient.PatchPatientCommand(badId, patchDoc);
            Func<Task> act = () => SendAsync(command);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }

        [Test]
        public async Task PatchPatientCommand_Throws_ApiException_When_Null_Patchdoc()
        {
            // Arrange
            var randomId = Guid.NewGuid();

            // Act
            var command = new PatchPatient.PatchPatientCommand(randomId, null);
            Func<Task> act = () => SendAsync(command);

            // Assert
            act.Should().Throw<ApiException>();
        }
    }
}