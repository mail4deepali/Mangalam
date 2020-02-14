using MMB.Mangalam.Web.Model;
using MMB.Mangalam.Web.Service;
using MMB.Mangalam.Web.Service.Constants;
using MMB.Mangalam.Web.Test.Helpers;
using Npgsql;
using System;
using System.Data;
using Test.Helpers;
using Xunit;
using Dapper;

namespace MMB.Mangalam.Web.Test
{
    [Collection("Database collection")]
    public class RegistrationTests : IDisposable
    {
        DatabaseFixture fixture;

        const string RegistrationTestUser1 = "1234567890";
        const string RegistrationTestUser2 = "1234567891";

        ConnectionStringService connectionStringService;
        IDbConnection connectionForTest = null;

        public RegistrationTests(DatabaseFixture fixture)
        {
            this.connectionStringService = new ConnectionStringService(ConnectionString.Value);
            this.fixture = fixture;

            UserHelper.CleanByUserName(RegistrationTestUser1);
            UserHelper.CleanByUserName(RegistrationTestUser2);

            this.connectionForTest = new NpgsqlConnection(ConnectionString.Value);
        }
        
        [Fact]
        public void TestNewRegistration()
        {
            SecurityService securityService = new SecurityService();
            string realPassword = "firlas890";
            //tests the lower casing of the password as well
            User expectedUser = UserHelper.Get(null, "First_name", false, "Last_name", "1234567890", UserRoles.Candidate, "", "1234567890");
            expectedUser.password = securityService.HashUserNameAndPassword(expectedUser.phone_number.ToString(), realPassword);
             

            Candidate expectedCandidate
                = CandidateHelper.Get("cfn", "cln", 1234567891, Castes.Jain, HighestEducations.BE_BTECH,
                            FamilyTypes.Joint, Genders.Female, Religions.HINDU);

            Address expectedUserAddress = AddressHelper.Get("ual1", "ual2", Districts.NASHIK, States.GUJRAT, Talukas.SHIROL, 123);
            Address expectedCandidateAddress = AddressHelper.Get("cal1", "cal2", Districts.KOLHAPUR, States.GOA, Talukas.DAUND, 345);


            NewRegistrationViewModel newRegistration = new NewRegistrationViewModel();
            newRegistration.address_line_1 = expectedUserAddress.address_line_1;
            newRegistration.address_line_2 = expectedUserAddress.address_line_2;
            
            newRegistration.candidate_address_line_1 = expectedCandidateAddress.address_line_1;
            newRegistration.candidate_address_line_2 = expectedCandidateAddress.address_line_2;
            newRegistration.candidate_district_id = expectedCandidateAddress.district_id;
            newRegistration.candidate_first_name = expectedCandidate.first_name;
            newRegistration.candidate_last_name = expectedCandidate.last_name;
            newRegistration.candidate_phone_number = expectedCandidate.phone_number;
            newRegistration.candidate_state_id = expectedCandidateAddress.state_id;
            newRegistration.candidate_taluka_id = expectedCandidateAddress.taluka_id;
            
            newRegistration.caste_id = expectedCandidate.caste_id;
            newRegistration.district_id = expectedUserAddress.district_id;
            newRegistration.education_id = expectedCandidate.education_id;
            newRegistration.familytype_id = expectedCandidate.family_type_id;
            newRegistration.first_name = expectedUser.first_name;
            newRegistration.gender_id =expectedCandidate.gender_id;
            
            newRegistration.language = new int[] { Languages.Marathi, Languages.Hindi };
            newRegistration.last_name = expectedUser.last_name;
            newRegistration.phone_number = Convert.ToInt64(expectedUser.phone_number);
            newRegistration.religion_id = expectedCandidate.religion_id;
            newRegistration.state_id = expectedUserAddress.state_id;
            newRegistration.taluka_id = expectedUserAddress.taluka_id;


            newRegistration.zip_code = expectedUserAddress.zip_code;
            newRegistration.candidate_zip_code = expectedCandidateAddress.zip_code;


            RegistrationService registrationService = new RegistrationService(this.connectionStringService,
                securityService, IOptionsHelper.Get());

            var response = registrationService.RegisterNewCandidate(newRegistration);

            Assert.True(response.IsSuccess, "Registration Response Failed.");

            Assert.Equal("1234567890", response.Data.user_name);
            Assert.Equal(realPassword, response.Data.password);

            //User
            User actualUser = connectionForTest
                .QueryFirst<User>("Select * from user_table where user_name = @user_name", new { expectedUser.user_name });

            Assert.NotNull(actualUser);

            UserHelper.Assrt(expectedUser, actualUser);

            //UserAddress
            Address actualUserAddress = connectionForTest
                .QueryFirst<Address>("Select * from address where id = @address_id", new {actualUser.address_id});

            Assert.NotNull(actualUserAddress);

            AddressHelper.Assrt(expectedUserAddress, actualUserAddress);

            //Candidate
            Candidate actualCandidate = connectionForTest
                .QueryFirst<Candidate>("Select * from Candidate where user_id = @id", new { actualUser.id });

            Assert.NotNull(actualCandidate);

            CandidateHelper.Assrt(expectedCandidate, actualCandidate);

            //Candidate Address
            Address actualCandidateAddress = connectionForTest
               .QueryFirst<Address>("Select * from address where id = @address_id", new { actualCandidate.address_id });

            Assert.NotNull(actualCandidateAddress);

            AddressHelper.Assrt(expectedCandidateAddress, actualCandidateAddress);


        }

        [Fact]
        public void TestNewRegistrationPhoneNumberValidationFailed()
        {
            SecurityService securityService = new SecurityService();

            NewRegistrationViewModel newRegistration1 = NewRegistrationHelper.Get();

            newRegistration1.phone_number = Convert.ToInt64(RegistrationTestUser2);


            RegistrationService registrationService = new RegistrationService(this.connectionStringService,
                securityService, IOptionsHelper.Get());

            var response = registrationService.RegisterNewCandidate(newRegistration1);

            Assert.True(response.IsSuccess, "Registration should be successful.");

            response = registrationService.RegisterNewCandidate(newRegistration1);

            Assert.False(response.IsSuccess, "Registration should fail.");

            Assert.Equal("User phone number already exists", response.Message);
            


        }

        public void Dispose()
        {
            UserHelper.CleanByUserName(RegistrationTestUser1);
            UserHelper.CleanByUserName(RegistrationTestUser2);
            this.connectionForTest.Dispose();
        }
    }
}
