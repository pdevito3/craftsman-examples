namespace Ordering.FunctionalTests.FunctionalTests.Patient
{
    using Ordering.SharedTestHelpers.Fakes.Patient;
    using Ordering.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class UpdatePatientRecordTests : TestBase
    {
        [Test]
        public async Task Put_Patient_Returns_NoContent()
        {
            // Arrange
            var fakePatient = new FakePatient { }.Generate();
            var updatedPatientDto = new FakePatientForUpdateDto { }.Generate();

            await InsertAsync(fakePatient);

            // Act
            var route = ApiRoutes.Patients.Put.Replace(ApiRoutes.Patients.PatientId, fakePatient.PatientId.ToString());
            var result = await _client.PutJsonRequestAsync(route, updatedPatientDto);

            // Assert
            result.StatusCode.Should().Be(204);
        }
    }
}