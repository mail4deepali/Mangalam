using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMB.Mangalam.Web.Model
{
    [Table("caste")]
    class Caste
    {
        public int casteid { get; set; }
        public string caste_name { get; set; } = "";
        public string caste_description { get; set; } = "";
        public int religionid { get; set; }
    }
}
