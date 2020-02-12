using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMB.Mangalam.Web.Model
{
    [Table("country")]
    class Country
    {
        public int countryid { get; set; }
        public string country_name { get; set; } = "";
        public string country_code { get; set; } = "";
    }
}
