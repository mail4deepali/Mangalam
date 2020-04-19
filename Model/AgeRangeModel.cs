using System;
using System.ComponentModel.DataAnnotations;

namespace MMB.Mangalam.Web.Model
{
    public class AgeRangeModel
    {
        [Required]
        public DateTime fromBirthdate { get; set; }

        [Required]
        public DateTime toBirthdate { get; set; }
        [Required]
        public int candidate_id { get; set; }

    }
    
}
