

using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;


namespace HZY.Toolkit.ToolKits
{
    public class WebClientHelper
    {
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


        //public static CheckResult CallGetReturnModel(string strUrl)
        //{
        //    CheckResult result = new CheckResult();

        //    StreamReader reader = null;
        //    HttpWebResponse response = null;
        //    try
        //    {
        //        HttpWebRequest HWRquest = (HttpWebRequest)WebRequest.Create(strUrl);
        //        HWRquest.Timeout = 300000;

        //        response = (HttpWebResponse)HWRquest.GetResponse();
        //        if (response.StatusCode != HttpStatusCode.OK)
        //        {
        //            result.IsPass = false;
        //            result.CheckMessage = "请求失败";
        //        }

        //        reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));
        //        result.IsPass = true;
        //        result.CheckMessage = reader.ReadToEnd();
        //    }
        //    catch (Exception e)
        //    {
        //        result.IsPass = false;
        //        result.CheckMessage = e.Message;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //            reader.Close();
        //        if (response != null)
        //            response.Close();
        //    }

        //    return result;
        //}

        public static string CallPost(string strUrl, string param, Encoding encoding)
        {
            string response_content = string.Empty;
            StreamReader reader = null;
            HttpWebResponse response = null;
            ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            try
            {
                HttpWebRequest HWRquest = (HttpWebRequest)WebRequest.Create(strUrl);

                HWRquest.Method = "POST";
                HWRquest.KeepAlive = true;
                HWRquest.ContentType = "application/x-www-form-urlencoded";
                byte[] bs = encoding.GetBytes(param);
                Stream reqStream = HWRquest.GetRequestStream();
                reqStream.Write(bs, 0, bs.Length);
                reqStream.Close();
                response = (HttpWebResponse)HWRquest.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    response_content = "error";
                }
                reader = new StreamReader(response.GetResponseStream(), encoding);
                response_content = reader.ReadToEnd();
            }
            catch (Exception e)
            {
                //LogHelp.Error("调用 CallPost方法" + e.Message);
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

        public static string CallPost1(Encoding encoding, string _url, string param)
        {
            HttpWebRequest hwRequest;
            HttpWebResponse hwResponse;



            byte[] bData = encoding.GetBytes(param);

            string strResult = string.Empty;
            try
            {
                hwRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(_url);
                hwRequest.Timeout = 500;
                hwRequest.Method = "POST";
                hwRequest.ContentType = "application/x-www-form-urlencoded";
                hwRequest.ContentLength = bData.Length;

                System.IO.Stream smWrite = hwRequest.GetRequestStream();
                smWrite.Write(bData, 0, bData.Length);
                smWrite.Close();

                hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), encoding);
                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();
            }
            catch (System.Exception err)
            {
                //LogHelp.Error("调用 CallPost1方法" + err.Message);
                return err.ToString();

            }

            return strResult;
        }



        public static string CallPost(string strUrl, string param, Encoding encoding, string oauthFile, string key)
        {
            //LogBase.LogHzCrm.LogInfo("000000", ELogType.Info);

            string response_content = string.Empty;
            StreamReader reader = null;
            HttpWebResponse response = null;
            ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            try
            {
                //LogBase.LogHzCrm.LogInfo("111111", ELogType.Info);
                //LogBase.LogHzCrm.LogInfo(oauthFile + ":::" + key, ELogType.Info);
                X509Certificate2 cer = new X509Certificate2(oauthFile, key, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);


                HttpWebRequest HWRquest = (HttpWebRequest)WebRequest.Create(strUrl);
                HWRquest.ClientCertificates.Add(cer);
                //LogBase.LogHzCrm.LogInfo("22222", ELogType.Info);

                HWRquest.Method = "POST";
                HWRquest.KeepAlive = true;
                byte[] bs = encoding.GetBytes(param);
                Stream reqStream = HWRquest.GetRequestStream();
                reqStream.Write(bs, 0, bs.Length);
                reqStream.Close();
                response = (HttpWebResponse)HWRquest.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    response_content = "error";
                }
                reader = new StreamReader(response.GetResponseStream(), encoding);
                response_content = reader.ReadToEnd();
                //LogBase.LogHzCrm.LogInfo("33333", ELogType.Info);
            }
            catch (Exception e)
            {
                response_content = e.Message;
                //LogHelp.Error("CallPost 方法"+ e.StackTrace);
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

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            // 总是接受  
            return true;
        }

        /// <summary>
        /// 根据链接下载图片到本地
        /// </summary>
        /// <param name="url">网址路径</param>
        /// <param name="savapath">本地保存路径</param>
        /// <returns> 返回保存后的文件路径及名称</returns>
        public static string DownLoadImages(string url, string WebRootPath)
        {

            //获取网站当前根目录
            string sWebRootFolder = WebRootPath;
            //保存图片路径
            var savePath = string.Format("\\Uploads\\{0}\\{1}\\{2}\\", DateTime.Now.Year, DateTime.Now.Month.ToString("D2"), DateTime.Now.Day.ToString("D2"));
            //文件名
            string filename = (System.IO.Path.GetFileName(url)).Replace("\"", "");
            //扩展名
            string extension = System.IO.Path.GetExtension(url);
            string webPath = savePath.Replace("\\".ToString(), '/'.ToString());
            savePath = sWebRootFolder + savePath;
            //无夹创建
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            WebClient mywebclient = new WebClient();

            //下载文件
            mywebclient.DownloadFile(new Uri(url), savePath + filename);

            return webPath + (filename.Contains(extension) ? filename : filename + extension);
        }

    }
}