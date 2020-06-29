using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MMB.Mangalam.Web.Model
{
    [Table("candidate")]
    public class Candidate
    {
        public int id { get; set; }
        public string first_name { get; set; }      
        public string last_name { get; set; }
        public long phone_number { get; set; }      
        public DateTime date_of_birth { get; set; }
        public int gender_id { get; set; }     
        public int religion_id { get; set; }       
        public int? caste_id { get; set; }       
        public int? education_id { get; set; }
        public int family_type_id { get; set; }
        public int address_id { get; set; }
        public int user_id { get; set; }
        public int marital_status_id { get; set; }
    }
}
