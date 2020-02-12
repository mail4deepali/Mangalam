using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace MMB.Mangalam.Web.Model
{
    [Table("candidate_language_map")]
    public class CandidateLanguageMap
    {
        public int id { get; set; }
        public int candidate_id { get; set; }
        public int language_id { get; set; }
       
    }
}
