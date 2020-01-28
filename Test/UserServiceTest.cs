using MMB.Mangalam.Web.Service;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Test
{
    public class UserServiceTest
    {
        [Fact]
        public void TestAuthenticate()
        {
            ConnectionStringService connectionStringService 
                = new ConnectionStringService(ConnectionString.Value);

            SecurityService securityService = new SecurityService();

            UserService userService = new UserService(connectionStringService, securityService);

            var user = userService.Authenticate("mladmin", "ML@Vkt0rY");

            Assert.NotNull(user);

        }
    }
}
