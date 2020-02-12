using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMB.Mangalam.Web.Model
{
    [Table("caste")]
    public class Caste
    {
        public int id { get; set; }
        public string caste_name { get; set; } = "";
        public string caste_description { get; set; } = "";

    }
}
