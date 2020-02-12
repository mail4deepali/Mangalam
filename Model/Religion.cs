using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMB.Mangalam.Web.Model
{
    [Table("religion")]
    public class Religion
    {
        public int id { get; set; }
        public string religion_name { get; set; } = "";
        public string religion_description { get; set; } = "";
    }
}
