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
using MMB.Mangalam.Web.Service.Helper;
using MMB.Mangalam.Web.Model.ViewModel;

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

        public JsonResponse<AuthenticateModel> RegisterNewCandidate(NewRegistrationViewModel newRegistrationModel)
        {
            JsonResponse<AuthenticateModel> jsonResponse = new JsonResponse<AuthenticateModel>();

            var result = ValidateRegistration(newRegistrationModel);
            if(result.IsValid)
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
                user.role_id = UserRoles.Candidate;
                user.is_user_login_first_time = true;
                user.date_of_birth = newRegistrationModel.user_birth_date;
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

                            jsonResponse.Data = infoModel;
                            jsonResponse.IsSuccess = true;
                            jsonResponse.Message = "Registration Successful";

                        }
                        catch (Exception ex)
                        {
                            if(ex.Message.Contains("user_table_phone_number_unique"))
                            {
                                jsonResponse.Message = "User phone number already exists";
                            }
                            else
                            {
                                jsonResponse.Message = "Some error occured. Please contact administrator.";
                            }

                            transaction.Rollback();
                            jsonResponse.IsSuccess = false;
                            
                        }
                    }
                }
            }
            else
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.Message = result.Messages();
            }

            return jsonResponse;

        }

        private string GetPassword(string first_name, string last_name, long phone_number, int index )
        {
            string password = first_name.Substring(0, index).ToLower() + last_name.Substring(0, index).ToLower() + phone_number.ToString().LastNChars(index);
            return password;
        }

      
        private void MapCandidate(Candidate candidate, NewRegistrationViewModel model)
        {
            candidate.first_name = model.candidate_first_name;
            candidate.last_name = model.candidate_last_name;
            candidate.phone_number = model.candidate_phone_number;
            candidate.gender_id = model.gender_id;
            candidate.religion_id = model.religion_id;
            candidate.marital_status_id = model.marital_status_id;
            if (model.occupation != null && model.occupation != "" && model.occupation == "Other")
            {
                candidate.occupation = model.otheroccupation;
            }
            else if (model.occupation != null && model.occupation != "" && model.occupation != "Other")
            {
                candidate.occupation = model.occupation;
            }

            if (model.caste_id != null)
            {
                candidate.caste_id = (int)model.caste_id;
            }
            else
            {
                candidate.caste_id = null;
            }
            if (model.education_id != null)
            {
                candidate.education_id = (int)model.education_id;
            }
            else 
            {
                candidate.education_id = null;
            }
            candidate.family_type_id = model.familytype_id;
            candidate.date_of_birth = model.candidate_birth_date;
        }
       
      
        private void MapEditCandidate(Candidate candidate, EditCandidateViewModel model)
        {
            candidate.user_id = model.user_id;
            candidate.id = model.candidate_id;
            candidate.address_id = model.address_id;
            candidate.first_name = model.candidate_first_name;
            candidate.last_name = model.candidate_last_name;
            candidate.phone_number = model.candidate_phone_number;
            candidate.gender_id = model.gender_id;
            candidate.religion_id = model.religion_id;
            candidate.marital_status_id = model.marital_status_id;
            if (model.occupation != null && model.occupation != "" && model.occupation == "Other")
            {
                candidate.occupation = model.otheroccupation;
            }
            else if (model.occupation != null && model.occupation != "" && model.occupation != "Other")
            {
                candidate.occupation = model.occupation;
            }

            if (model.caste_id != null)
            {
                candidate.caste_id = (int)model.caste_id;
            }
            else
            {
                candidate.caste_id = null;
            }
            if (model.education_id != null)
            {
                candidate.education_id = (int)model.education_id;
            }
            else 
            {
                candidate.education_id = null;
            }
            candidate.family_type_id = model.familytype_id;
            candidate.date_of_birth = model.candidate_birth_date;
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
            if (newRegistrationModel.state_id != null)
            {
                address.state_id = (int)newRegistrationModel.state_id;
            }
            else
            {
                address.state_id = null;
            }
            address.district_id = newRegistrationModel.district_id;
            address.zip_code = newRegistrationModel.zip_code;
            
        }

        private void MapCandidateAddress(Address address, NewRegistrationViewModel newRegistrationModel)
        {
            address.address_line_1 = newRegistrationModel.candidate_address_line_1;
            address.address_line_2 = newRegistrationModel.candidate_address_line_2;
            address.taluka_id = newRegistrationModel.candidate_taluka_id;
            if (newRegistrationModel.candidate_state_id != null)
            {
                address.state_id = (int)newRegistrationModel.candidate_state_id;
            }
            else {
                address.state_id = null;
            }
            address.district_id = newRegistrationModel.candidate_district_id;
            address.zip_code = newRegistrationModel.candidate_zip_code;
        }
        
        private void MapEditCandidateAddress(Address address, EditCandidateViewModel editCandidateViewModel)
        {
            address.address_line_1 = editCandidateViewModel.candidate_address_line_1;
            address.address_line_2 = editCandidateViewModel.candidate_address_line_2;
            address.taluka_id = editCandidateViewModel.candidate_taluka_id;
            if (editCandidateViewModel.candidate_state_id != null)
            {
                address.state_id = (int)editCandidateViewModel.candidate_state_id;
            }
            else {
                address.state_id = null;
            }
            address.district_id = editCandidateViewModel.candidate_district_id;
            address.zip_code = editCandidateViewModel.candidate_zip_code;
            address.id = editCandidateViewModel.address_id;
        }

        private FluentValidation.Results.ValidationResult ValidateRegistration(NewRegistrationViewModel candidateform)
        {
            var validator = new NewRegistrationValidator();
            var result = validator.Validate(candidateform);      
            return result;
        }

        private FluentValidation.Results.ValidationResult ValidateEditCandidate(EditCandidateViewModel candidateform)
        {
            var validator = new EditCandidateValidator();
            var result = validator.Validate(candidateform);      
            return result;
        }


        public JsonResponse<String> Editcandidate(EditCandidateViewModel editCandidateViewModel)
        {
            JsonResponse<String> jsonResponse = new JsonResponse<String>();

            var result = ValidateEditCandidate(editCandidateViewModel);
            if(result.IsValid)
            {
               
                Address candidateAddress = new Address();
                Candidate candidate = new Candidate();
                CandidateLanguageMap candidateLanguageMap;


                MapEditCandidate(candidate, editCandidateViewModel);
                MapEditCandidateAddress(candidateAddress, editCandidateViewModel);

                using (IDbConnection dbConnection = new NpgsqlConnection(_ConnectionStringService.Value))
                {
                    dbConnection.Open();

                    using (var transaction = dbConnection.BeginTransaction())
                    {

                        try
                        {
                               dbConnection.Update<Address>(candidateAddress, transaction);

                               dbConnection.Update<Candidate>(candidate, transaction);

                               dbConnection.Query("delete from candidate_language_map where candidate_id = @p0", new { p0 = candidate.id});

                            for (int i = 0; i < editCandidateViewModel.language.Length; i++)
                            {
                                candidateLanguageMap = new CandidateLanguageMap();
                                candidateLanguageMap.candidate_id = candidate.id;
                                candidateLanguageMap.language_id = editCandidateViewModel.language[i];
                                dbConnection.Insert<CandidateLanguageMap>(candidateLanguageMap, transaction);
                            }

                            transaction.Commit();

                            jsonResponse.Data = "Updated successfully";
                            jsonResponse.IsSuccess = true;
                            jsonResponse.Message = "Candidate Updated Successful";

                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.Contains("user_table_phone_number_unique"))
                            {
                                jsonResponse.Message = "User phone number already exists";
                            }
                            else
                            {
                                jsonResponse.Message = "Some error occured. Please contact administrator.";
                            }

                            transaction.Rollback();
                            jsonResponse.IsSuccess = false;

                        }
                    }
                }
            }
            else
            {
                jsonResponse.IsSuccess = false;
                jsonResponse.Message = result.Messages();
            }

            return jsonResponse;

        }

        
    }
}
