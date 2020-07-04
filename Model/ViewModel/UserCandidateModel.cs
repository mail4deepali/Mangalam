using System;
using System.Collections.Generic;
using System.Text;

namespace MMB.Mangalam.Web.Model.ViewModel
{
    public class UserCandidateModel
    {
        public int otherPhotosCount { get; set; }
        public User user { get; set; }
        public CandidateDetails candidate { get; set; }
    }
}
