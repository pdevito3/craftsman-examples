namespace Ordering.FunctionalTests.FunctionalTests.Patient
{
    using Ordering.SharedTestHelpers.Fakes.Patient;
    using Ordering.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class DeletePatientTests : TestBase
    {
        [Test]
        public async Task Delete_PatientReturns_NoContent_WithAuth()
        {
            // Arrange
            var fakePatient = new FakePatient { }.Generate();

            _client.AddAuth(new[] {"patients.delete"});

            await InsertAsync(fakePatient);

            // Act
            var route = ApiRoutes.Patients.Delete.Replace(ApiRoutes.Patients.PatientId, fakePatient.PatientId.ToString());
            var result = await _client.DeleteRequestAsync(route);

            // Assert
            result.StatusCode.Should().Be(204);
        }
            
        [Test]
        public async Task Delete_Patient_Returns_Unauthorized_Without_Valid_Token()
        {
            // Arrange
            var fakePatient = new FakePatient { }.Generate();

            await InsertAsync(fakePatient);

            // Act
            var route = ApiRoutes.Patients.Delete.Replace(ApiRoutes.Patients.PatientId, fakePatient.PatientId.ToString());
            var result = await _client.DeleteRequestAsync(route);

            // Assert
            result.StatusCode.Should().Be(401);
        }
            
        [Test]
        public async Task Delete_Patient_Returns_Forbidden_Without_Proper_Scope()
        {
            // Arrange
            var fakePatient = new FakePatient { }.Generate();
            _client.AddAuth();

            await InsertAsync(fakePatient);

            // Act
            var route = ApiRoutes.Patients.Delete.Replace(ApiRoutes.Patients.PatientId, fakePatient.PatientId.ToString());
            var result = await _client.DeleteRequestAsync(route);

            // Assert
            result.StatusCode.Should().Be(403);
        }
    }
}