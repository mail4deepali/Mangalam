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
    public class AddressHelper
    {
        public static void Assrt(Address expected, Address actual)
        {
            Assert.Equal(expected.address_line_1, actual.address_line_1);
            Assert.Equal(expected.address_line_2, actual.address_line_2);
            Assert.Equal(expected.district_id, actual.district_id);
            Assert.Equal(expected.state_id, actual.state_id);
            Assert.Equal(expected.taluka_id, actual.taluka_id);
            Assert.Equal(expected.zip_code, actual.zip_code);
          

        }

        public static Address Get(string address_line_1, string address_line_2, Int16 district_id, Int16 state_id, 
            Int16 taluka_id, Int16 zip_code)
        {
            Address address = new Address();
            address.address_line_1 = address_line_1;
            address.address_line_2 = address_line_2;
            address.district_id = district_id;
            address.state_id = state_id;
            address.taluka_id = taluka_id;
            address.zip_code = zip_code;

            return address;
        }
    }
}
