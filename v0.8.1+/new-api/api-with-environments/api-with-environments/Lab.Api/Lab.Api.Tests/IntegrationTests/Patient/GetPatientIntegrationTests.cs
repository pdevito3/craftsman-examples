
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
    using Infrastructure.Persistence.Contexts;
    using Microsoft.Extensions.DependencyInjection;
    using Application.Wrappers;

    [Collection("Sequential")]
    public class GetPatientIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public GetPatientIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task GetPatients_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<LabDbContext>();
                context.Database.EnsureCreated();

                //context.Patients.RemoveRange(context.Patients);
                context.Patients.AddRange(fakePatientOne, fakePatientTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync("api/Patients")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response<IEnumerable<PatientDto>>>(responseContent)?.Data;

            // Assert
            result.StatusCode.Should().Be(200);
            response.Should().ContainEquivalentOf(fakePatientOne, options =>
                options.ExcludingMissingMembers());
            response.Should().ContainEquivalentOf(fakePatientTwo, options =>
                options.ExcludingMissingMembers());
        }
        
        [Fact]
        public async Task GetPatient_ReturnsSuccessCodeAndResourceWithAccurateFields()
        {
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<LabDbContext>();
                context.Database.EnsureCreated();

                //context.Patients.RemoveRange(context.Patients);
                context.Patients.AddRange(fakePatientOne, fakePatientTwo);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var result = await client.GetAsync($"api/Patients/{fakePatientOne.PatientId}")
                .ConfigureAwait(false);
            var responseContent = await result.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var response = JsonConvert.DeserializeObject<Response<PatientDto>>(responseContent)?.Data;

            // Assert
            result.StatusCode.Should().Be(200);
            response.Should().BeEquivalentTo(fakePatientOne, options =>
                options.ExcludingMissingMembers());
        }
    } 
}