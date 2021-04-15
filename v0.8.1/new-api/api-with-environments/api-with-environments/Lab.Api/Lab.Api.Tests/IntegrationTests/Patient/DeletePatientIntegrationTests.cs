
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
    using Microsoft.AspNetCore.JsonPatch;
    using System.Linq;
    using AutoMapper;
    using Bogus;
    using Application.Mappings;
    using System.Text;
    using Application.Wrappers;

    [Collection("Sequential")]
    public class DeletePatientIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    { 
        private readonly CustomWebApplicationFactory _factory;

        public DeletePatientIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        
        [Fact]
        public async Task DeletePatient204AndFieldsWereSuccessfullyUpdated()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();

            var appFactory = _factory;
            using (var scope = appFactory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<LabDbContext> ();
                context.Database.EnsureCreated();

                context.Patients.RemoveRange(context.Patients);
                context.Patients.AddRange(fakePatientOne);
                context.SaveChanges();
            }

            var client = appFactory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            // Act
            // get the value i want to update. assumes I can use sieve for this field. if this is not an option, just use something else
            var getResult = await client.GetAsync($"api/Patients/?filters=FirstName=={fakePatientOne.FirstName}")
                .ConfigureAwait(false);
            var getResponseContent = await getResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var getResponse = JsonConvert.DeserializeObject<Response<IEnumerable<PatientDto>>>(getResponseContent);
            var id = getResponse.Data.FirstOrDefault().PatientId;

            // delete it
            var method = new HttpMethod("DELETE");
            var deleteRequest = new HttpRequestMessage(method, $"api/Patients/{id}");
            var deleteResult = await client.SendAsync(deleteRequest)
                .ConfigureAwait(false);

            // get it again to confirm updates
            var checkResult = await client.GetAsync($"api/Patients/{id}")
                .ConfigureAwait(false);
            var checkResponseContent = await checkResult.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            var checkResponse = JsonConvert.DeserializeObject<Response<PatientDto>>(checkResponseContent);

            // Assert
            deleteResult.StatusCode.Should().Be(204);
            checkResponse.Data.Should().Be(null);
        }
    } 
}