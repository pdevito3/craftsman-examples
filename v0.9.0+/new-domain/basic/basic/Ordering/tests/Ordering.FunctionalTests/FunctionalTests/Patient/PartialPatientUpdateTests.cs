namespace Ordering.FunctionalTests.FunctionalTests.Patient
{
    using Ordering.SharedTestHelpers.Fakes.Patient;
    using Ordering.Core.Dtos.Patient;
    using Ordering.FunctionalTests.TestUtilities;
    using Microsoft.AspNetCore.JsonPatch;
    using FluentAssertions;
    using NUnit.Framework;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class PartialPatientUpdateTests : TestBase
    {
        [Test]
        public async Task Patch_Patient_Returns_NoContent()
        {
            // Arrange
            var fakePatient = new FakePatient { }.Generate();
            var patchDoc = new JsonPatchDocument<PatientForUpdateDto>();
            patchDoc.Replace(p => p.ExternalId, "Easily Identified Value For Test");

            await InsertAsync(fakePatient);

            // Act
            var route = ApiRoutes.Patients.Patch.Replace(ApiRoutes.Patients.PatientId, fakePatient.PatientId.ToString());
            var result = await _client.PatchJsonRequestAsync(route, patchDoc);

            // Assert
            result.StatusCode.Should().Be(204);
        }
    }
}