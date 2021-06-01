namespace Ordering.IntegrationTests.FeatureTests.Patient
{
    using Ordering.SharedTestHelpers.Fakes.Patient;
    using Ordering.IntegrationTests.TestUtilities;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ordering.WebApi.Features.Patients;
    using static TestFixture;

    public class PatientQueryTests : TestBase
    {
        [Test]
        public async Task PatientQuery_Returns_Resource_With_Accurate_Props()
        {
            // Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            await InsertAsync(fakePatientOne);

            // Act
            var query = new GetPatient.PatientQuery(fakePatientOne.PatientId);
            var patients = await SendAsync(query);

            // Assert
            patients.Should().BeEquivalentTo(fakePatientOne, options =>
                options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientQuery_Throws_KeyNotFoundException_When_Record_Does_Not_Exist()
        {
            // Arrange
            var badId = Guid.NewGuid();

            // Act
            var query = new GetPatient.PatientQuery(badId);
            Func<Task> act = () => SendAsync(query);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
    }
}