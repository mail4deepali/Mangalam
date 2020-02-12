using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMB.Mangalam.Web.Model
{
    [Table("family_type")]
    class FamilyType
    {
        public int family_typeid { get; set; }
        public string family_type { get; set; } = "";
        public string family_type_description { get; set; } = "";
    }
}
