using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MMB.Mangalam.Web.Model
{
    public class NewRegistrationViewModel
    {
        [Required]
        public string first_name { get; set; } 
        [Required]
        public string last_name { get; set; } 
       
         [Required]
         public long phone_number { get; set; }

        [Required]
        public DateTime user_birth_date { get; set; }


         [Required]
         public string address_line_1 { get; set; }
         [Required]
         public string address_line_2 { get; set; } 
         [Required]
         public int taluka_id { get; set; }
         [Required]
         public int district_id { get; set; }
         [Required]
         public int state_id { get; set; } 
       
         [Required]
         public string candidate_first_name { get; set; }
         [Required]
         public string candidate_last_name { get; set; }
         [Required]
         public long candidate_phone_number { get; set; }
        [Required]
        public DateTime candidate_birth_date { get; set; }
        [Required]
         public string candidate_address_line_1 { get; set; }
         [Required]
         public string candidate_address_line_2 { get; set; }
         [Required]
         public int candidate_taluka_id { get; set; }
         [Required]
         public int candidate_district_id { get; set; } 
         [Required]
         public int candidate_state_id { get; set; }
        [Required]
        public int gender_id { get; set; }
         [Required]
         public int religion_id { get; set; } 
         [Required]
         public int caste_id { get; set; }
         [Required]
         public int education_id { get; set; }
        [Required]
        public int[] language { get; set; } 
        [Required]
        public int familytype_id { get; set; }         

        public int zip_code { get; set; }
        public int candidate_zip_code { get; set; }
    }
}
