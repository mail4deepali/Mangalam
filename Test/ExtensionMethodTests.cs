using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using MMB.Mangalam.Web.Service.Helper;

namespace MMB.Mangalam.Web.Test
{
    public class ExtensionMethodTests
    {
        [Fact]
        public void TestGetLastNDigits()
        {
            Assert.Equal("890", "1234567890".LastNChars(3));
            Assert.Equal("7890", "1234567890".LastNChars(4));
            Assert.Equal("90", "90".LastNChars(3));
            Assert.Equal("1234567890", "1234567890".LastNChars(10));
            Assert.Equal("1234567890", "1234567890".LastNChars(11));
        }
    }
}
