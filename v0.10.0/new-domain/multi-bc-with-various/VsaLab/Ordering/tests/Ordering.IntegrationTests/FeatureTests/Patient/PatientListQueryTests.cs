namespace Ordering.IntegrationTests.FeatureTests.Patient
{
    using Ordering.Core.Dtos.Patient;
    using Ordering.SharedTestHelpers.Fakes.Patient;
    using Ordering.Core.Exceptions;
    using Ordering.WebApi.Features.Patients;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using static TestFixture;

    public class PatientListQueryTests : TestBase
    {
        
        [Test]
        public async Task PatientListQuery_Returns_Resource_With_Accurate_Props()
        {
            // Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            var queryParameters = new PatientParametersDto();

            await InsertAsync(fakePatientOne, fakePatientTwo);

            // Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients.Should().HaveCount(2);
        }
        
        [Test]
        public async Task PatientListQuery_Returns_Expected_Page_Size_And_Number()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            var fakePatientThree = new FakePatient { }.Generate();
            var queryParameters = new PatientParametersDto() { PageSize = 1, PageNumber = 2 };

            await InsertAsync(fakePatientOne, fakePatientTwo, fakePatientThree);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients.Should().HaveCount(1);
        }
        
        [Test]
        public async Task PatientListQuery_Throws_ApiException_When_Null_Query_Parameters()
        {
            // Arrange
            // N/A

            // Act
            var query = new GetPatientList.PatientListQuery(null);
            Func<Task> act = () => SendAsync(query);

            // Assert
            act.Should().Throw<ApiException>();
        }
        
        [Test]
        public async Task PatientListQuery_Returns_Sorted_Patient_ExternalId_List_In_Asc_Order()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.ExternalId = "bravo";
            fakePatientTwo.ExternalId = "alpha";
            var queryParameters = new PatientParametersDto() { SortOrder = "ExternalId" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
            patients
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Returns_Sorted_Patient_ExternalId_List_In_Desc_Order()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.ExternalId = "bravo";
            fakePatientTwo.ExternalId = "alpha";
            var queryParameters = new PatientParametersDto() { SortOrder = "ExternalId" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
            patients
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Returns_Sorted_Patient_InternalId_List_In_Asc_Order()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.InternalId = "bravo";
            fakePatientTwo.InternalId = "alpha";
            var queryParameters = new PatientParametersDto() { SortOrder = "InternalId" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
            patients
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Returns_Sorted_Patient_InternalId_List_In_Desc_Order()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.InternalId = "bravo";
            fakePatientTwo.InternalId = "alpha";
            var queryParameters = new PatientParametersDto() { SortOrder = "InternalId" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
            patients
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Returns_Sorted_Patient_FirstName_List_In_Asc_Order()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.FirstName = "bravo";
            fakePatientTwo.FirstName = "alpha";
            var queryParameters = new PatientParametersDto() { SortOrder = "FirstName" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
            patients
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Returns_Sorted_Patient_FirstName_List_In_Desc_Order()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.FirstName = "bravo";
            fakePatientTwo.FirstName = "alpha";
            var queryParameters = new PatientParametersDto() { SortOrder = "FirstName" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
            patients
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Returns_Sorted_Patient_LastName_List_In_Asc_Order()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.LastName = "bravo";
            fakePatientTwo.LastName = "alpha";
            var queryParameters = new PatientParametersDto() { SortOrder = "LastName" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
            patients
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Returns_Sorted_Patient_LastName_List_In_Desc_Order()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.LastName = "bravo";
            fakePatientTwo.LastName = "alpha";
            var queryParameters = new PatientParametersDto() { SortOrder = "LastName" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
            patients
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Returns_Sorted_Patient_Dob_List_In_Asc_Order()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.Dob = DateTime.Now.AddDays(2);
            fakePatientTwo.Dob = DateTime.Now.AddDays(1);
            var queryParameters = new PatientParametersDto() { SortOrder = "Dob" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
            patients
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Returns_Sorted_Patient_Dob_List_In_Desc_Order()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.Dob = DateTime.Now.AddDays(2);
            fakePatientTwo.Dob = DateTime.Now.AddDays(1);
            var queryParameters = new PatientParametersDto() { SortOrder = "Dob" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
            patients
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Returns_Sorted_Patient_Sex_List_In_Asc_Order()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.Sex = "bravo";
            fakePatientTwo.Sex = "alpha";
            var queryParameters = new PatientParametersDto() { SortOrder = "Sex" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
            patients
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Returns_Sorted_Patient_Sex_List_In_Desc_Order()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.Sex = "bravo";
            fakePatientTwo.Sex = "alpha";
            var queryParameters = new PatientParametersDto() { SortOrder = "Sex" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
            patients
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Returns_Sorted_Patient_Gender_List_In_Asc_Order()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.Gender = "bravo";
            fakePatientTwo.Gender = "alpha";
            var queryParameters = new PatientParametersDto() { SortOrder = "Gender" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
            patients
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Returns_Sorted_Patient_Gender_List_In_Desc_Order()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.Gender = "bravo";
            fakePatientTwo.Gender = "alpha";
            var queryParameters = new PatientParametersDto() { SortOrder = "Gender" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
            patients
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Returns_Sorted_Patient_Race_List_In_Asc_Order()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.Race = "bravo";
            fakePatientTwo.Race = "alpha";
            var queryParameters = new PatientParametersDto() { SortOrder = "Race" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
            patients
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Returns_Sorted_Patient_Race_List_In_Desc_Order()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.Race = "bravo";
            fakePatientTwo.Race = "alpha";
            var queryParameters = new PatientParametersDto() { SortOrder = "Race" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
            patients
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Returns_Sorted_Patient_Ethnicity_List_In_Asc_Order()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.Ethnicity = "bravo";
            fakePatientTwo.Ethnicity = "alpha";
            var queryParameters = new PatientParametersDto() { SortOrder = "Ethnicity" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
            patients
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientOne, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Returns_Sorted_Patient_Ethnicity_List_In_Desc_Order()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.Ethnicity = "bravo";
            fakePatientTwo.Ethnicity = "alpha";
            var queryParameters = new PatientParametersDto() { SortOrder = "Ethnicity" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
            patients
                .Skip(1)
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientOne, options =>
                    options.ExcludingMissingMembers());
        }

        
        [Test]
        public async Task PatientListQuery_Filters_Patient_PatientId()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.PatientId = Guid.NewGuid();
            fakePatientTwo.PatientId = Guid.NewGuid();
            var queryParameters = new PatientParametersDto() { Filters = $"PatientId == {fakePatientTwo.PatientId}" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients.Should().HaveCount(1);
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Filters_Patient_ExternalId()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.ExternalId = "alpha";
            fakePatientTwo.ExternalId = "bravo";
            var queryParameters = new PatientParametersDto() { Filters = $"ExternalId == {fakePatientTwo.ExternalId}" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients.Should().HaveCount(1);
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Filters_Patient_InternalId()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.InternalId = "alpha";
            fakePatientTwo.InternalId = "bravo";
            var queryParameters = new PatientParametersDto() { Filters = $"InternalId == {fakePatientTwo.InternalId}" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients.Should().HaveCount(1);
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Filters_Patient_FirstName()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.FirstName = "alpha";
            fakePatientTwo.FirstName = "bravo";
            var queryParameters = new PatientParametersDto() { Filters = $"FirstName == {fakePatientTwo.FirstName}" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients.Should().HaveCount(1);
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Filters_Patient_LastName()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.LastName = "alpha";
            fakePatientTwo.LastName = "bravo";
            var queryParameters = new PatientParametersDto() { Filters = $"LastName == {fakePatientTwo.LastName}" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients.Should().HaveCount(1);
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Filters_Patient_Dob()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.Dob = DateTime.Now.AddDays(1);
            fakePatientTwo.Dob = DateTime.Parse(DateTime.Now.AddDays(2).ToString("MM/dd/yyyy"));
            var queryParameters = new PatientParametersDto() { Filters = $"Dob == {fakePatientTwo.Dob}" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients.Should().HaveCount(1);
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Filters_Patient_Sex()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.Sex = "alpha";
            fakePatientTwo.Sex = "bravo";
            var queryParameters = new PatientParametersDto() { Filters = $"Sex == {fakePatientTwo.Sex}" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients.Should().HaveCount(1);
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Filters_Patient_Gender()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.Gender = "alpha";
            fakePatientTwo.Gender = "bravo";
            var queryParameters = new PatientParametersDto() { Filters = $"Gender == {fakePatientTwo.Gender}" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients.Should().HaveCount(1);
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Filters_Patient_Race()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.Race = "alpha";
            fakePatientTwo.Race = "bravo";
            var queryParameters = new PatientParametersDto() { Filters = $"Race == {fakePatientTwo.Race}" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients.Should().HaveCount(1);
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
        }

        [Test]
        public async Task PatientListQuery_Filters_Patient_Ethnicity()
        {
            //Arrange
            var fakePatientOne = new FakePatient { }.Generate();
            var fakePatientTwo = new FakePatient { }.Generate();
            fakePatientOne.Ethnicity = "alpha";
            fakePatientTwo.Ethnicity = "bravo";
            var queryParameters = new PatientParametersDto() { Filters = $"Ethnicity == {fakePatientTwo.Ethnicity}" };

            await InsertAsync(fakePatientOne, fakePatientTwo);

            //Act
            var query = new GetPatientList.PatientListQuery(queryParameters);
            var patients = await SendAsync(query);

            // Assert
            patients.Should().HaveCount(1);
            patients
                .FirstOrDefault()
                .Should().BeEquivalentTo(fakePatientTwo, options =>
                    options.ExcludingMissingMembers());
        }

    }
}