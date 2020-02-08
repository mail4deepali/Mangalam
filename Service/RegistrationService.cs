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

        public void RegisterNewCandidate(Candidate candidate)
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(_ConnectionStringService.Value))
            {
                dbConnection.Open();
                dbConnection.Execute("INSERT INTO candidate (first_name,last_name, phone_number,gender_id, caste_id, religion_id, education_id, family_type_id ) VALUES(@candidate_first_name,@candidate_last_name,@candidate_phone_number, @gender, @caste, @religion, @education,@familytype)", candidate);
                dbConnection.Execute("INSERT INTO address (address_line_1,address_line_2,country_id,state_id,district_id,taluka_id) VALUES(@candidate_address_line_1,@candidate_address_line_2,1,@candidate_state, @candidate_district, @candidate_taluka)", candidate);
            }

        }

        public string ValidateForm(Candidate candidateform)
        {
            string messsge ="";
            if (candidateform.first_name.Length < 2)
            {
                messsge += "firstname is too short";
            }
            if (candidateform.last_name.Length < 2)
            {
                messsge += "lastname is too short";
            }
            


            return null;
        }

    }
}
