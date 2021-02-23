
namespace Lab.Api.Tests.IntegrationTests.Patient
{
    using Application.Dtos.Patient;
    using FluentAssertions;
    using Lab.Api.Tests.Fakes.Patient;
    using Microsoft.AspNetCore.Mvc.Testing;
    using System.Threading.Tasks;
    using Xunit;
    using Newtonsoft.Json;
    using System.Net.Http;
    using WebApi;
    using System.Collections.Generic;
    using Application.Wrappers;

    [Collection("Sequential")]
    public class CreatePatientIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public CreatePatientIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task PostPatientReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var fakePatient = new FakePatientDto().Generate();

            // Act
            var httpResponse = await client.PostAsJsonAsync("api/Patients", fakePatient)
                .ConfigureAwait(false);

            // Assert
            httpResponse.EnsureSuccessStatusCode();

            var resultDto = JsonConvert.DeserializeObject<Response<PatientDto>>(await httpResponse.Content.ReadAsStringAsync()
                .ConfigureAwait(false));

            httpResponse.StatusCode.Should().Be(201);
            resultDto.Data.FirstName.Should().Be(fakePatient.FirstName);
            resultDto.Data.LastName.Should().Be(fakePatient.LastName);
            resultDto.Data.Sex.Should().Be(fakePatient.Sex);
            resultDto.Data.Gender.Should().Be(fakePatient.Gender);
            resultDto.Data.Race.Should().Be(fakePatient.Race);
            resultDto.Data.Ethnicity.Should().Be(fakePatient.Ethnicity);
        }
    } 
}