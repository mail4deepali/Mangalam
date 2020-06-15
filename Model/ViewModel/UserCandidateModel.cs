using System;
using System.Collections.Generic;
using System.Text;

namespace MMB.Mangalam.Web.Model.ViewModel
{
    public class UserCandidateModel
    {
        public User user { get; set; }
        public List<CandidateDetails> candidateList {get; set;}
        public CandidateDetails selectedCandidate { get; set; }
    }
}
