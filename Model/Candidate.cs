using System;
using System.Collections.Generic;
using System.Text;

namespace MMB.Mangalam.Web.Model
{
   public class Candidate
    {
        public string first_name { get; set; }      
        public string last_name { get; set; }
        public long phone_number { get; set; }      
        public string address_line_1 { get; set; }       
        public string address_line_2 { get; set; }        
        public int gender_id { get; set; }     
        public int religion_id { get; set; }       
        public int caste_id { get; set; }       
        public int education_id { get; set; }
        public int familytype_id { get; set; }
        public int address_id { get; set; }
       
    }
}
