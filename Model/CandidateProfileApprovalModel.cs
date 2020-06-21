using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MMB.Mangalam.Web.Model
{
    public class CandidateProfileApprovalModel
    {
        public List<CandidateImagaeModel> profileimages {get; set;}
    }


    public class CandidateImagaeModel
    {
        public int imageLoggedid { get; set; }
        public string image { get; set; }
        public User user { get; set; }
        public Candidate candidate { get; set; }
        public bool is_approved { get; set; }
        public bool is_profile { get; set; }
        public bool is_from_other_three_photos { get; set; }

    }
}
