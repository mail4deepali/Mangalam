using System;
using System.Collections.Generic;
using System.Text;

namespace MMB.Mangalam.Web.Model
{
    class Caste
    {
        public int casteid { get; set; }
        public string caste_name { get; set; } = "";
        public string caste_description { get; set; } = "";
        public int religionid { get; set; }
    }
}
