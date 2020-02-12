using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMB.Mangalam.Web.Model
{
    [Table("religion")]
    class Religion
    {
        public int religionid { get; set; }
        public string religion_name { get; set; } = "";
        public string religion_description { get; set; } = "";
    }
}
