using System;
using System.ComponentModel.DataAnnotations;

namespace MMB.Mangalam.Web.Model
{
    public class AuthenticateModel
    {
        [Required]
        public string user_name { get; set; }

        [Required]
        public string password { get; set; }
    }
    
}
