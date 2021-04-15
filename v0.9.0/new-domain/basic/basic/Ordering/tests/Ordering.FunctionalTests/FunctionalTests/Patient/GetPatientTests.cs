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
        public async Task Get_Patient_Record_Returns_NoContent()
        {
            // Arrange
            var fakePatient = new FakePatient { }.Generate();

            await InsertAsync(fakePatient);

            // Act
            var route = ApiRoutes.Patients.GetRecord.Replace(ApiRoutes.Patients.PatientId, fakePatient.PatientId.ToString());
            var result = await _client.GetRequestAsync(route);

            // Assert
            result.StatusCode.Should().Be(200);
        }
    }
}