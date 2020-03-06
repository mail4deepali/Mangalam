using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MMB.Mangalam.Web.Model.ViewModel
{
   public class FileUploadModel
    {

        [Required]
        public FileToUpload[] fileDetails { get; set; }

        [Required]
        public User user { get; set; }

    }

}
