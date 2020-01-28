﻿using MMB.Mangalam.Web.Service;
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
            Assert.Equal("f78d959290c92c50bcce1a83dddaafbd7f5fe01b696a9e904c3fa37febea71d1", hashedPassword);

        }
    }
}
