using System;
using System.Collections.Generic;
using System.Text;

namespace MMB.Mangalam.Web.Model
{
    public class CandidateDetails
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public DateTime date_of_birth { get; set; }
        public string gender { get; set; }  
        public string religion { get; set; }
        public String caste { get; set; }
        public string image { get; set; }
        public string age { get; set; } 
        public string occupation { get; set; }
        public long phone_number { get; set; }
        public string education { get; set; }
        public string family_type { get; set; }
        public string taluka { get; set; }
        public string district { get; set; }
        public string state { get; set; }
        public string marital_status { get; set; }
        public string[] languages { get; set; }
        public int gender_id { get; set; }
        public int religion_id { get; set; }
        public int? caste_id { get; set; }
        public int? education_id { get; set; }
        public int family_type_id { get; set; }
        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }
        public int taluka_id { get; set; }
        public int district_id { get; set; }
        public int state_id { get; set; }
        public int user_id { get; set; }
        public int marital_status_id { get; set; }
        public int[] language { get; set; }
        public string otheroccupation { get; set; }
        public int address_id { get; set; }
    }
}
