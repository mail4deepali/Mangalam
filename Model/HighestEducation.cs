﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMB.Mangalam.Web.Model
{
    [Table("highest_education")]
    class HighestEducation
    {
        public int educationid { get; set; }
        public string education_degree { get; set; } = "";
        public string education_field_description { get; set; } = "";
    }
}
