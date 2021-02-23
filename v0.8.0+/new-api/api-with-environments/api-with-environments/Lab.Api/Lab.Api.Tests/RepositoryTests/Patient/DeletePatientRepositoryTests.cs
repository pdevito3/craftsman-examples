
namespace Lab.Api.Tests.RepositoryTests.Patient
{
    using Application.Dtos.Patient;
    using FluentAssertions;
    using Lab.Api.Tests.Fakes.Patient;
    using Infrastructure.Persistence.Contexts;
    using Infrastructure.Persistence.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Sieve.Models;
    using Sieve.Services;
    using System;
    using System.Linq;
    using Xunit;
    using Application.Interfaces;
    using Moq;

    public class DeletePatientRepositoryTests
    { 
        
        [Fact]
        public void DeletePatient_ReturnsProperCount()
        {
            //Arrange
            var dbOptions = new DbContextOptionsBuilder<LabDbContext>()
                .UseInMemoryDatabase(databaseName: $"PatientDb{Guid.NewGuid()}")
                .Options;
            var sieveOptions = Options.Create(new SieveOptions());

            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            var fakePatientThree = new FakePatient { }.Generate();

            //Act
            using (var context = new LabDbContext(dbOptions))
            {
                context.Patients.AddRange(fakePatientOne, fakePatientTwo, fakePatientThree);

                var service = new PatientRepository(context, new SieveProcessor(sieveOptions));
                service.DeletePatient(fakePatientTwo);

                context.SaveChanges();

                //Assert
                var patientList = context.Patients.ToList();

                patientList.Should()
                    .NotBeEmpty()
                    .And.HaveCount(2);

                patientList.Should().ContainEquivalentOf(fakePatientOne);
                patientList.Should().ContainEquivalentOf(fakePatientThree);
                Assert.DoesNotContain(patientList, p => p == fakePatientTwo);

                context.Database.EnsureDeleted();
            }
        }
    } 
}