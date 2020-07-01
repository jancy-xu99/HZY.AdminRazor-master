using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;

using System.Security;

using System.Runtime.InteropServices;
using System.ComponentModel;


namespace HZY.Toolkit.ToolKits
{
   public class HttpWebHelpers
    { 
        #region  可自定义参数请求接口
        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url">接口地址</param>
        /// <param name="parameters">参数</param>
        /// <param name="method">请求类型 post 或get</param>
        /// <returns></returns>
        public static string sendPost(string url, IDictionary<string, string> parameters, string method)
        {
            if (method.ToLower() == "post")
            {
                HttpWebRequest req = null;
                HttpWebResponse rsp = null;
                System.IO.Stream reqStream = null;
                try
                {
                    req = (HttpWebRequest)WebRequest.Create(url);
                    req.Method = method;
                    req.KeepAlive = false;
                    req.ProtocolVersion = HttpVersion.Version10;
                    req.Timeout = 5000;
                    req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                    byte[] postData = Encoding.UTF8.GetBytes(BuildQuery(parameters, "utf8"));
                    reqStream = req.GetRequestStream();
                    reqStream.Write(postData, 0, postData.Length);
                    rsp = (HttpWebResponse)req.GetResponse();
                    Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
                    string result = GetResponseAsString(rsp, encoding);
                    return result;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    if (reqStream != null) reqStream.Close();
                    if (rsp != null) rsp.Close();
                }
            }
            else
            {
                //创建请求
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "?" + BuildQuery(parameters, "utf8"));

                //GET请求
                request.Method = "GET";
                request.ReadWriteTimeout = 5000;
                request.ContentType = "text/html;charset=UTF-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

                //返回内容
                string retString = myStreamReader.ReadToEnd();
                return retString;
            }
        }
        public static string HttpWebRequestGet(string url)
        {
            var req = (HttpWebRequest)HttpWebRequest.Create(url);
            var encoding = Encoding.UTF8;
            ////req.Accept = "application/json";
            //设置没有缓存
            req.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            req.Timeout = 600 * 1000;
            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                
                //LogHelp.Error(ex.Message);
            }
            return null;
        }
        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        private static string BuildQuery(IDictionary<string, string> parameters, string encode)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;
            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name))//&& !string.IsNullOrEmpty(value)
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }
                    postData.Append(name);
                    postData.Append("=");
                    if (encode == "gb2312")
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.GetEncoding("gb2312")));
                    }
                    else if (encode == "utf8")
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    }
                    else
                    {
                        postData.Append(value);
                    }
                    hasParam = true;
                }
            }
            return postData.ToString();
        }

        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        private static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            System.IO.Stream stream = null;
            StreamReader reader = null;
            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }

        #region url编码
        public static string UrlEncode(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return url;
            return HttpUtility.UrlEncode(url);
        }
        public static string UrlDncode(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return url;
            return HttpUtility.UrlDecode(url);
        }
        #endregion
        public static string HttpWebRequestGet(HttpMethod method,
             string url,
             string value = null,
             int Timeout_seconds = 600)
        {
            value = UrlEncode(value);
            url = url + "?jsonStr=" + value;
            var req = (HttpWebRequest)HttpWebRequest.Create(url);
            var encoding = Encoding.UTF8;
            req.Accept = "application/json";
            //设置没有缓存
            req.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            req.Timeout = Timeout_seconds * 1000;
            req.Method = method.ToString();
            req.ContentType = "application/json";



            byte[] bs = null;
            if (req.Method == "POST" || req.Method == "PUT")
            {
                if (!string.IsNullOrEmpty(value))
                {

                    bs = encoding.GetBytes(value);
                    req.ContentLength = bs.Length;
                    using (Stream reqStream = req.GetRequestStream())
                    {

                        reqStream.Write(bs, 0, bs.Length);
                        reqStream.Close();
                    }
                }
            }

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                
                //LogHelp.Error(ex.Message);
            }
            return null;
        }
        #endregion

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetCookie(string url, string cookieName, StringBuilder cookieData, ref int size);


        ///url ：你请求的站点地址、
        ///cookieName：cookie名称，获取全部cookie传null、（建议传null）
        ///cookieData：cookie数据保存对象，数据会保存到这个对象中。
        ///size：获取到的实际数据大小。 
        ///dwFlags：cookie的标志（位或运算，目前我接触到的只有：
        /// （0x00002000，httpreadonly）    
        /// lpReserved：保留参数对象（有知道的朋友请留言回复下，谢谢！）
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string url, string cookieName, StringBuilder cookieData, ref int size, int dwFlags, object lpReserved);


        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern int GetLastError();


        /// 下面这个方法是获取到cookie后，对cookie字符串进行拆分重组的。
        public static CookieContainer GetUriCookieContainer(Uri uri)
        {
            CookieContainer cookies = null;
            int datasize = 2048;

            StringBuilder cookieData = new StringBuilder(datasize);
            if (!InternetGetCookie(uri.ToString(), null, cookieData, ref datasize))
            {
                int errCode = GetLastError();
                if (datasize < 0)
                    return null;

                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookie(uri.ToString(), null, cookieData, ref datasize))
                {
                    errCode = GetLastError();
                    return null;
                }

            }
            if (cookieData.Length > 0)
            {
                cookies = new CookieContainer();
                string[] cooks = cookieData.ToString().Split(';');
                for (int i = 0; i < cooks.Length; i++)
                {
                    if (cooks[i].IndexOf(',') == -1)
                        cookies.SetCookies(uri, cooks[i]);
                }
            }
            return cookies;
        }


        public static string GetCookiesString(CookieContainer cookies, Uri uri)
        {
            if (cookies == null || uri == null)
                return "";
            CookieCollection cc = cookies.GetCookies(uri);
            string szCookies = "";
            foreach (Cookie cook in cc)
            {
                szCookies = szCookies + cook.Name + "=" + cook.Value + ";";
            }
            return szCookies;
        }

    }
}
