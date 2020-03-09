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
using MMB.Mangalam.Web.Model.ViewModel;

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

        public JsonResponse<User> Authenticate(string userName, string password)
        {
            JsonResponse<User> response = new JsonResponse<User>();

            string hashedPassword = _SecurityService.HashUserNameAndPassword(userName, password);
            User? user = null;
            using (IDbConnection connection = new NpgsqlConnection(_ConnectionStringService.Value))
            {
                user = connection.QueryFirstOrDefault<User>("Select * from user_table where user_name = @p0 and password = @p1", new { p0 = userName, p1 = hashedPassword });
            }

            if (user != null)
            {
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

                response.IsSuccess = true;
                response.Message = "User Authenticated";
                response.Data = user;
                //added this as saw in 1 post but seems not needed
                //using (IDbConnection connection = new NpgsqlConnection(_ConnectionStringService.Value))
                //{
                //    connection.Execute("Update user_table set token = @token where id = @user_id", new { user.token, user_id = user.id });
                //}
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Username or password Incorrect.";
            }

           

            return response;

        }

        public bool IsUserLoginFirstTime(string userName, string hashedPassword)
        {
            using (IDbConnection connection = new NpgsqlConnection(_ConnectionStringService.Value))
            {
                User user = connection.QuerySingle<User>("Select * from user_table where user_name = @p0 and password = @p1", new { p0 = userName, p1 = hashedPassword });
                return user.is_user_login_first_time;
            }
        }

        public bool UpdatePassword(int id, string password)
        {
            using (IDbConnection connection = new NpgsqlConnection(_ConnectionStringService.Value))
            {
               int value = connection.Execute("Update user_table set password = @p0 where id = @p1", new { id, password});
                return value > 0 ? true : false;
            }
            
        }

        public IEnumerable<User> GetAll()
        {
            using (IDbConnection connection = new NpgsqlConnection(_ConnectionStringService.Value))
            {
                List<User> users = connection.QuerySingle<List<User>>("Select * from user_table ");
                return users;
            }           
        }

        public User Get(int id)
        {
            using (IDbConnection connection = new NpgsqlConnection(_ConnectionStringService.Value))
            {
                User user = connection.QuerySingle<User>("Select * from user_table where id = @id", new { id });
                return user;
            }
        }
    }
}
