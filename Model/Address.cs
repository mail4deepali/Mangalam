using System;
using System.Collections.Generic;
using System.Text;

namespace MMB.Mangalam.Web.Model
{
   public class Address
    {
        public int id { get; set; }
        public string address_line_1 { get; set; } = "";
        public string address_line_2 { get; set; } = "";
        public string city { get; set; } = "";
        public int country_id { get; set; } 
        public int state_id { get; set; } 
        public int district_id { get; set; } 
        public int taluka_id { get; set; } 
    }
}
