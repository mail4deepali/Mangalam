using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MMB.Mangalam.Web.Model
{
    public class CandidateModel
    {
        [Required]
        public string first_name { get; set; } = "";
        [Required]
        public string last_name { get; set; } = "";
        [Required]
        public int phone_number { get; set; }
        [Required]
        public string address_line_1 { get; set; } = "";
        [Required]
        public string address_line_2 { get; set; } = "";
        [Required]
        public string taluka { get; set; } = "";
        [Required]
        public string district { get; set; } = "";
        [Required]
        public string state { get; set; } = "";

        [Required]
        public string candidate_first_name { get; set; } = "";
        [Required]
        public string candidate_last_name { get; set; } = "";
        [Required]
        public int candidate_phone_number { get; set; }
        [Required]
        public string candidate_address_line_1 { get; set; } = "";
        [Required]
        public string candidate_address_line_2 { get; set; } = "";
        [Required]
        public string candidate_taluka { get; set; } = "";
        [Required]
        public string candidate_district { get; set; } = "";
        [Required]
        public string candidate_state { get; set; } = "";
        [Required]
        public string gender { get; set; } = "";
        [Required]
        public string religion { get; set; } = "";
        [Required]
        public string caste { get; set; } = "";
        [Required]
        public string education { get; set; } = "";
        [Required]
        public string[] language { get; set; } = {""};
        [Required]
        public string familytype { get; set; } = "";

    }
}
