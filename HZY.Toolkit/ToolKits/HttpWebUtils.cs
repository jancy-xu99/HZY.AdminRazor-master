
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web;

namespace HZY.Toolkit.ToolKits
{
    public static class HttpWebUtils
    {
        /// <summary>
        /// 获取客户端请求的数据
        /// </summary>
        /// <param name="httpRequest"></param>
        /// <param name="requestEncoding"></param>
        /// <returns></returns>
        public static string GetClientRequestData(HttpRequest httpRequest, Encoding requestEncoding)
        {
            string requestStr = string.Empty;
            try
            {
                if (httpRequest.ContentLength > 0)
                {
                    using (var reader = new StreamReader(httpRequest.Body, requestEncoding, false))
                    {
                        requestStr = reader.ReadToEnd();
                    }
                }
                return requestStr;
            }
            catch (Exception exception)
            {
                //LogHelp.Error(exception.Message);
                return exception.Message;
            }
        }

        /// <summary>
        /// 将服务器数据输出到客户端
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <param name="encoding"></param>
        /// <param name="postData"></param>
        public static void ResponseDataToClient(HttpResponse httpResponse, Encoding encoding, string postData)
        {
            if (httpResponse != null)
            {
                try
                {
                    //httpResponse.ContentEncoding = encoding;
                    using (var ms = new MemoryStream(encoding.GetBytes(postData)))
                    {
                        var block = new byte[1024];
                        int len = ms.Read(block, 0, block.Length);
                        while (len > 0)
                        {
                            httpResponse.Body.Write(block, 0, len);
                            len = ms.Read(block, 0, block.Length);
                        }
                    }
                    httpResponse.Body.Flush();
                }
                catch (Exception exception)
                {
                    //LogHelp.Error(exception.Message);
                }
            }
        }



        /// <summary>
        /// 获取客户端的请求Ip
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetClientRequestIp(HttpRequest request)
        {
            try
            {
                var iPAddress = request.HttpContext.Connection.RemoteIpAddress;
                if (iPAddress.ToString() == "::1")
                {
                    return "127.0.0.1";
                }
                else
                {
                    return iPAddress.ToString();
                }
            }
            catch (Exception ex)
            {
                //LogHelp.Error(ex.Message);
                return "IP获取失败！";
            }

        }




        /// <summary>
        /// 获取客户端的请求Ip
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        //public static string GetClientRequestIp(HttpRequest request)
        //{
        //    //如果客户端使用了代理服务器，则利用HTTP_X_FORWARDED_FOR找到客户端IP地址
        //    string userHostAddress = string.Empty;

        //    if (!string.IsNullOrEmpty(request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
        //        userHostAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',')[0].Trim();
        //    //否则直接读取REMOTE_ADDR获取客户端IP地址
        //    if (string.IsNullOrEmpty(userHostAddress))
        //    {
        //        userHostAddress = request.ServerVariables["REMOTE_ADDR"];
        //    }
        //    //前两者均失败，则利用Request.UserHostAddress属性获取IP地址，但此时无法确定该IP是客户端IP还是代理IP
        //    if (string.IsNullOrEmpty(userHostAddress))
        //    {
        //        userHostAddress = request.UserHostAddress;
        //    }
        //    //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
        //    if (!string.IsNullOrEmpty(userHostAddress) && CheckIpFormat(userHostAddress))
        //    {
        //        // Log.LogBase.TaSrvLog.LogOpt("Get Ip", "TaApi", 1, userHostAddress);
        //        return userHostAddress;
        //    }
        //    return "127.0.0.1";
        //}

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool CheckIpFormat(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// 获取Uri内容中query的字典
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static IDictionary<string, string> GetRequestUrlQueryStr(Uri uri)
        {
            if (uri != null && !string.IsNullOrEmpty(uri.Query))
            {
                IDictionary<string, string> result = new Dictionary<string, string>();
                foreach (var c in uri.Query.TrimStart('?').Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var arr = c.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if (arr.Length >= 2)
                    {
                        result.Add(arr[0], arr[1]);
                    }
                }
                return result;
            }
            return null;
        }

        /// <summary>
        /// 执行HttpWebRequest请求
        /// </summary>
        /// <param name="postUrl">待请求地址</param>
        /// <param name="methodType">请求方法</param>
        /// <param name="paramData">待请求数据</param>
        /// <param name="dataEncode">待请求数据格式</param>
        /// <returns></returns>
        public static string PostHttpWebRequest(string postUrl, string methodType, string paramData, Encoding dataEncode)
        {
            var request = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));

            byte[] bytes = dataEncode.GetBytes(paramData);

            request.Method = methodType;
            request.ContentType = "application/json";

            request.ContentLength = bytes.Length;

            using (Stream writeStream = request.GetRequestStream())
            {
                writeStream.Write(bytes, 0, bytes.Length);
                writeStream.Close();
            }
            string result = "";

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                        using (var readStream = new StreamReader(responseStream, Encoding.UTF8))
                        {
                            result = readStream.ReadToEnd();
                        }
                }
            }

            return result;
        }

        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        public static string CallGet(string strUrl)
        {
            string response_content = string.Empty;
            StreamReader reader = null;
            HttpWebResponse response = null;
            try
            {
                HttpWebRequest HWRquest = (HttpWebRequest)WebRequest.Create(strUrl);
                HWRquest.Timeout = 300000;

                response = (HttpWebResponse)HWRquest.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    response_content = "error";
                }

                reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
                response_content = reader.ReadToEnd();
            }
            catch (Exception e)
            {
                response_content = e.Message;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (response != null)
                    response.Close();
            }
            return response_content;
        }
        #region url编码
        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlEncode(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return url;
            return HttpUtility.UrlEncode(url);
        }
        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlDncode(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return url;
            return HttpUtility.UrlDecode(url);
        }
        #endregion




    }


}
