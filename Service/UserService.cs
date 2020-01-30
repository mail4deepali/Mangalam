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
using Model;

namespace MMB.Mangalam.Web.Service
{

    public class UserService : IUserService
    {
        ConnectionStringService? _ConnectionStringService = null;
        SecurityService? _SecurityService = null;

        private readonly AppSettings _appSettings;
     
        public UserService(ConnectionStringService connectionStringService, SecurityService securityService, IOptions<AppSettings> appSettings)
        {
            _ConnectionStringService = connectionStringService;
            _SecurityService = securityService;
            _appSettings = appSettings.Value;
        }

        public User Authenticate(string userName, string password)
        {
            string hashedPassword = _SecurityService.HashUserNameAndPassword(userName, password);
            User? user = null;
            using (IDbConnection connection = new NpgsqlConnection(_ConnectionStringService.Value))
            {
                user = connection.QueryFirstOrDefault<User>("Select * from user_table where user_name = @p0 and password = @p1", new { p0 = userName, p1 = hashedPassword });
            }

            if (user == null)
            {
                return null;
            }

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.token = tokenHandler.WriteToken(token);

            return user.WithoutPassword();

        }

        public IEnumerable<User> GetAll()
        {
            using (IDbConnection connection = new NpgsqlConnection(_ConnectionStringService.Value))
            {
                List<User> users = connection.QuerySingle<List<User>>("Select * from user_table ");
                return users.WithoutPasswords();
            }           
        }
    }
}
