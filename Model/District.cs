using System;
using System.Collections.Generic;
using System.Text;
namespace MMB.Mangalam.Web.Model
{
    class District
    {
        public int districtid { get; set; }
        public int stateid { get; set; }
        public string district_name { get; set; } = "";
        public string district_description { get; set; } = "";
    }
}
