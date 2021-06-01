namespace Billing.FunctionalTests.FunctionalTests.Patient
{
    using Billing.SharedTestHelpers.Fakes.Patient;
    using Billing.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class CreatePatientTests : TestBase
    {
        [Test]
        public async Task Create_Patient_Returns_Created()
        {
            // Arrange
            var fakePatient = new FakePatientForCreationDto { }.Generate();

            // Act
            var route = ApiRoutes.Patients.Create;
            var result = await _client.PostJsonRequestAsync(route, fakePatient);

            // Assert
            result.StatusCode.Should().Be(201);
        }
    }
}