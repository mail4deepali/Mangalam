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

        public static string Messages(this FluentValidation.Results.ValidationResult result)
        {
            StringBuilder message = new StringBuilder();
            foreach(var error in result.Errors)
            {
                message.Append($"{error.ErrorMessage}\n");
            }

            return message.ToString();
        }
    }
}
