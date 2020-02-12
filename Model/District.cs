using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMB.Mangalam.Web.Model
{
    [Table("district")]
    class District
    {
        public int districtid { get; set; }
        public int stateid { get; set; }
        public string district_name { get; set; } = "";
        public string district_description { get; set; } = "";
    }
}
