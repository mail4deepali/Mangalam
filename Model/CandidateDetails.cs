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
    }
}
