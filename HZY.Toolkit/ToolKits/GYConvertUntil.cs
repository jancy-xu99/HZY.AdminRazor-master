using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Toolkit.ToolKits
{
    public class GYConvertUntil
    { 

        public static string SqlConvertToString(object obj)
        {
            if (obj != null)
            {
                return "'" + obj + "'";
            }
            return "";
        }


        public static DateTime TryParse(string inputText, DateTime defaultValue)
        {
            if (string.IsNullOrEmpty(inputText))
                return defaultValue;
            DateTime result;
            return DateTime.TryParse(inputText, out result) ? result : defaultValue;
        }
    }




}
