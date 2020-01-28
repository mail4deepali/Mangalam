using System;

namespace MMB.Mangalam.Web.Model
{
    public class User
    {
        public int id { get; set; }
        public string first_name { get; set; } = "";
        public string last_name { get; set; } = "";
        public string phone_number { get; set; } = "";
        public string? alternate_phone_number { get; set; }
        public string password { get; set; } = "";

        public string token { get; set; } = "";

    }
    
}
