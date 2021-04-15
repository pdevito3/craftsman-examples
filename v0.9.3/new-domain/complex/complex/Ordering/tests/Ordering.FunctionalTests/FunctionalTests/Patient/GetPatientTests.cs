namespace Ordering.FunctionalTests.FunctionalTests.Patient
{
    using Ordering.SharedTestHelpers.Fakes.Patient;
    using Ordering.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class GetPatientTests : TestBase
    {
        [Test]
        public async Task Get_Patient_Record_Returns_NoContent_WithAuth()
        {
            // Arrange
            var fakePatient = new FakePatient { }.Generate();

            _client.AddAuth(new[] {"patients.read"});

            await InsertAsync(fakePatient);

            // Act
            var route = ApiRoutes.Patients.GetRecord.Replace(ApiRoutes.Patients.PatientId, fakePatient.PatientId.ToString());
            var result = await _client.GetRequestAsync(route);

            // Assert
            result.StatusCode.Should().Be(200);
        }
            
        [Test]
        public async Task Get_Patient_Record_Returns_Unauthorized_Without_Valid_Token()
        {
            // Arrange
            var fakePatient = new FakePatient { }.Generate();

            await InsertAsync(fakePatient);

            // Act
            var route = ApiRoutes.Patients.GetRecord.Replace(ApiRoutes.Patients.PatientId, fakePatient.PatientId.ToString());
            var result = await _client.GetRequestAsync(route);

            // Assert
            result.StatusCode.Should().Be(401);
        }
            
        [Test]
        public async Task Get_Patient_Record_Returns_Forbidden_Without_Proper_Scope()
        {
            // Arrange
            var fakePatient = new FakePatient { }.Generate();
            _client.AddAuth();

            await InsertAsync(fakePatient);

            // Act
            var route = ApiRoutes.Patients.GetRecord.Replace(ApiRoutes.Patients.PatientId, fakePatient.PatientId.ToString());
            var result = await _client.GetRequestAsync(route);

            // Assert
            result.StatusCode.Should().Be(403);
        }
    }
}