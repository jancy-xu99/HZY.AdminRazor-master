using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using GYLib.Base.Utils;
using HZY.Toolkit.Entitys;
using HZY.Toolkit.ToolKits;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HZY.Admin.Controllers.Wap
{
    public class WxOauthController : Controller
    {
        public IConfiguration Configuration { get; }

        private static readonly string DefaultUserAgent =
           "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        public WxOauthController(IConfiguration _config)
        {
            Configuration = _config;
        }

        // GET: WxOauth
        public ActionResult Index()
        {
            return View();
        }

        // GET: WxOauth/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WxOauth/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WxOauth/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WxOauth/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WxOauth/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: WxOauth/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WxOauth/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public string ContextGetOpenID(string redirect_url, string code)
        {
            string openid = "";
            var weixin = Configuration.GetSection("WeiXin").Get<WeiXin>();
            openid = GetOpenID(weixin.AppId, redirect_url, code, weixin.AppSecret);
            return openid;
        }

        public string GetOpenID(string appid, string redirect_url, string code, string screct)
        {
            string strJson = "";
            if (string.IsNullOrEmpty(code))
            {
                redirect_url = HttpUtility.UrlEncode(redirect_url);

                HttpContext.Response.Redirect(string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect",
                   appid, redirect_url, new Random().Next(1000, 200000).ToString()));
            }
            else
            {
                strJson = HttpWebUtils.CallGet(string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code",
               appid, screct, code));
            }
            return GetJsonValue(strJson, "openid");
        }




        public string GetJsonValue(string jsonStr, string key)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(jsonStr))
            {
                key = "\"" + key.Trim('"') + "\"";
                int index = jsonStr.IndexOf(key) + key.Length + 1;
                if (index > key.Length + 1)
                {
                    //先截逗号，若是最后一个，截“｝”号，取最小值
                    int end = jsonStr.IndexOf(',', index);
                    if (end == -1)
                    {
                        end = jsonStr.IndexOf('}', index);
                    }

                    result = jsonStr.Substring(index, end - index);
                    result = result.Trim(new char[] { '"', ' ', '\'' }); //过滤引号或空格
                }
            }
            return result;
        }

    }
}