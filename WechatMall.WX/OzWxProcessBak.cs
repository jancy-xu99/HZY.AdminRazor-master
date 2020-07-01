using GYWx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatMall.Wx
{
    public class OzWxProcessBak : IWxProcess
    {
        public string GetWXToken()
        {
            //测试
            //   string json = null;
            //return WxGetMethod.GetWxAccessToken(ref json);
            //防止和现有环境冲突
            string json = null;
            return GYWx.Post.WxGetMethod.GetWxAccessToken(ref json);
        }


        public GYWx.Reply.ReplayMsgBase ProcessLcationEvent(GYWx.Receive.EventMsg.LocationEvent sender)
        {

            //string locationStr = "";
            //if (sender != null)
            //{
            //    locationStr = sender.Latitude + "|" + sender.Longitude + "|" + sender.Precision;
            //}

            ////发送模板
            //string templeId = "mAsbg-aH9UpHgVbFUY9TH3PUBZNRd923fcxQVefWU6k";
            //string json = null;
            //string token = GetWXToken();
            //var input = new SendTempletInput();
            //input.template_id = templeId;
            //input.touser = sender.FromUserName;
            //input.topcolor = "#FF0000";
            //input.url = "http://weixin.qq.com/download";
            //input.data = new TempletMsgLoginCls()
            //{
            //    first = new TempletMsgTextCls() { value = "测试登陆验证地理位置事件:", color = "#FF0000" },
            //    keyword1 = new TempletMsgTextCls() { value = "locationinfo:" + locationStr, color = "#FF0000" },
            //    keyword2 = new TempletMsgTextCls() { value = "登陆验证地理位置事件:", color = "#FF0000" },
            //    keyword3 = new TempletMsgTextCls() { value = "发送目标微信号：" + sender.FromUserName, color = "#FF0000" },
            //    remark = new TempletMsgTextCls() { value = "发送时间：" + sender.CreateTime, color = "#FF0000" },
            //};
            //var result = WxGetMethod.SendTempletMsg(input, token, ref json);

            //return null;

            return null;
        }

        public GYWx.Reply.ReplayMsgBase ProcessLinkMsg(GYWx.Receive.NormalMsg.LinkMsg sender)
        {

            ////发送模板
            //string templeId = "mAsbg-aH9UpHgVbFUY9TH3PUBZNRd923fcxQVefWU6k";
            //string json = null;
            //string token = GetWXToken();
            //var input = new SendTempletInput();
            //input.template_id = templeId;
            //input.touser = sender.FromUserName;
            //input.topcolor = "#FF0000";
            //input.url = "http://weixin.qq.com/download";
            //input.data = new TempletMsgLoginCls()
            //{
            //    first = new TempletMsgTextCls() { value = "测试发送连接:", color = "#FF0000" },
            //    keyword1 = new TempletMsgTextCls() { value = "Url:" + sender.Url, color = "#FF0000" },
            //    keyword2 = new TempletMsgTextCls() { value = "连接:", color = "#FF0000" },
            //    keyword3 = new TempletMsgTextCls() { value = "发送目标微信号：" + sender.FromUserName, color = "#FF0000" },
            //    remark = new TempletMsgTextCls() { value = "发送时间：" + sender.CreateTime, color = "#FF0000" },
            //};
            //var result = WxGetMethod.SendTempletMsg(input, token, ref json);


            //GYWx.Reply.NormalReply.Replytext text = new Replytext();
            //text.FromUserName = sender.ToUserName;
            //text.ToUserName = sender.FromUserName;
            //text.CreateTime = sender.CreateTime;
            //text.Content = "连接已收到";
            //return text;

            return null;
        }

        public GYWx.Reply.ReplayMsgBase ProcessLocationMsg(GYWx.Receive.NormalMsg.LocationMsg sender)
        {

            //string locationStr = "";
            //if (sender != null)
            //{
            //    locationStr = sender.Label + "|" + sender.Location_X + "|" + sender.Location_Y + "|" + sender.MsgId + "|" + sender.MsgType + "|" + sender.Scale;
            //}

            ////发送模板
            //string templeId = "mAsbg-aH9UpHgVbFUY9TH3PUBZNRd923fcxQVefWU6k";
            //string json = null;
            //string token = GetWXToken();
            //var input = new SendTempletInput();
            //input.template_id = templeId;
            //input.touser = sender.FromUserName;
            //input.topcolor = "#FF0000";
            //input.url = "http://weixin.qq.com/download";
            //input.data = new TempletMsgLoginCls()
            //{
            //    first = new TempletMsgTextCls() { value = "测试发送地理位置:", color = "#FF0000" },
            //    keyword1 = new TempletMsgTextCls() { value = "locationinfo:" + locationStr, color = "#FF0000" },
            //    keyword2 = new TempletMsgTextCls() { value = "地理位置:", color = "#FF0000" },
            //    keyword3 = new TempletMsgTextCls() { value = "发送目标微信号：" + sender.FromUserName, color = "#FF0000" },
            //    remark = new TempletMsgTextCls() { value = "发送时间：" + sender.CreateTime, color = "#FF0000" },
            //};
            //var result = WxGetMethod.SendTempletMsg(input, token, ref json);


            //GYWx.Reply.NormalReply.Replytext text = new Replytext();
            //text.FromUserName = sender.ToUserName;
            //text.ToUserName = sender.FromUserName;
            //text.CreateTime = sender.CreateTime;
            //text.Content = "地理位置已收到";
            //return text;

            return null;
        }

        public GYWx.Reply.ReplayMsgBase ProcessMenuEvent(GYWx.Receive.EventMsg.MenuEvent sender)
        {
            //GYWx.Reply.ReplayMsgBase msg = null;
            ////测试
            //if (sender.Event.ToLower() == "click")
            //{
            //    //string json = null;
            //    //string token = GetWXToken();
            //    //var input = new ReplyBMsgUserText();
            //    //input.touser = new string[] { sender.FromUserName };
            //    //input.text = new GYWx.Post.JsonModel.Cls.ContentCls() { content = "测试发送模板素材" };
            //    //input.msgtype = "text";

            //    //var result = WxGetMethod.SendBatchMsgByOpenID(input, token, ref json);

            //    GYWx.Reply.NormalReply.Replynews arictil = new Replynews();
            //    arictil.FromUserName = sender.ToUserName;
            //    arictil.ToUserName = sender.FromUserName;
            //    arictil.CreateTime = sender.CreateTime;
            //    arictil.MsgType = "news";
            //    arictil.ArticleCount = "1";
            //    arictil.Articles = new ArticleCls()
            //    {
            //        item = new NewsCls[]{
            //              new NewsCls(){
            //                Url="123231",
            //                 Title="123123",
            //                  Description="213123",
            //                   PicUrl="2313"
            //              }
            //        }
            //    };
            //    return arictil;

            //    //GYWx.Reply.NormalReply.Replytext text = new Replytext();
            //    //text.FromUserName = sender.ToUserName;
            //    //text.ToUserName = sender.FromUserName;
            //    //text.CreateTime = sender.CreateTime;
            //    //text.Content = "点击clike测试:" ;
            //    //return text;
            //}
            //else
            //{
            //    return msg;
            //}
            return null;
        }

        public GYWx.Reply.ReplayMsgBase ProcessPicMsg(GYWx.Receive.NormalMsg.PicMsg sender)
        {
            return null;
            ////发送模板
            //string templeId = "mAsbg-aH9UpHgVbFUY9TH3PUBZNRd923fcxQVefWU6k";
            //string json = null;
            //string token = GetWXToken();
            //var input = new SendTempletInput();
            //input.template_id = templeId;
            //input.touser = sender.FromUserName;
            //input.topcolor = "#FF0000";
            //input.url = "http://weixin.qq.com/download";
            //input.data = new TempletMsgLoginCls()
            //{
            //    first = new TempletMsgTextCls() { value = "测试发送图片事件:", color = "#FF0000" },
            //    keyword1 = new TempletMsgTextCls() { value = "PicUrl:" + sender.PicUrl, color = "#FF0000" },
            //    keyword2 = new TempletMsgTextCls() { value = "发送图片事件:" + sender.ToUserName, color = "#FF0000" },
            //    keyword3 = new TempletMsgTextCls() { value = "发送目标微信号：" + sender.FromUserName, color = "#FF0000" },
            //    remark = new TempletMsgTextCls() { value = "发送时间：" + sender.CreateTime, color = "#FF0000" },
            //};
            //var result = WxGetMethod.SendTempletMsg(input, token, ref json);

            //GYWx.Reply.NormalReply.Replynews arictil = new Replynews();
            //arictil.FromUserName = sender.ToUserName;
            //arictil.ToUserName = sender.FromUserName;
            //arictil.CreateTime = sender.CreateTime;
            //arictil.ArticleCount = "2";
            //arictil.Articles = new ArticleCls()
            //{
            //    item = new NewsCls[]{
            //              new NewsCls(){
            //                 Title="测试图文1",
            //                  Description="测试图文描述1",
            //                   PicUrl="www.baidu.com",
            //                    Url="www.baidu.com" 
            //              }
            //              ,
            //                new NewsCls(){
            //                 Title="测试图文2",
            //                  Description="测试图文描述2",
            //                   PicUrl="www.baidu.com",
            //                    Url="www.baidu.com" 
            //              }
            //        }
            //};
            //return arictil;

            //GYWx.Reply.NormalReply.Replytext text = new Replytext();
            //text.FromUserName = sender.ToUserName;
            //text.ToUserName = sender.FromUserName;
            //text.CreateTime = sender.CreateTime;
            //text.Content = "图片已收到";
            //return text;

            return null;
        }


        public GYWx.Reply.ReplayMsgBase ProcessSubscribeEvent2(GYWx.Receive.EventMsg.SubscribeEvent2 sender)
        {
            /////订阅
            //if (sender.Event == "subscribe")
            //{
            //    GYWx.Reply.NormalReply.Replytext text = new Replytext();
            //    text.FromUserName = sender.ToUserName;
            //    text.ToUserName = sender.FromUserName;
            //    text.CreateTime = sender.CreateTime;
            //    string wxwebsite = ConfigurationManager.AppSettings["WXWebAddress"];
            //    string url = "<a href=\"" + wxwebsite + "airport/LotteryCoupon.aspx\">测试</a>";

            //    text.Content = HttpUtility.HtmlEncode(url);
            //    if (!string.IsNullOrEmpty(sender.EventKey) && sender.EventKey.StartsWith("qrscene_"))
            //    {
            //        string param = sender.EventKey.Substring("qrscene_".Length);
            //        ///扫描二维码关注
            //        ///
            //        text.Content = System.Web.HttpContext.Current.Server.HtmlEncode("Hello World(<a href=\"www.baidu.com\">扫描关注</a>)");

            //        //发送模板
            //        string templeId = "mAsbg-aH9UpHgVbFUY9TH3PUBZNRd923fcxQVefWU6k";
            //        string json = null;
            //        string token = GetWXToken();
            //        var input = new SendTempletInput();
            //        input.template_id = templeId;
            //        input.touser = sender.FromUserName;
            //        input.topcolor = "#FF0000";
            //        input.url = "http://weixin.qq.com/download";
            //        input.data = new TempletMsgLoginCls()
            //        {
            //            first = new TempletMsgTextCls() { value = "测试数据微信扫二维码关注", color = "#FF0000" },
            //            keyword1 = new TempletMsgTextCls() { value = "参数：" + param, color = "#FF0000" },
            //            keyword2 = new TempletMsgTextCls() { value = "连接Url（测试：<a href=\"www.baidu.com\">测试</a>） :" + input.url, color = "#FF0000" },
            //            keyword3 = new TempletMsgTextCls() { value = "发送目标微信号：" + sender.FromUserName, color = "#FF0000" },
            //            remark = new TempletMsgTextCls() { value = "发送时间：" + sender.CreateTime, color = "#FF0000" },
            //        };
            //        var result = WxGetMethod.SendTempletMsg(input, token, ref json);
            //    }

            //    return text;
            //}
            ////取消订阅
            //if (sender.Event == "unsubscribe")
            //{
            //    GYWx.Reply.NormalReply.Replytext text = new Replytext();
            //    text.FromUserName = sender.ToUserName;
            //    text.ToUserName = sender.FromUserName;
            //    text.CreateTime = sender.CreateTime;
            //    text.Content = "Hello World";
            //    if (!string.IsNullOrEmpty(sender.EventKey) && sender.EventKey.StartsWith("qrscene_"))
            //    {
            //        string param = sender.EventKey.Substring("qrscene_".Length);
            //        ///扫描二维码关注
            //        ///
            //        text.Content = "Hello World(取消关注)";

            //        //发送模板
            //        string templeId = "mAsbg-aH9UpHgVbFUY9TH3PUBZNRd923fcxQVefWU6k";
            //        string json = null;
            //        string token = GetWXToken();
            //        var input = new SendTempletInput();
            //        input.template_id = templeId;
            //        input.touser = sender.FromUserName;
            //        input.topcolor = "#FF0000";
            //        input.url = "http://weixin.qq.com/download";
            //        input.data = new TempletMsgLoginCls()
            //        {
            //            first = new TempletMsgTextCls() { value = "测试取消关注", color = "#FF0000" },
            //            keyword1 = new TempletMsgTextCls() { value = "参数：" + param, color = "#FF0000" },
            //            keyword2 = new TempletMsgTextCls() { value = "连接Url（测试：<a href=\"www.baidu.com\">测试</a>） :" + input.url, color = "#FF0000" },
            //            keyword3 = new TempletMsgTextCls() { value = "发送目标微信号：" + sender.FromUserName, color = "#FF0000" },
            //            remark = new TempletMsgTextCls() { value = "发送时间：" + sender.CreateTime, color = "#FF0000" },
            //        };
            //        var result = WxGetMethod.SendTempletMsg(input, token, ref json);
            //    }

            //    return text;
            //}

            ////扫描二维码
            //if (sender.EventKey == "SCAN")
            //{

            //}

            //return null;
            return null;
        }

        public GYWx.Reply.ReplayMsgBase ProcessTextMsg(GYWx.Receive.NormalMsg.TextMsg sender)
        {
            //try
            //{
            //    string strUrl = @"http://dkf.ozner.net/service/wxcustomer.aspx";

            //    string date = WxXmlHelper.CreateXml(sender);
            //    string result = WebClientHelper.CallPost(strUrl, date, Encoding.UTF8);
            //    //result = "消息已收到！" + result;

            //    //GYWx.Reply.NormalReply.Replytext text = new Replytext();
            //    //text.FromUserName = sender.ToUserName;
            //    //text.ToUserName = sender.FromUserName;
            //    //text.CreateTime = sender.CreateTime;
            //    //text.Content = result;
            //    return null;
            //}
            //catch (Exception ex)
            //{
            //    LogBase.LogHzCrm.LogExErr(ex);

            //    GYWx.Reply.NormalReply.Replytext text = new Replytext();
            //    text.FromUserName = sender.ToUserName;
            //    text.ToUserName = sender.FromUserName;
            //    text.CreateTime = sender.CreateTime;
            //    text.Content = "发生了异常:" + ex.Message;
            //    return text;
            //}
            return null;
        }

        public GYWx.Reply.ReplayMsgBase ProcessVideoMsg(GYWx.Receive.NormalMsg.VideoMsg sender)
        {
            //string locationStr = "";
            //if (sender != null)
            //{
            //    locationStr = sender.MediaId + "|" + sender.ThumbMediaId;
            //}

            ////发送模板
            //string templeId = "mAsbg-aH9UpHgVbFUY9TH3PUBZNRd923fcxQVefWU6k";
            //string json = null;
            //string token = GetWXToken();
            //var input = new SendTempletInput();
            //input.template_id = templeId;
            //input.touser = sender.FromUserName;
            //input.topcolor = "#FF0000";
            //input.url = "http://weixin.qq.com/download";
            //input.data = new TempletMsgLoginCls()
            //{
            //    first = new TempletMsgTextCls() { value = "测试发送视频:", color = "#FF0000" },
            //    keyword1 = new TempletMsgTextCls() { value = "mediaInfo:" + locationStr, color = "#FF0000" },
            //    keyword2 = new TempletMsgTextCls() { value = "发送视频:", color = "#FF0000" },
            //    keyword3 = new TempletMsgTextCls() { value = "发送目标微信号：" + sender.FromUserName, color = "#FF0000" },
            //    remark = new TempletMsgTextCls() { value = "发送时间：" + sender.CreateTime, color = "#FF0000" },
            //};
            //var result = WxGetMethod.SendTempletMsg(input, token, ref json);


            //GYWx.Reply.NormalReply.Replytext text = new Replytext();
            //text.FromUserName = sender.ToUserName;
            //text.ToUserName = sender.FromUserName;
            //text.CreateTime = sender.CreateTime;
            //text.Content = "视频已收到";
            //return text;

            return null;
        }

        public GYWx.Reply.ReplayMsgBase ProcessVoiceMsg(GYWx.Receive.NormalMsg.VoiceMsg sender)
        {
            //string locationStr = "";
            //if (sender != null)
            //{
            //    locationStr = sender.MediaId + "|" + sender.Recognition;
            //}

            ////发送模板
            //string templeId = "mAsbg-aH9UpHgVbFUY9TH3PUBZNRd923fcxQVefWU6k";
            //string json = null;
            //string token = GetWXToken();
            //var input = new SendTempletInput();
            //input.template_id = templeId;
            //input.touser = sender.FromUserName;
            //input.topcolor = "#FF0000";
            //input.url = "http://weixin.qq.com/download";
            //input.data = new TempletMsgLoginCls()
            //{
            //    first = new TempletMsgTextCls() { value = "测试发送声音:", color = "#FF0000" },
            //    keyword1 = new TempletMsgTextCls() { value = "voiceInfo:" + locationStr, color = "#FF0000" },
            //    keyword2 = new TempletMsgTextCls() { value = "发送声音:", color = "#FF0000" },
            //    keyword3 = new TempletMsgTextCls() { value = "发送目标微信号：" + sender.FromUserName, color = "#FF0000" },
            //    remark = new TempletMsgTextCls() { value = "发送时间：" + sender.CreateTime, color = "#FF0000" },
            //};
            //var result = WxGetMethod.SendTempletMsg(input, token, ref json);

            //GYWx.Reply.NormalReply.Replytext text = new Replytext();
            //text.FromUserName = sender.ToUserName;
            //text.ToUserName = sender.FromUserName;
            //text.CreateTime = sender.CreateTime;
            //text.Content = "声音已收到";
            //return text;

            return null;
        }

        public GYWx.Reply.ReplayMsgBase ProcessBatchMsgEvent(GYWx.Receive.EventMsg.BMsgEvent sender)
        {
            return null;
        }

        public GYWx.Reply.ReplayMsgBase ProcessLocationSelectEvent(GYWx.Receive.EventMsg.LocationSelectEvent sender)
        {
            ////发送模板
            //string templeId = "mAsbg-aH9UpHgVbFUY9TH3PUBZNRd923fcxQVefWU6k";
            //string json = null;
            //string token = GetWXToken();
            //var input = new SendTempletInput();
            //input.template_id = templeId;
            //input.touser = sender.FromUserName;
            //input.topcolor = "#FF0000";
            //input.url = "http://weixin.qq.com/download";

            //string locationStr = "";
            //if (sender.SendLocationInfo != null)
            //{
            //    locationStr = sender.SendLocationInfo.Label + "|" + sender.SendLocationInfo.Location_X + "|" + sender.SendLocationInfo.Location_Y + "|" + sender.SendLocationInfo.Poiname + "|" + sender.SendLocationInfo.Scale;
            //}

            //input.data = new TempletMsgLoginCls()
            //{
            //    first = new TempletMsgTextCls() { value = "测试弹出地理位置选择器:", color = "#FF0000" },
            //    keyword1 = new TempletMsgTextCls() { value = "Event:" + sender.Event, color = "#FF0000" },
            //    keyword2 = new TempletMsgTextCls() { value = "EventKey:" + sender.EventKey, color = "#FF0000" },
            //    keyword3 = new TempletMsgTextCls() { value = "木有内容", color = "#FF0000" },
            //    remark = new TempletMsgTextCls() { value = "SendLocationInfo：" + locationStr, color = "#FF0000" },
            //};
            //var result = WxGetMethod.SendTempletMsg(input, token, ref json);

            return null;
        }

        public GYWx.Reply.ReplayMsgBase ProcessPicEvent(GYWx.Receive.EventMsg.PicEvent sender)
        {
            ////发送模板
            //string templeId = "mAsbg-aH9UpHgVbFUY9TH3PUBZNRd923fcxQVefWU6k";
            //string json = null;
            //string token = GetWXToken();
            //var input = new SendTempletInput();
            //input.template_id = templeId;
            //input.touser = sender.FromUserName;
            //input.topcolor = "#FF0000";
            //input.url = "http://weixin.qq.com/download";

            //string picListStr = "";
            //if (sender.SendPicsInfo.PickList != null)
            //{
            //    foreach (var pic in sender.SendPicsInfo.PickList)
            //    {
            //        if (pic.item != null)
            //        {
            //            picListStr += pic.item.PicMd5Sum + "|";
            //        }
            //    }
            //}

            //input.data = new TempletMsgLoginCls()
            //{
            //    first = new TempletMsgTextCls() { value = "测试弹出图片相关:", color = "#FF0000" },
            //    keyword1 = new TempletMsgTextCls() { value = "Event:" + sender.Event, color = "#FF0000" },
            //    keyword2 = new TempletMsgTextCls() { value = "EventKey:" + sender.EventKey, color = "#FF0000" },
            //    keyword3 = new TempletMsgTextCls() { value = "Count：" + sender.SendPicsInfo.Count, color = "#FF0000" },
            //    remark = new TempletMsgTextCls() { value = "ScanResult：" + picListStr, color = "#FF0000" },
            //};
            //var result = WxGetMethod.SendTempletMsg(input, token, ref json);

            return null;
        }

        public GYWx.Reply.ReplayMsgBase ProcessScancCodeEvent(GYWx.Receive.EventMsg.ScanCodeEvent sender)
        {
            ////发送模板
            //string templeId = "mAsbg-aH9UpHgVbFUY9TH3PUBZNRd923fcxQVefWU6k";
            //string json = null;
            //string token = GetWXToken();
            //var input = new SendTempletInput();
            //input.template_id = templeId;
            //input.touser = sender.FromUserName;
            //input.topcolor = "#FF0000";
            //input.url = "http://weixin.qq.com/download";
            //input.data = new TempletMsgLoginCls()
            //{
            //    first = new TempletMsgTextCls() { value = "测试扫描二维码:", color = "#FF0000" },
            //    keyword1 = new TempletMsgTextCls() { value = "Event:" + sender.Event, color = "#FF0000" },
            //    keyword2 = new TempletMsgTextCls() { value = "EventKey:" + sender.EventKey, color = "#FF0000" },
            //    keyword3 = new TempletMsgTextCls() { value = "ScanType：" + sender.ScanCodeInfo.ScanType, color = "#FF0000" },
            //    remark = new TempletMsgTextCls() { value = "ScanResult：" + sender.ScanCodeInfo.ScanResult, color = "#FF0000" },
            //};
            //var result = WxGetMethod.SendTempletMsg(input, token, ref json);

            return null;
        }

        public GYWx.Reply.ReplayMsgBase ProcessTempletMsgEvent(GYWx.Receive.EventMsg.TempletMsgEvent sender)
        {
            return null;
        }

        public GYWx.Reply.ReplayMsgBase AnnualRenewEvent(GYWx.Receive.EventMsg.VerifySuccessEvent sender)
        {
            throw new NotImplementedException();
        }

        public GYWx.Reply.ReplayMsgBase NamingVerifyFailEvent(GYWx.Receive.EventMsg.VerifyFailEvent sender)
        {
            throw new NotImplementedException();
        }

        public GYWx.Reply.ReplayMsgBase NamingVerifySuccessEvent(GYWx.Receive.EventMsg.VerifySuccessEvent sender)
        {
            throw new NotImplementedException();
        }

        public GYWx.Reply.ReplayMsgBase QualificationVerifyFailEvent(GYWx.Receive.EventMsg.VerifyFailEvent sender)
        {
            throw new NotImplementedException();
        }

        public GYWx.Reply.ReplayMsgBase QualificationVerifySuccessEvent(GYWx.Receive.EventMsg.VerifySuccessEvent sender)
        {
            throw new NotImplementedException();
        }

        public GYWx.Reply.ReplayMsgBase VerifyExpiredEvent(GYWx.Receive.EventMsg.VerifySuccessEvent sender)
        {
            throw new NotImplementedException();
        }
    }
}
