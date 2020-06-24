using System;
using System.ComponentModel.DataAnnotations;

namespace MMB.Mangalam.Web.Model
{
    public class AgeRangeModel
    {
        public int fromAge { get; set; }
        public int toAge { get; set; }
     
        public int? candidate_id { get; set; }

        public int? caste_id { get; set; }
        public int? state_id { get; set; }
        public int? education_id { get; set; }
        public int? gender_id { get; set; }

    }
    
}
