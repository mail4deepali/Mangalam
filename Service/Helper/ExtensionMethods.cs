using System;
using System.Collections.Generic;
using System.Text;

namespace MMB.Mangalam.Web.Service.Helper
{
    public static class ExtensionMethods
    {
        public static string LastNChars(this string source, int tail_length)
        {
            if (tail_length >= source.Length)
                return source;

            return source.Substring(source.Length - tail_length);
        }
    }
}
