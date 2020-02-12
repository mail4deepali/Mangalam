using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMB.Mangalam.Web.Model
{
    [Table("color_complexion")]
    class ColorComplexion
    {
        public int colorid { get; set; }
        public string color_complexion { get; set; } = "";
        public string color_complexion_description { get; set; } = "";
    }
}
