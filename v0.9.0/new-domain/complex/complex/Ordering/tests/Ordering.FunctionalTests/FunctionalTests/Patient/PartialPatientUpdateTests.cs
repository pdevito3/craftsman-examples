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
        public async Task Patch_Patient_Returns_NoContent_WithAuth()
        {
            // Arrange
            var fakePatient = new FakePatient { }.Generate();
            var patchDoc = new JsonPatchDocument<PatientForUpdateDto>();
            patchDoc.Replace(p => p.ExternalId, "Easily Identified Value For Test");

            _client.AddAuth(new[] {"patients.update"});

            await InsertAsync(fakePatient);

            // Act
            var route = ApiRoutes.Patients.Patch.Replace(ApiRoutes.Patients.PatientId, fakePatient.PatientId.ToString());
            var result = await _client.PatchJsonRequestAsync(route, patchDoc);

            // Assert
            result.StatusCode.Should().Be(204);
        }
            
        [Test]
        public async Task Patch_Patient_Returns_Unauthorized_Without_Valid_Token()
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
            result.StatusCode.Should().Be(401);
        }
            
        [Test]
        public async Task Patch_Patient_Returns_Forbidden_Without_Proper_Scope()
        {
            // Arrange
            var fakePatient = new FakePatient { }.Generate();
            var patchDoc = new JsonPatchDocument<PatientForUpdateDto>();
            patchDoc.Replace(p => p.ExternalId, "Easily Identified Value For Test");
            _client.AddAuth();

            await InsertAsync(fakePatient);

            // Act
            var route = ApiRoutes.Patients.Patch.Replace(ApiRoutes.Patients.PatientId, fakePatient.PatientId.ToString());
            var result = await _client.PatchJsonRequestAsync(route, patchDoc);

            // Assert
            result.StatusCode.Should().Be(403);
        }
    }
}