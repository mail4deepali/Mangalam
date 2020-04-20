using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MMB.Mangalam.Web.Model
{
   [Table("address")]
   public class Address
    {
        public int id { get; set; }
        public string address_line_1 { get; set; } = "";
        public string address_line_2 { get; set; } = "";
        
        public int? state_id { get; set; } 
        public int district_id { get; set; } 
        public int taluka_id { get; set; } 
        public int zip_code { get; set; }
    }
}
