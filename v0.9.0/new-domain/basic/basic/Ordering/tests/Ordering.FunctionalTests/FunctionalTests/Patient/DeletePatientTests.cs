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
        public async Task Delete_PatientReturns_NoContent()
        {
            // Arrange
            var fakePatient = new FakePatient { }.Generate();

            await InsertAsync(fakePatient);

            // Act
            var route = ApiRoutes.Patients.Delete.Replace(ApiRoutes.Patients.PatientId, fakePatient.PatientId.ToString());
            var result = await _client.DeleteRequestAsync(route);

            // Assert
            result.StatusCode.Should().Be(204);
        }
    }
}