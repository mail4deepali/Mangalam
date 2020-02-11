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
            string password = newRegistrationModel.first_name.Substring(0, 3) + newRegistrationModel.last_name.Substring(0, 3) + (newRegistrationModel.phone_number /10000000).ToString();
            string hashedPassword = _SecurityService.HashUserNameAndPassword(username, password);
            User user = new User();
            Address useraddress, candidateaddress;
            Candidate candidate = new Candidate();

            using (IDbConnection dbConnection = new NpgsqlConnection(_ConnectionStringService.Value))
            {
                dbConnection.Open();

                using (var transaction = dbConnection.BeginTransaction())
                {

                    candidate = MapCandidate(candidate, newRegistrationModel);
                    user = MapUser(user, newRegistrationModel);
                    user.user_name = username;
                    user.password = hashedPassword;

                    if (newRegistrationModel.isCandidateAddressSameAsUserAddress)
                    {
                        useraddress = dbConnection.QueryFirstOrDefault<Address>("Select * from address where address_line_1 = @address_line_1 and address_line_2 = @address_line_2 and country_id = 1 and state_id = @state and district_id = @district and taluka_id = @taluka  ", newRegistrationModel);
                        if (useraddress == null)
                        {
                            dbConnection.Execute("INSERT INTO address (address_line_1,address_line_2,country_id,state_id,district_id,taluka_id) VALUES(@address_line_1,@address_line_2,1,@state, @district, @taluka)", newRegistrationModel);
                            useraddress = dbConnection.QueryFirstOrDefault<Address>("Select * from address where address_line_1 = @address_line_1 and address_line_2 = @address_line_2 and country_id = 1 and state_id = @state and district_id = @district and taluka_id = @taluka  ", newRegistrationModel);
                        }

                        user.address_id = useraddress.id;
                        candidate.address_id = useraddress.id;
                    }
                    else
                    {
                        useraddress = dbConnection.QueryFirstOrDefault<Address>("Select * from address where address_line_1 = @address_line_1 and address_line_2 = @address_line_2 and country_id = 1 and state_id = @state and district_id = @district and taluka_id = @taluka  ", newRegistrationModel);
                        if (useraddress == null)
                        {
                            dbConnection.Execute("INSERT INTO address (address_line_1,address_line_2,country_id,state_id,district_id,taluka_id) VALUES(@address_line_1,@address_line_2,1,@state, @district, @taluka)", newRegistrationModel);
                            useraddress = dbConnection.QueryFirstOrDefault<Address>("Select * from address where address_line_1 = @address_line_1 and address_line_2 = @address_line_2 and country_id = 1 and state_id = @state and district_id = @district and taluka_id = @taluka  ", newRegistrationModel);
                        }
                        user.address_id = useraddress.id;

                        candidateaddress = dbConnection.QueryFirstOrDefault<Address>("Select * from address where address_line_1 = @address_line_1 and address_line_2 = @address_line_2 and country_id = 1 and state_id = @state and district_id = @district and taluka_id = @taluka  ", newRegistrationModel);
                        if (candidateaddress == null)
                        {
                            dbConnection.Execute("INSERT INTO address (address_line_1,address_line_2,country_id,state_id,district_id,taluka_id) VALUES(@candidate_address_line_1,@candidate_address_line_2,1,@candidate_state, @candidate_district, @candidate_taluka)", newRegistrationModel);
                            candidateaddress = dbConnection.QueryFirstOrDefault<Address>("Select * from address where address_line_1 = @address_line_1 and address_line_2 = @address_line_2 and country_id = 1 and state_id = @state and district_id = @district and taluka_id = @taluka  ", newRegistrationModel);
                        }
                        candidate.address_id = candidateaddress.id;


                    }
                    
                    dbConnection.Execute("INSERT INTO user_table (first_name,last_name, phone_number,password, user_name , address_id) VALUES(@first_name,@last_name,@phone_number,@password,@user_name, @address_id)", user);

                    dbConnection.Execute("INSERT INTO candidate (first_name,last_name, phone_number,gender_id, caste_id, religion_id, education_id, family_type_id , address_id) VALUES(@first_name,@last_name,@phone_number, @gender_id, @caste_id, @religion_id, @education_id,@familytype_id, @address_id)", candidate);

                    transaction.Commit();
                }
                user.password = password;
                return user;
                
            }

        }

        private Candidate MapCandidate(Candidate candidate, NewRegistrationViewModel model)
        {
            candidate.first_name = model.first_name;
            candidate.last_name = model.last_name;
            candidate.phone_number = model.phone_number;
            candidate.gender_id = model.gender;
            candidate.religion_id = model.religion;
            candidate.caste_id = model.caste;
            candidate.education_id = model.education;
            candidate.familytype_id = model.familytype;

            return candidate;
        }
       
        private User MapUser(User user, NewRegistrationViewModel newRegistrationModel)
        {
            user.first_name = newRegistrationModel.first_name;
            user.last_name = newRegistrationModel.last_name;         
            user.phone_number = newRegistrationModel.phone_number.ToString();
            user.roleid = UserRoleConstants.Candidate;
            return user;
        }

        public FluentValidation.Results.ValidationResult ValidateForm(NewRegistrationViewModel candidateform)
        {
            var validator = new CandidateValidator();
            var result = validator.Validate(candidateform);      
            return result;
        }

    }
}
