using System;
using System.Collections.Generic;
using System.Text;

namespace MMB.Mangalam.Web.Model
{
    class State
    {
        public int stateid { get; set; }
        public int countryid { get; set; }
        public string state_name { get; set; } = "";
        public string state_description { get; set; } = "";
    }
}
