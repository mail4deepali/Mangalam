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

            var response = userService.Authenticate("mladmin", "ML@Vkt0rY");

            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Data);
            Assert.NotNull(response.Data.token);
            Assert.True(response.Data.token.Length > 0);

            //no need to test the saved user
            //var savedUser = userService.Get(user.id);
            //Assert.Equal(savedUser.token, user.token);

        }

        [Fact]
        public void TestUpdatePassword()
        {
            ConnectionStringService connectionStringService
                = new ConnectionStringService(ConnectionString.Value);

            SecurityService securityService = new SecurityService();

            UserService userService = new UserService(connectionStringService, securityService,
                IOptionsHelper.Get());

            var response = userService.UpdatePassword("992296994", securityService.HashUserNameAndPassword("992296994", "Deeps@123"));

            Assert.True(response.IsSuccess);
        }

    }
}
