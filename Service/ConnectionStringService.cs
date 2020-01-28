using System;
using System.Collections.Generic;
using System.Text;

namespace MMB.Mangalam.Web.Service
{
    public class ConnectionStringService
    {
        public string Value { get; set; }
        public ConnectionStringService(string value)
        {
            this.Value = value;
        }
    }
}
