using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMB.Mangalam.Web.Model
{
    [Table("state")]
    class State
    {
        public int stateid { get; set; }
        public int countryid { get; set; }
        public string state_name { get; set; } = "";
        public string state_description { get; set; } = "";
    }
}
