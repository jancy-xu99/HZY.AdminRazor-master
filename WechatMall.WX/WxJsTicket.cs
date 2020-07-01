

using GYWx.Common;
using WechatMall.WX.Model.Wx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WechatMall.Wx
{
    public static class WxJsTicket
    {
        private static object _obj;
        private static DateTime _overDue;
        private static string _ticket;
        static WxJsTicket()
        {
            _obj = new object();
            _ticket = string.Empty;
        }

        public static string GetJsTicket(string access_token)
        {
            lock (_obj)
            {
                if (_overDue <= DateTime.Now || string.IsNullOrEmpty(_ticket))
                {
                    var tokenModel = _GetTicket(access_token);
                    if (tokenModel != null)
                    {
                        int i;
                        if (!int.TryParse(tokenModel.expires_in, out i))
                        {
                            i = 6000;
                        }
                        _overDue = DateTime.Now.AddSeconds(i - 2000);
                        _ticket = tokenModel.ticket;
                    }
                }
            }
            return _ticket;

        }
        public static void SetOverDueTime(DateTime dt)
        {
            lock (_obj)
            {
                _overDue = dt;
            }
        }
        private static JsTicket _GetTicket(string access_token)
        {
            string url = string.Format(@"https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", access_token);
            string response_content = WebClientHelper.CallGet(url);
            if (response_content.IndexOf("ticket") > 0)
            {
                // LogBase.LogHzCrm.LogInfo("ticket:" + response_content, ELogType.Info);

                //JavaScriptSerializer serializer = new JavaScriptSerializer();
                return GYLib.Base.Utils.JsonUtils.ParseFromJson<JsTicket>(response_content);

                //return serializer.Deserialize<JsTicket>(response_content);
            }
            return null;
        }

    }
}
