namespace Ordering.IntegrationTests.FeatureTests.Patient
{
    using Ordering.SharedTestHelpers.Fakes.Patient;
    using Ordering.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System;
    using System.Threading.Tasks;
    using Ordering.WebApi.Features.Patients;
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
            var command = new DeletePatient.DeletePatientCommand(patientId);
            await SendAsync(command);
            var patients = await ExecuteDbContextAsync(db => db.Patients.ToListAsync());

            // Assert
            patients.Count.Should().Be(0);
        }

        [Test]
        public async Task DeletePatientCommand_Throws_KeyNotFoundException_When_Record_Does_Not_Exist()
        {
            // Arrange
            var badId = Guid.NewGuid();

            // Act
            var command = new DeletePatient.DeletePatientCommand(badId);
            Func<Task> act = () => SendAsync(command);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
    }
}