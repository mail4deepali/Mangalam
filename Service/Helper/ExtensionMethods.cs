using System;
using System.Collections.Generic;
using System.Text;

namespace MMB.Mangalam.Web.Service.Helper
{
    public static class ExtensionMethods
    {
        public static string GetLastThreeDigit(long number, int index)
        {
            string result = "";

            for (int i = 1; i <= index; i++)
            {
                result = (number % 10).ToString() + result;
                number = number / 10;
            }

            return result;
        }
    }
}
