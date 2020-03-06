using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MMB.Mangalam.Web.Model.ViewModel
{
    public class FileToUpload
    {
        public string fileName { get; set; }
        
        public int fileSize { get; set; }
        
        public string fileType { get; set; }
        
        
        public DateTime lastModifiedDate { get; set; }

        public string fileAsBase64 { get; set; }
    }
}
