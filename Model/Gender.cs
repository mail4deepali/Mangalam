﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MMB.Mangalam.Web.Model
{
    [Table("gender")]
    public class Gender
    {
        public int id { get; set; }
        public string gender { get; set; } = "";
        public string gender_description { get; set; } = "";
    }
}
