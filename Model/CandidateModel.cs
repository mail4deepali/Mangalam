using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MMB.Mangalam.Web.Model
{
    public class Candidate
    {
        [Required]
        public string first_name { get; set; } 
        [Required]
        public string last_name { get; set; } 
       
         [Required]
         public int phone_number { get; set; }
         [Required]
         public string address_line_1 { get; set; }
         [Required]
         public string address_line_2 { get; set; } 
         [Required]
         public int taluka { get; set; }
         [Required]
         public int district { get; set; }
         [Required]
         public int state { get; set; } 

         [Required]
         public string candidate_first_name { get; set; }
         [Required]
         public string candidate_last_name { get; set; }
         [Required]
         public int candidate_phone_number { get; set; }
         [Required]
         public string candidate_address_line_1 { get; set; }
         [Required]
         public string candidate_address_line_2 { get; set; }
         [Required]
         public int candidate_taluka { get; set; }
         [Required]
         public int candidate_district { get; set; } 
         [Required]
         public int candidate_state { get; set; }
        [Required]
        public int gender { get; set; }
         [Required]
         public int religion { get; set; } 
         [Required]
         public int caste { get; set; }
         [Required]
         public int education { get; set; }
        [Required]
        public int[] language { get; set; } 
        [Required]
        public int familytype { get; set; }         
    }
}
