using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MMB.Mangalam.Web.Model
{
    public class EditCandidateViewModel
    {
         public int candidate_id { get; set; }
         public int user_id { get; set; }

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
         
         public int? candidate_state_id { get; set; }
        [Required]
        public int gender_id { get; set; }
         [Required]
         public int religion_id { get; set; } 
         public int? caste_id { get; set; }
         public int? education_id { get; set; }
        [Required]
        public int[] language { get; set; } 
        [Required]
        public int familytype_id { get; set; }         

        public int zip_code { get; set; }
        public int candidate_zip_code { get; set; }

        public int marital_status_id { get; set; }

        public string occupation { get; set; }
        public string otheroccupation { get; set; }
        public int address_id { get; set; }
    }
}
