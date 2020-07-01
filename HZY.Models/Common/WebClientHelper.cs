using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace HZY.Models.Common
{
    public class WebClientHelper
    {
        private string _url;

        public void SetUrl(string url)
        {
            _url = url;
        }

        public string CallGet(string param)
        {
            if (!string.IsNullOrWhiteSpace(param))
            {
                if (_url.IndexOf("?") > 0)
                {
                    _url += param;
                }
                else
                {
                    _url += "?" + param;
                }
            }
            return CallGet();
        }

        public string CallGet()
        {
            string response_content = string.Empty;
            StreamReader reader = null;
            HttpWebResponse response = null;
            try
            {
                HttpWebRequest HWRquest = (HttpWebRequest)WebRequest.Create(_url);
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

        public string CallPost(string param)
        {
            string response_content = string.Empty;
            StreamReader reader = null;
            HttpWebResponse response = null;
            try
            {
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest HWRquest = (HttpWebRequest)WebRequest.Create(_url);

                HWRquest.Method = "POST";
                //HWRquest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                HWRquest.ContentType = "application/xml";
                HWRquest.KeepAlive = true;
                byte[] bs = encoding.GetBytes(param);

                Stream reqStream = HWRquest.GetRequestStream();
                //{
                reqStream.Write(bs, 0, bs.Length);
                reqStream.Close();
                //}

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

        public String CallPost(string param, int Timeout = 5000, string tencoding = "ASCII")
        {
            HttpWebRequest hwRequest;
            HttpWebResponse hwResponse;

            Encoding encoding = Encoding.ASCII;

            switch (tencoding)
            {
                case "UTF8":
                    encoding = Encoding.UTF8;
                    break;
                case "ASCII":
                default:
                    encoding = Encoding.GetEncoding(tencoding);
                    break;
            }


            byte[] bData = encoding.GetBytes(param);

            string strResult = string.Empty;
            try
            {
                hwRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(_url);
                hwRequest.Timeout = Timeout;
                hwRequest.Method = "POST";
                hwRequest.ContentType = "application/x-www-form-urlencoded";
                hwRequest.ContentLength = bData.Length;

                System.IO.Stream smWrite = hwRequest.GetRequestStream();
                smWrite.Write(bData, 0, bData.Length);
                smWrite.Close();
            }
            catch (System.Exception err)
            {
                return strResult;
            }

            //get response
            try
            {
                hwResponse = (HttpWebResponse)hwRequest.GetResponse();
                StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), encoding);
                strResult = srReader.ReadToEnd();
                srReader.Close();
                hwResponse.Close();
            }
            catch (System.Exception err)
            {
                return err.ToString();
            }

            return strResult;
        }

        public string UploadFile(string filename)
        {
            WebClient c = new WebClient();
            string res = string.Empty;
            try
            {
                byte[] result = c.UploadFile(new Uri(_url), filename);
                res = Encoding.Default.GetString(result);
            }
            catch { }
            return res;
        }

        public string UploadFile(string filename, string contenttype)
        {
            FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fileStream);
            byte[] buffer = br.ReadBytes(Convert.ToInt32(fileStream.Length));

            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");

            WebRequest req = WebRequest.Create(_url);
            req.Method = "POST";
            req.ContentType = "multipart/form-data; boundary=" + boundary;

            StringBuilder sb = new StringBuilder();
            sb.Append("--" + boundary + "\r\n");
            sb.Append("Content-Disposition: form-data; name=\"media\"; filename=\"" + filename + "\"" + "\"");
            sb.Append("\r\n");
            sb.Append("Content-Type: " + contenttype);
            sb.Append("\r\n\r\n");
            string head = sb.ToString();
            byte[] form_data = Encoding.UTF8.GetBytes(head);

            byte[] foot_data = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            long length = form_data.Length + fileStream.Length + foot_data.Length;

            req.ContentLength = length;

            Stream requestStream = req.GetRequestStream();

            requestStream.Write(form_data, 0, form_data.Length);

            requestStream.Write(buffer, 0, buffer.Length);

            requestStream.Write(foot_data, 0, foot_data.Length);

            requestStream.Close();
            fileStream.Close();
            fileStream.Dispose();
            br.Close();
            br.Dispose();

            WebResponse pos = req.GetResponse();
            StreamReader sr = new StreamReader(pos.GetResponseStream(), Encoding.UTF8);
            string html = sr.ReadToEnd().Trim();
            sr.Close();
            sr.Dispose();
            if (pos != null)
            {
                pos.Close();
                pos = null;
            }
            if (req != null)
            {
                req = null;
            }
            return html;
        }
    }
}
