using MMB.Mangalam.Web.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;

namespace MMB.Mangalam.Web.Service
{
    public class UserService
    {
        ConnectionStringService? _ConnectionStringService = null;
        SecurityService? _SecurityService = null;
        public UserService(ConnectionStringService connectionStringService, SecurityService securityService)
        {
            _ConnectionStringService = connectionStringService;
            _SecurityService = securityService;
        }

        public User Authenticate(string userName, string password)
        {
            string hashedPassword = _SecurityService.HashUserNameAndPassword(userName, password);
            User? user = null;
            using (IDbConnection connection = new NpgsqlConnection(_ConnectionStringService.Value))
            {
                user = connection.QuerySingle<User>("Select * from user_table where user_name = @p0 and password = @p1", new { p0 = userName, p1 = hashedPassword });
            }

            return user;
        }
    }
}
