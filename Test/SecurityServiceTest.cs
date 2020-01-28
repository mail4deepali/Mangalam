using MMB.Mangalam.Web.Service;
using System;
using Xunit;

namespace Test
{

    public class SecurityServiceTest
    {


        [Fact]
        public void TestHasPassword()
        {
            SecurityService securityService = new SecurityService();
            string hashedPassword = securityService.HashUserNameAndPassword("mladmin", "ML@Vkt0rY");
            var p = hashedPassword;
        }
    }
}
