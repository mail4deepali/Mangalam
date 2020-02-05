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

        public CandidateModel RegisterNewCandidate(string userName, string password)
        {
            return null;
        }

        public string ValidateForm(CandidateModel candidateform)
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
