using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GYWx.Post;
using GYWx.Post.JsonModel;
using HZY.DapperCore.Interface;
using HZY.Models.Act;
using HZY.Models.Common;
using HZY.Toolkit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace HZY.Admin.Controllers
{
    public class ActOauthController : Controller
    {

        /// <summary>
        /// 返回URL
        /// </summary>
        public string GoUrl { get; set; }

        /// <summary>
        /// 活动Id
        /// </summary>
        public string ActId { get; set; }

        public OauthBase _oauthbase;

        CookieOptions cookie = new CookieOptions();

        /// <summary>
        /// 是否是微信再次回调
        /// </summary>
        public bool IsWxCallBack { get; set; }

        public IActivityService _activityService { get; set; }

        /// <summary>
        /// 根据活动Id 获取活动相关配置信息
        /// </summary>
        public ActivityModel _activity
        {
            get
            {
                var model = new ActivityModel();
                ActId = Request.Query["actid"].ToString();
                if (!string.IsNullOrEmpty(ActId))
                {
                    //根据活动号获得活动配置信息   方法待写！
                    model = _activityService.GetActivityById(Convert.ToInt32(ActId));
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
            cookie.HttpOnly = true;
            cookie.IsEssential = true;
            int expire = Convert.ToInt32(60);//过期时间
            cookie.Expires = (expire > 0 ? DateTime.Now.AddMinutes(expire) : DateTime.Now.AddMinutes(60));
            base.OnActionExecuting(context);

            if (_activity != null)
            {
                if (DateTime.Now < _activity.ActStartTime)
                {
                    MessageBox.Show("温馨提示:活动未开始！");
                    return;
                }
                if (DateTime.Now >= _activity.ActEndTime)
                {
                    MessageBox.Show("温馨提示:活动已结束！");
                    return;
                }
                if (!_activity.ActIsEnable)
                {
                    MessageBox.Show("提示:当前活动未激活！");
                    return;
                }
                if (_activity.ActIsWechat)//必须微信端打开
                {
                    //判断微信端用户信息是否有值
                    var data = Request.Cookies["UserCookieModel"];
                    if (string.IsNullOrWhiteSpace(data))
                    {
                        IsWxCallBack = GYLib.Base.Utils.ConvertUtils.ConvertToBool(Request.Query["isWxCallBack"]);
                        if (IsWxCallBack)
                        {
                            GetWeiXinInfo();
                        }
                        else
                        {
                            RedirectWeiXinInfo();
                        }
                    }


                }
            }
            else
            {
                MessageBox.Show("提示:没有获取到相关活动信息，请稍后再试！");
                return;
            }
        }

        private void GetWeiXinInfo()
        {
            #region 获取微信信息

            Response.Cookies.Append("WxOpenId", "error", cookie);
            string resultParam = "";
            if (Request.Query["code"].ToString() != null)
            {
                string code = Request.Query["code"];
                if (!string.IsNullOrEmpty(code))
                {
                    WebClientHelper wc = new WebClientHelper();
                    string strUrl =
                        string.Format(
                            @"https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code"
                            , ConfigurationManager.GetSection("WxAppId")
                            , ConfigurationManager.GetSection("WxSecret")
                            , code);

                    wc.SetUrl(strUrl);
                    string strResult = wc.CallGet("");
                    if (!string.IsNullOrEmpty(strResult))
                    {
                        if (strResult.IndexOf("errcode") > 0)
                        {
                            var json = JsonConvert.SerializeObject(strResult);
                            Response.Cookies.Append("WxOpenId", "error", cookie);
                            // WebSession.WxOpenId = "error";

                        }
                        else
                        {
                            _oauthbase = JsonConvert.DeserializeObject<OauthBase>(strResult);

                            if (_oauthbase != null && !string.IsNullOrEmpty(_oauthbase.openid))
                            {
                                //  WebSession.WxOpenId = _oauthbase.openid;
                                Response.Cookies.Append("WxOpenId", _oauthbase.openid, cookie);
                                resultParam = "openid=" + _oauthbase.openid;

                                //额外获取用户信息
                                WxUserInfo _wxUserInfo = GetWxUserInfo(_oauthbase.openid);
                                if (_wxUserInfo != null && !string.IsNullOrEmpty(_wxUserInfo.openid))
                                {
                                    //ResponseLogin reLogin = new ResponseLogin();
                                    //reLogin.Name = _wxUserInfo.nickname;
                                    //reLogin.WxOpenId = _wxUserInfo.openid;
                                    //reLogin.WxImgUrl = _wxUserInfo.headimgurl;
                                    //reLogin.Subscribe = ConvertUtils.ConvertToInt(_wxUserInfo.subscribe);
                                    //reLogin.Sex = ConvertUtils.ConvertToInt(_wxUserInfo.sex);

                                    ////额外获取支付的OpenId
                                    //RequestLogin login = new RequestLogin();
                                    //login.WxOpenId = _wxUserInfo.openid;
                                    //login.Flag = (int)EnumLoginFlag.WXOpenID;
                                    //ResponseLogin responseLogin = MemberDataAccess.MemberLogin(login, _accesstoken);
                                    //if (responseLogin.IsSucessed == 1)
                                    //{
                                    //    reLogin.PayWxOpenId = responseLogin.PayWxOpenId;
                                    //}
                                    //WebCommon.CreateSession(this.Page.Session, reLogin);
                                    Response.Cookies.Append("UserCookieModel", JsonConvert.SerializeObject(_wxUserInfo), cookie);

                                    resultParam = string.Format("openid={0}&subscribe={3}&nickname={1}&headimgurl={2}", _wxUserInfo.openid, _wxUserInfo.nickname, _wxUserInfo.headimgurl, _wxUserInfo.subscribe ?? "0");
                                }
                                else
                                {
                                    WxAccToken.SetOverDueTime(DateTime.Now.AddHours(-1));
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            // GetNextUrl();
        }

        private void RedirectWeiXinInfo()
        {
            // string qqGoto = ConfigerUtils.WxWebAddressNew + "NewWxOauth.aspx?goUrl=" + Server.UrlEncode(GoUrl) + "&infoType=" + InfoType + "&isWxCallBack=true&param=" + ParamUcode + "&ucode=" + ucode + "&index=" + Index;
            string qqGoto = "";
            Response.Redirect(string.Format(@"https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=STATE#wechat_redirect"
                , ConfigurationManager.GetSection("WxAppId")
                , WebUtility.UrlDecode(qqGoto)));
        }

        private WxUserInfo GetWxUserInfo(string openid)
        {
            WxUserInfo returnResult = null;

            string returnToken = "";
            string returnUserJson = "";

            string wxAccessToken = WxGetMethod.GetWxAccessToken(ref returnToken);
            if (returnToken.Contains("errcode"))
            {
                //日志记录错误信息


                return returnResult;
            }

            returnResult = WxGetMethod.GetUserInfo(openid, wxAccessToken, ref returnUserJson);

            if (returnUserJson.Contains("errcode"))
            {
                //手动刷新wxAccessToken
                WxAccToken.SetOverDueTime(DateTime.Now.AddHours(-1));
                wxAccessToken = WxGetMethod.GetWxAccessToken(ref returnToken);

                //再次请求
                returnResult = WxGetMethod.GetUserInfo(openid, wxAccessToken, ref returnUserJson);
                if (returnResult == null || returnUserJson.Contains("errcode"))
                {
                    //日志记录错误信息


                    return returnResult;
                }
            }

            return returnResult;
        }

        #region Cookie方法

        protected void SetCookies(string key, string value, int minutes = 30)
        {
            HttpContext.Response.Cookies.Append(key, value, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(minutes)
            });
        }

        /// <summary>
        /// 删除指定的cookie
        /// </summary>
        /// <param name="key">键</param>
        protected void DeleteCookies(string key)
        {
            HttpContext.Response.Cookies.Delete(key);
        }

        /// <summary>
        /// 获取cookies
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回对应的值</returns>
        protected string GetCookies(string key)
        {
            HttpContext.Request.Cookies.TryGetValue(key, out string value);
            if (string.IsNullOrEmpty(value))
                value = string.Empty;
            return value;
        }

        #endregion

    }
}
