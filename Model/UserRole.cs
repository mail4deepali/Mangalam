using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMB.Mangalam.Web.Model
{
    [Table("user_role")]
    public class UserRole
    {
        
        public int roleid { get; set; }
    
        public string user_role { get; set; } = "";
         
        public string role_description { get; set; } = "";
    }
    
}
