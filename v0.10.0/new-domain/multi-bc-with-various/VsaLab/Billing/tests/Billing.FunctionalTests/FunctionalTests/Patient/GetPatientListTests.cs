namespace Billing.FunctionalTests.FunctionalTests.Patient
{
    using Billing.SharedTestHelpers.Fakes.Patient;
    using Billing.FunctionalTests.TestUtilities;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class GetPatientListTests : TestBase
    {
        [Test]
        public async Task Get_Patient_List_Returns_NoContent()
        {
            // Arrange
            // N/A

            // Act
            var result = await _client.GetRequestAsync(ApiRoutes.Patients.GetList);

            // Assert
            result.StatusCode.Should().Be(200);
        }
    }
}