using System;
using System.Collections.Generic;
using System.Text;

namespace MMB.Mangalam.Web.Service
{
    public class FileUploadStringService
    {
        public string Value { get; set; }
        public FileUploadStringService(string value)
        {
            this.Value = value;
        }
    }
}
