﻿using MMB.Mangalam.Web.Model;
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
using MMB.Mangalam.Web.Service.Helper;

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

        public AuthenticateModel RegisterNewCandidate(NewRegistrationViewModel newRegistrationModel)
        {
            const int passwordGenerationIndex = 3;
            string username = newRegistrationModel.phone_number.ToString();
            string password = GetPassword(newRegistrationModel.first_name, newRegistrationModel.last_name, newRegistrationModel.phone_number, passwordGenerationIndex);
            string hashedPassword = _SecurityService.HashUserNameAndPassword(username, password);
            AuthenticateModel infoModel = new AuthenticateModel();
            infoModel.user_name = username;
            infoModel.password = password;

            User user = new User();
            Address userAddress = new Address();
            Address candidateAddress = new Address();
            Candidate candidate = new Candidate();
            CandidateLanguageMap candidateLanguageMap;

            MapCandidate(candidate, newRegistrationModel);
            MapUser(user, newRegistrationModel);
            MapUserAddress(userAddress, newRegistrationModel);
            MapCandidateAddress(candidateAddress, newRegistrationModel);

            user.user_name = username;
            user.password = hashedPassword;
            user.role_id = UserRoleConstants.Candidate;

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
                        Int16 candidate_id = (Int16)dbConnection.Insert<Candidate>(candidate, transaction);

                        for (int i = 0; i < newRegistrationModel.language.Length; i++)
                        {
                            candidateLanguageMap = new CandidateLanguageMap();
                            candidateLanguageMap.candidate_id = candidate_id;
                            candidateLanguageMap.language_id = newRegistrationModel.language[i];
                            dbConnection.Insert<CandidateLanguageMap>(candidateLanguageMap, transaction);
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                
                return infoModel;
                
            }

        }

        private string GetPassword(string first_name, string last_name, long phone_number, int index )
        {
            string password = first_name.Substring(0, index) + last_name.Substring(0, index) + ExtensionMethods.GetLastThreeDigit(phone_number, index);
            return password;
        }

      
        private void MapCandidate(Candidate candidate, NewRegistrationViewModel model)
        {
            candidate.first_name = model.candidate_first_name;
            candidate.last_name = model.candidate_last_name;
            candidate.phone_number = model.candidate_phone_number;
            candidate.gender_id = model.gender_id;
            candidate.religion_id = model.religion_id;
            candidate.caste_id = model.caste_id;
            candidate.education_id = model.education_id;
            candidate.family_type_id = model.familytype_id;

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
            address.taluka_id = newRegistrationModel.taluka_id;
            address.state_id = newRegistrationModel.state_id;
            address.district_id = newRegistrationModel.district_id;

            
        }

        private void MapCandidateAddress(Address address, NewRegistrationViewModel newRegistrationModel)
        {
            address.address_line_1 = newRegistrationModel.candidate_address_line_1;
            address.address_line_2 = newRegistrationModel.candidate_address_line_2;
            address.taluka_id = newRegistrationModel.candidate_taluka_id;
            address.state_id = newRegistrationModel.candidate_state_id;
            address.district_id = newRegistrationModel.candidate_district_id;

        }

        public FluentValidation.Results.ValidationResult ValidateForm(NewRegistrationViewModel candidateform)
        {
            var validator = new NewRegistrationValidator();
            var result = validator.Validate(candidateform);      
            return result;
        }

    }
}
