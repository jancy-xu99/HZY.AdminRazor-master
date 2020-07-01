using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace HZY.Toolkit.ToolKits
{
    public static class StringExtensions
    {
        private static readonly Regex WebUrlExpression = new Regex(@"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"
          , RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex EmailExpression = new Regex(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$"
            , RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex StripHTMLExpression = new Regex("<\\S[^><]*>"
            , RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline
            | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static readonly char[] IllegalUrlCharacters = new[] { ';', '/', '\\', '?', ':', '@', '&', '=', '+', '$', ',', '<', '>', '#', '%', '.', '!', '*', '\'', '"', '(', ')', '[', ']', '{', '}', '|', '^', '`', '~', '–', '‘', '’', '“', '”', '»', '«' };

        [DebuggerStepThrough]
        public static bool IsWebUrl(this string target)
        {
            return !string.IsNullOrEmpty(target) && WebUrlExpression.IsMatch(target);
        }

        [DebuggerStepThrough]
        public static bool IsEmail(this string target)
        {
            return !string.IsNullOrEmpty(target) && EmailExpression.IsMatch(target);
        }

        [DebuggerStepThrough]
        public static string StripHtml(this string target)
        {
            return StripHTMLExpression.Replace(target, string.Empty);
        }
        [DebuggerStepThrough]
        public static string Replace(this string target, ICollection<string> oldValues, string newValue)
        {
            foreach (var oldValue in oldValues)
            {
                target = target.Replace(oldValue, newValue);
            }
            return target;
        }

        [DebuggerStepThrough]
        public static string NullSafe(this string target)
        {
            return (target ?? string.Empty).Trim();
        }

        [DebuggerStepThrough]
        public static string FormatWith(this string target, params object[] args)
        {
            Check.Argument.IsNotNullOrEmpty(target, "target");

            return string.Format(Culture.Current, target, args);
        }
        [DebuggerStepThrough]
        public static string Hash(this string target)
        {
            Check.Argument.IsNotNullOrEmpty(target, "target");
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = Encoding.Unicode.GetBytes(target);
                byte[] hash = md5.ComputeHash(data);

                return Convert.ToBase64String(hash);
            }
        }
        [DebuggerStepThrough]
        public static string WrapAt(this string target, int index)
        {
            const int DotCount = 3;

            Check.Argument.IsNotNullOrEmpty(target, "target");
            Check.Argument.IsNotZeroOrNegative(index, "index");

            return (target.Length <= index) ? target : string.Concat(target.Substring(0, index - DotCount), new string('.', DotCount));
        }


        [DebuggerStepThrough]
        public static Guid ToGuid(this string target)
        {
            Guid result = Guid.Empty;

            if ((!string.IsNullOrEmpty(target)) && (target.Trim().Length == 22))
            {
                string encoded = string.Concat(target.Trim().Replace("-", "+").Replace("_", "/"), "==");

                try
                {
                    byte[] base64 = Convert.FromBase64String(encoded);

                    result = new Guid(base64);
                }
                catch (FormatException)
                {
                }
            }

            return result;
        }
        [DebuggerStepThrough]
        public static long ToLong(this string target)
        {
            long result;

            long.TryParse(target, out result);

            return result;
        }

        //[DebuggerStepThrough]
        //public static T ToEnum<T>(this string target, T defaultValue) where T : IComparable, IFormattable
        //{
        //    T convertedValue = defaultValue;

        //    if (!string.IsNullOrEmpty(target))
        //    {
        //        try
        //        {
        //            convertedValue =(T)Enum.Parse(typeof(T), target.Trim(), true);
        //        }
        //        catch (ArgumentException)
        //        {
        //        }
        //    }

        //    return convertedValue;
        //}
        [DebuggerStepThrough]
        public static string ToLegalUrl(this string target)
        {
            if (string.IsNullOrEmpty(target))
            {
                return target;
            }

            target = target.Trim();

            if (target.IndexOfAny(IllegalUrlCharacters) > -1)
            {
                foreach (char character in IllegalUrlCharacters)
                {
                    target = target.Replace(character.ToString(Culture.Current), string.Empty);
                }
            }

            target = target.Replace(" ", "-");

            while (target.Contains("--"))
            {
                target = target.Replace("--", "-");
            }

            return target;
        }
        [DebuggerStepThrough]
        public static string UrlEncode(this string target)
        {
            return HttpUtility.UrlEncode(target);
        }
        [DebuggerStepThrough]
        public static string UrlDecode(this string target)
        {
            return HttpUtility.UrlDecode(target);
        }
        [DebuggerStepThrough]
        public static string AttributeEncode(this string target)
        {
            return HttpUtility.HtmlAttributeEncode(target);
        }
        [DebuggerStepThrough]
        public static string HtmlEncode(this string target)
        {
            return HttpUtility.HtmlEncode(target);
        }
        [DebuggerStepThrough]
        public static string HtmlDecode(this string target)
        {
            return HttpUtility.HtmlDecode(target);
        }
        /// <summary>
        /// 拆分字符串：默认以‘|’分割
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="strs"></param>
        /// <param name="splitChars"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string[] SplitByChar(this string target, char[] splitChars)
        {
            string[] stResult = null;
            foreach (var chr in splitChars)
            {
                if (target.IndexOf(chr) >= 0)
                {
                    stResult = target.Split(chr);
                    break;
                }
            }

            if (stResult == null)
                stResult = target.Split('|'); //默认以此分割

            //去除空值
            if (stResult != null)
            {
                var list = stResult.ToList().Where(c => !string.IsNullOrEmpty(c));
                return list.ToArray();
            }

            return stResult;
        }
        /// <summary>
        /// http://msdn.microsoft.com/zh-cn/library/vstudio/system.io.stringreader.readline.aspx
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string ReadToString(this string target)
        {
            // From textReaderText, create a continuous paragraph 
            // with two spaces between each sentence.
            string aLine, aParagraph = null;
            StringReader strReader = new StringReader(target);
            while (true)
            {
                aLine = strReader.ReadLine();

                if (aLine != null)
                {
                    aParagraph = aParagraph + aLine;// +" ";
                }
                else
                {
                    //aParagraph = aParagraph + "\n";
                    break;
                }
            }
            strReader.Close();

            return aParagraph;//.Replace("\\\\n","\n").Replace("\\\"","\"");
        }
        [DebuggerStepThrough]
        public static string ConvertToHtmlCode(this string target)
        {
            string result = "";
            result = target.Replace("<", "&lt;");
            result = result.Replace(">", "&gt;");
            result = result.Replace("\r\n", "<BR>");
            result = result.Replace(" ", "&nbsp;");
            return result;
        }
        public static string GetMD5Hash(this string str)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(str));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
        /// <summary>
        /// http://blog.sina.com.cn/s/blog_3fa7f1a60100sk6h.html
        /// </summary>
        /// <param name="strSrc"></param>
        /// <returns></returns>
        public static bool IsGuid(this string strSrc)
        {
            Guid g = Guid.Empty;
            return Guid.TryParse(strSrc, out g);
        }
    }
}
