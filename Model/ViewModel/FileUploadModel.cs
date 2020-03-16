using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace MMB.Mangalam.Web.Model.ViewModel
{
   public class FileUploadModel
    {
        public IList<IFormFile>  fileDetails { get; set; }

        public User user { get; set; }

    }

}
