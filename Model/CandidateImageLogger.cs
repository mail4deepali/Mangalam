using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MMB.Mangalam.Web.Model
{
    [Table("candidate_image_logger")]
    public class CandidateImageLogger
    {

        public int id { get; set; }
        public int user_id { get; set; }
        public int candidate_id { get; set; }
        public string image_name { get; set; } = "";
        public string image_path { get; set; } = "";
        public string content_type { get; set; } = "";
        public DateTime image_upload_time { get; set; } 
        public bool is_approved { get; set; }
        public bool is_profile_pic { get; set; }
        public bool is_from_other_three_photos { get; set; }
        public bool is_deleted { get; set; }
    }
}
