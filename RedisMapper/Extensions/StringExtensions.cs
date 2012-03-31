using System;
using System.Collections.Generic;
using System.Web;

namespace RedisMapper
{
    public static class StringExtensions
    {
        const string TRUE = "true";
        const string FALSE = "false";

        private static Dictionary<string, string> CachedEncodedStringValue = new Dictionary<string, string>();

        public static bool EqualsOrdinalIgnoreCase(this string A, string B)
        {
            return string.Equals(A, B, StringComparison.OrdinalIgnoreCase);
        }

        public static Int32 ToInt32(this bool booleanValue)
        {
            return Convert.ToInt32(booleanValue);
        }

        public static Int32 ToInt32(this string integerStr)
        {
            return integerStr.ToInt32(0);
        }

        public static double ToDouble(this string doubleStr)
        {
            double val = 0.0;
            double.TryParse(doubleStr, out val);
            return val;
        }

        public static Int32 ToInt32(this string integerStr, Func<Int32> getDefaultValue)
        {
            return integerStr.ToInt32(getDefaultValue());
        }

        public static Int32 ToInt32(this string integerStr, Int32 defaultValue)
        {
            var value = 0;
            var canParse = int.TryParse(integerStr, out value);
            if (!canParse) value = defaultValue;

            return value;
        }

   

        public static Boolean ToBoolean(this string trueOrFalse)
        {
            var ret = false;
            if (!string.IsNullOrEmpty(trueOrFalse))
            {
                ret = trueOrFalse.Trim().EqualsOrdinalIgnoreCase(TRUE);
            }

            return ret;
        }

        public static Boolean ToBoolean(this int trueOrFalse)
        {
            return trueOrFalse == 1;
        }

      

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}