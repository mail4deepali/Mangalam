using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMB.Mangalam.Web.Model
{
    [Table("state")]
    public class State
    {
        public int id { get; set; }
        public int countryid { get; set; }
        public string state_name { get; set; } = "";
        public string state_description { get; set; } = "";
    }
}
