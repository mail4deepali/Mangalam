﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMB.Mangalam.Web.Model
{
    [Table("sub_caste")]
    public class SubCaste
    {
        public int id { get; set; }
        public string subcaste_name { get; set; } = "";
        public int casteid { get; set; }
    }
}
