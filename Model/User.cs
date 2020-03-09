using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMB.Mangalam.Web.Model
{
    [Table("user_table")]
    public class User
    {
        public int id { get; set; }
        public string user_name { get; set; } = "";
        public string first_name { get; set; } = "";
        public string last_name { get; set; } = "";
        public string phone_number { get; set; } = "";
        public string? alternate_phone_number { get; set; }
        public string password { get; set; } = "";        
        public string token { get; set; } = "";
        public int address_id { get; set; }
        public int role_id { get; set; }
        public bool is_user_login_first_time { get; set; }
        public string confirm_password { get; set; }
    }
    
}
