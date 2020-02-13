using MMB.Mangalam.Web.Service;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using MMB.Mangalam.Web.Model.Helpers;
using Test.Helpers;

namespace MMB.Mangalam.Web.Test
{
    public class UserServiceTest
    {
        [Fact]
        public void TestAuthenticate()
        {
            ConnectionStringService connectionStringService
                = new ConnectionStringService(ConnectionString.Value);

            SecurityService securityService = new SecurityService();
           
            UserService userService = new UserService(connectionStringService, securityService, 
                IOptionsHelper.Get());

            var user = userService.Authenticate("mladmin", "ML@Vkt0rY");

            Assert.NotNull(user);
            Assert.NotNull(user.token);
            Assert.True(user.token.Length > 0);

            //no need to test the saved user
            //var savedUser = userService.Get(user.id);
            //Assert.Equal(savedUser.token, user.token);

        }
    }
}
