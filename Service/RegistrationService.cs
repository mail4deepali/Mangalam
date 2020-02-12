using MMB.Mangalam.Web.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using MMB.Mangalam.Web.Model.Helpers;
using Microsoft.Extensions.Options;
using FluentValidation;
using MMB.Mangalam.Web.Service.Constants;
using Dapper.Contrib.Extensions;

namespace MMB.Mangalam.Web.Service
{

    public class RegistrationService 
    {
        ConnectionStringService? _ConnectionStringService = null;
        SecurityService? _SecurityService = null;

        private readonly AppSettings _appSettings;
     
        public RegistrationService(ConnectionStringService connectionStringService, SecurityService securityService, IOptions<AppSettings> appSettings)
        {
            _ConnectionStringService = connectionStringService;
            _SecurityService = securityService;
            _appSettings = appSettings.Value;
        }

        public User RegisterNewCandidate(NewRegistrationViewModel newRegistrationModel)
        {
            string username = newRegistrationModel.phone_number.ToString();
            string password 
                = newRegistrationModel.first_name.Substring(0, 3) 
                + newRegistrationModel.last_name.Substring(0, 3) + (newRegistrationModel.phone_number /10000000).ToString();
            
            string hashedPassword = _SecurityService.HashUserNameAndPassword(username, password);
            
            User user = new User();
            Address userAddress = new Address();
            Address candidateAddress = new Address();
            Candidate candidate = new Candidate();

            MapCandidate(candidate, newRegistrationModel);
            MapUser(user, newRegistrationModel);
            MapUserAddress(userAddress, newRegistrationModel);
            MapCandidateAddress(candidateAddress, newRegistrationModel);


            user.user_name = username;
            user.password = hashedPassword;

            //add in table todo
            //user.roleid = UserRoleConstants.Candidate;

            using (IDbConnection dbConnection = new NpgsqlConnection(_ConnectionStringService.Value))
            {
                dbConnection.Open();

                using (var transaction = dbConnection.BeginTransaction())
                {

                    try
                    {
                        user.address_id = (Int32)dbConnection.Insert<Address>(userAddress, transaction);
                        candidate.address_id = (Int32)dbConnection.Insert<Address>(candidateAddress, transaction);

                        candidate.user_id = (Int32)dbConnection.Insert<User>(user, transaction);
                        dbConnection.Insert<Candidate>(candidate, transaction);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                
                return user;
                
            }

        }

        private void MapCandidate(Candidate candidate, NewRegistrationViewModel model)
        {
            candidate.first_name = model.candidate_first_name;
            candidate.last_name = model.candidate_last_name;
            candidate.phone_number = model.candidate_phone_number;
            candidate.gender_id = model.gender;
            candidate.religion_id = model.religion;
            candidate.caste_id = model.caste;
            candidate.education_id = model.education;
            candidate.family_type_id = model.familytype;

        }
       
        private void MapUser(User user, NewRegistrationViewModel newRegistrationModel)
        {
            user.first_name = newRegistrationModel.first_name;
            user.last_name = newRegistrationModel.last_name;         
            user.phone_number = newRegistrationModel.phone_number.ToString();
            
            
        }

        private void MapUserAddress(Address address, NewRegistrationViewModel newRegistrationModel)
        {
            address.address_line_1 = newRegistrationModel.address_line_1;
            address.address_line_2 = newRegistrationModel.address_line_2;
            address.taluka_id = newRegistrationModel.taluka;
            address.state_id = newRegistrationModel.state;
            address.district_id = newRegistrationModel.district;

            
        }

        private void MapCandidateAddress(Address address, NewRegistrationViewModel newRegistrationModel)
        {
            address.address_line_1 = newRegistrationModel.candidate_address_line_1;
            address.address_line_2 = newRegistrationModel.candidate_address_line_2;
            address.taluka_id = newRegistrationModel.candidate_taluka;
            address.state_id = newRegistrationModel.candidate_state;
            address.district_id = newRegistrationModel.candidate_district;

        }

        public FluentValidation.Results.ValidationResult ValidateForm(NewRegistrationViewModel candidateform)
        {
            var validator = new CandidateValidator();
            var result = validator.Validate(candidateform);      
            return result;
        }

    }
}
