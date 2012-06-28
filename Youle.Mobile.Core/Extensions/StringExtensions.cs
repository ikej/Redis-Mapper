using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace Youle.Mobile.Core
{
    public static class StringExtensions
    {
        const string TRUE = "true";
        const string FALSE = "false";
        public const string MatchEmailPattern =
           @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
    + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
    + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
    + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

        private static Dictionary<string, string> CachedEncodedStringValue = new Dictionary<string, string>();

        public static bool EqualsOrdinalIgnoreCase(this string A, string B)
        {
            return string.Equals(A, B, StringComparison.OrdinalIgnoreCase);
        }

        public static string MakeSureNotNull(this string val)
        {
            if (val == null)
            {
                return string.Empty;
            }
            else
            {
                return val;
            }
        }

        public static bool IsEmail(this string emailStr)
        {
            return Regex.IsMatch(emailStr, MatchEmailPattern);
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

        /// <summary>
        /// This function will encode the special char with url encode strategy
        /// </summary>
        /// <param name="value"></param>
        /// <param name="special"></param>
        /// <returns></returns>
        public static string EncodeSpecial(this string value, string special)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var encoded = string.Empty;

                if (CachedEncodedStringValue.ContainsKey(special)) encoded = CachedEncodedStringValue[special];
                else
                {
                    encoded = HttpUtility.UrlEncode(special);
                    CachedEncodedStringValue[special] = encoded;
                }

                return value.Replace(special, encoded);
            }

            return value;
        }

        /// <summary>
        /// Will call HttpUtility.UrlEncode method
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UrlEncode(this string value)
        {
            return HttpUtility.UrlEncode(value);
        }

        /// <summary>
        /// 对URL中的中文进行编码
        /// 其中也包括空格
        /// 全角符号及《》
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EncodeChineseChars(this string value)
        {
            // chinese characters & space chars
            const string CHINESE_PATTERN = @"[(\u4e00-\u9fa5)|(\uFF00-\uFFFF)|\s|《》]*";

            var evaluator = new MatchEvaluator(match =>
            {
                var ret = match.Value.UrlEncode();
                return ret;
            });

            value = Regex.Replace(value, CHINESE_PATTERN, evaluator);

            return value;
        }

        /// <summary>
        /// Will call HttpUtility.UrlEncode method
        /// </summary>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string UrlEncode(this string value, Encoding encoding)
        {
            return HttpUtility.UrlEncode(value, encoding);
        }

        public static string MakeSureUnicodeStringByteLength(this string value, int limitLength)
        {
            // there are 2 bytes for one character for unicode encoding, C# uses Unicode which is 2 bytes per character by default
            if (value.Length > limitLength / 2)
            {
                value = value.Substring(0, limitLength / 2);
            }

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


        public static string ConfigValue(this string key)
        {
            return SingletonBase<ConfigurableSet>.Instance[key];
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Reformat mobile
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static string MobileFormat(this string mobile)
        {
            if (mobile.IsNullOrEmpty()) return string.Empty;

            if (mobile.StartsWith("9")) return mobile;

            return mobile.Substring(mobile.IndexOf("1"));
        }
    }
}