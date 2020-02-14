using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using Xunit;
using MMB.Mangalam.Web.Model;

namespace MMB.Mangalam.Web.Test.Helpers
{
    public static class UserHelper
    {
        public static void  CleanByUserName(string user_name)
        {
            string sql = @"delete from candidate_language_map where candidate_id in (
            select id from candidate where user_id = (Select id from user_table where user_name = @user_name)
	            );
	
	            delete from candidate where user_id = (Select id from user_table where user_name = @user_name);
	            delete from user_table where user_name = @user_name;";

            using (IDbConnection connection = new NpgsqlConnection(ConnectionString.Value))
            {
                connection.Execute(sql, new { user_name });
            }

        }

        public static void Assrt(User expected, User actual)
        {
            Assert.Equal(expected.alternate_phone_number, actual.alternate_phone_number);
            Assert.Equal(expected.first_name, actual.first_name);
            Assert.Equal(expected.IsUserloginAfterRegistration, actual.IsUserloginAfterRegistration);
            Assert.Equal(expected.last_name, actual.last_name);
            Assert.Equal(expected.password, actual.password);
            Assert.Equal(expected.phone_number, actual.phone_number);
            Assert.Equal(expected.role_id, actual.role_id);
            Assert.Equal(expected.token, actual.token);
            Assert.Equal(expected.user_name, actual.user_name);

        }

        public static User Get(string alternate_phone_number, string first_name, bool IsUserloginAfterRegistration,
            string last_name, string phone_number, Int16 role_id, string token, string user_name)
        {
            User user = new User();
            user.alternate_phone_number = alternate_phone_number;
            user.first_name = first_name;
            user.IsUserloginAfterRegistration = IsUserloginAfterRegistration;
            user.last_name = last_name;
            user.phone_number = phone_number;
            user.role_id = role_id;
            user.token = token;
            user.user_name = user_name;

            return user;
        }
    }
}
