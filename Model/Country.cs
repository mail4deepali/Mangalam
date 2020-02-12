using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMB.Mangalam.Web.Model
{
    [Table("country")]
    public class Country
    {
        public int id { get; set; }
        public string country_name { get; set; } = "";
        public string country_code { get; set; } = "";
    }
}
