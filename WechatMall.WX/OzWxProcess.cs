using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using GYWx;
using GYWx.Common;
using GYWx.Reply.NormalReply;
using GYWx.Post;
using GYWx.Post.JsonModel.TempletMsg.Input;
using GYWx.Post.JsonModel.BatchMsg.Input;
using WechatMall.WX.Enum;
using WechatMall.WX.Dictionary;
using WechatMall.Wx.SendTempletCls;
using System.Configuration;

using GYWx.Receive;
using System.Data;
using WechatMall.WX.Model.Wx;
using System.Web;
using GYWx.Reply;
using GYLib.Base.Utils;
using GYWx.Receive.NormalMsg;
namespace WechatMall.Wx
{
    public class OzWxProcess : IWxProcess
    {

        public static string WxHelloWorld = "";

        //微信关注的网址
        public static string WXWebAddress = "";

        /// 任何方法的具体实现案例 请参照此文件的最早版本

        public string GetWXToken()
        {
            string json = null;
            return WxGetMethod.GetWxAccessToken(ref json);
        }

        /// <summary>
        /// 获取地理位置事件
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public ReplayMsgBase ProcessLcationEvent(GYWx.Receive.EventMsg.LocationEvent sender)
        {
            return null;
        }

        /// <summary>
        /// 接收用户发送链接
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public ReplayMsgBase ProcessLinkMsg(GYWx.Receive.NormalMsg.LinkMsg sender)
        {
            return GetServiceResult(sender);
            //string isSend = ConfigurationManager.AppSettings["IsPostMsgCustomerService"];
            //if (isSend == "True")
            //{
            //    //发送多客服消息
            //    try
            //    {
            //        SendCustomerServiceLazy(sender);
            //        return null;
            //        //string result = SendCustomerService(sender);
            //        //string result = HttpUtility.HtmlEncode("客官您好，浩小泽在这里恭候多时啦！\n<a href=\"http://www.oznerwater.com/lktnew/wap/mall/mallHomePage.aspx\">※了解浩泽产品></a>\n<a href=\"http://www.oznerwater.com/lktnew/wap/Member/MemberSpecial.aspx\">※了解浩泽会员权益></a>\n<a href=\"http://www.oznerwater.com/lktnew/wap/other/csoauth.aspx?flw=2\">※浩泽售后服务及建议></a>\n<a href=\"http://www.oznerwater.com/lktnew/wap/other/csoauth.aspx?flw=1\">※人工客服></a>");
            //        //return null;
            //        //NormalReply.Replytext text = new Replytext();
            //        //text.FromUserName = sender.ToUserName;
            //        //text.ToUserName = sender.FromUserName;
            //        //text.CreateTime = sender.CreateTime;
            //        //text.Content = result;
            //        //return text;
            //    }
            //    catch (Exception ex)
            //    {
            //        LogBase.LogHzLkt.LogExErr(ex);
            //        return null;
            //    }
            //}
            //else
            //{
            //    //转发自带多客服
            //    ReplayMsgBase result = new ReplayMsgBase();
            //    result.FromUserName = sender.ToUserName;
            //    result.ToUserName = sender.FromUserName;
            //    result.CreateTime = sender.CreateTime;
            //    result.MsgType = "transfer_customer_service";
            //    return result;
            //}
            //return null;
        }

        /// <summary>
        /// 接收用户发送地理位置
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public ReplayMsgBase ProcessLocationMsg(GYWx.Receive.NormalMsg.LocationMsg sender)
        {
            return GetServiceResult(sender);
            //string isSend = ConfigurationManager.AppSettings["IsPostMsgCustomerService"];
            //if (isSend == "True")
            //{
            //    //发送多客服消息
            //    try
            //    {
            //        string result = "客官您好，浩小泽在这里恭候多时啦！\n<a href=\"http://www.oznerwater.com/lktnew/wap/mall/mallHomePage.aspx\">※了解浩泽产品></a>\n<a href=\"http://www.oznerwater.com/lktnew/wap/Member/MemberSpecial.aspx\">※了解浩泽会员权益></a>\n<a href=\"http://www.oznerwater.com/lktnew/wap/other/csoauth.aspx?flw=2\">※浩泽售后服务及建议></a>\n<a href=\"http://www.oznerwater.com/lktnew/wap/other/csoauth.aspx?flw=1\">※人工客服></a>";// SendCustomerService(sender);

            //        GYWx.Reply.NormalReply.Replytext text = new Replytext();
            //        text.FromUserName = sender.ToUserName;
            //        text.ToUserName = sender.FromUserName;
            //        text.CreateTime = sender.CreateTime;
            //        text.Content = result;
            //        return text;
            //    }
            //    catch (Exception ex)
            //    {
            //        LogBase.LogHzLkt.LogExErr(ex);
            //        return null;
            //    }
            //}
            //else
            //{
            //    //转发自带多客服
            //    ReplayMsgBase result = new ReplayMsgBase();
            //    result.FromUserName = sender.ToUserName;
            //    result.ToUserName = sender.FromUserName;
            //    result.CreateTime = sender.CreateTime;
            //    result.MsgType = "transfer_customer_service";
            //    return result;
            //}
            //return null;
        }

        /// <summary>
        /// 用户与菜单交互
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public ReplayMsgBase ProcessMenuEvent(GYWx.Receive.EventMsg.MenuEvent sender)
        {
            ReplayMsgBase msg = null;
            //测试
            if (sender.Event.ToLower() == "click")
            {
                //string json = null;
                //string token = GetWXToken();
                //var input = new ReplyBMsgUserText();
                //input.touser = new string[] { sender.FromUserName };
                //input.text = new GYWx.Post.JsonModel.Cls.ContentCls() { content = "测试发送模板素材" };
                //input.msgtype = "text";

                //var result = WxGetMethod.SendBatchMsgByOpenID(input, token, ref json);

                if (sender.EventKey == "V1001_jbwz")
                {
                    GYWx.Reply.NormalReply.Replynews arictil = new Replynews();
                    arictil.FromUserName = sender.ToUserName;
                    arictil.ToUserName = sender.FromUserName;
                    arictil.CreateTime = sender.CreateTime;
                    arictil.MsgType = "news";
                    arictil.ArticleCount = "4";
                    arictil.Articles = new ArticleCls()
                    {
                        item = new NewsCls[]{
                            new NewsCls(){
                                Url="http://www.oznerwater.com/lktnew/wap/wxshareoauth.aspx?gourl=http://www.oznerwater.com/lktnew/wx/wxhtml/A1.html",
                                Title="我和健康有个约惠",
                                Description="我和健康有个约惠",
                                PicUrl="http://www.oznerwater.com/lktnew/upload/wxts/0.jpg"
                            },
                             new NewsCls(){
                                Url="http://www.oznerwater.com/lktnew/wap/wxshareoauth.aspx?gourl=http://www.oznerwater.com/lktnew/wx/wxhtml/A2.html",
                                Title="第一次跟他接吻快亲上去的时候他突然",
                                Description="",
                                PicUrl="http://www.oznerwater.com/lktnew/upload/wxts/1.jpg"
                            },
                             new NewsCls(){
                                Url="http://www.oznerwater.com/lktnew/wap/wxshareoauth.aspx?gourl=http://www.oznerwater.com/lktnew/wx/wxhtml/A3.html",
                                Title="彭麻麻最近拍了一个广告片，震惊国人！",
                                Description="",
                                PicUrl="http://www.oznerwater.com/lktnew/upload/wxts/2.jpg"
                            },
                             new NewsCls(){
                                Url="http://www.oznerwater.com/lktnew/wap/wxshareoauth.aspx?gourl=http://www.oznerwater.com/lktnew/wx/wxhtml/A4.html",
                                Title="惊呆了！震动是吃哪补哪！养生达人私藏集锦！",
                                Description="",
                                PicUrl="http://www.oznerwater.com/lktnew/upload/wxts/3.jpg"
                            }
                        }
                    };
                    return arictil;
                }
                else
                {
                    GYWx.Reply.NormalReply.Replytext text = new Replytext();
                    text.FromUserName = sender.ToUserName;
                    text.ToUserName = sender.FromUserName;
                    text.CreateTime = sender.CreateTime;
                    text.Content = "点击clike测试:";
                    return text;
                }

            }
            else
            {
                return msg;
            }
        }

        /// <summary>
        /// 接收用户发送图片
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public ReplayMsgBase ProcessPicMsg(GYWx.Receive.NormalMsg.PicMsg sender)
        {
            return GetServiceResult(sender);
            //string isSend = ConfigurationManager.AppSettings["IsPostMsgCustomerService"];
            //if (isSend == "True")
            //{
            //    //发送多客服消息
            //    try
            //    {
            //        SendCustomerServiceLazy(sender);
            //        return null;
            //        //string result = HttpUtility.HtmlEncode("客官您好，浩小泽在这里恭候多时啦！\n<a href=\"http://www.oznerwater.com/lktnew/wap/mall/mallHomePage.aspx\">※了解浩泽产品></a>\n<a href=\"http://www.oznerwater.com/lktnew/wap/Member/MemberSpecial.aspx\">※了解浩泽会员权益></a>\n<a href=\"http://www.oznerwater.com/lktnew/wap/other/csoauth.aspx?flw=2\">※浩泽售后服务及建议></a>\n<a href=\"http://www.oznerwater.com/lktnew/wap/other/csoauth.aspx?flw=1\">※人工客服></a>");
            //        //string result = SendCustomerService(sender);
            //        //return null;
            //        //NormalReply.Replytext text = new Replytext();
            //        //text.FromUserName = sender.ToUserName;
            //        //text.ToUserName = sender.FromUserName;
            //        //text.CreateTime = sender.CreateTime;
            //        //text.Content = result;
            //        //return text;
            //    }
            //    catch (Exception ex)
            //    {
            //        LogBase.LogHzLkt.LogExErr(ex);
            //        return null;
            //    }
            //}
            //else
            //{
            //    //转发自带多客服
            //    ReplayMsgBase result = new ReplayMsgBase();
            //    result.FromUserName = sender.ToUserName;
            //    result.ToUserName = sender.FromUserName;
            //    result.CreateTime = sender.CreateTime;
            //    result.MsgType = "transfer_customer_service";
            //    return result;
            //}

            ////if (string.IsNullOrWhiteSpace(result))
            ////{
            ////    result = "消息已收到！";
            ////}

            ////NormalReply.Replytext text = new Replytext();
            ////text.FromUserName = sender.ToUserName;
            ////text.ToUserName = sender.FromUserName;
            ////text.CreateTime = sender.CreateTime;
            ////text.Content = result;
            ////return text;
            //return null;
        }

        /// <summary>
        /// 用户关注 
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public ReplayMsgBase ProcessSubscribeEvent2(GYWx.Receive.EventMsg.SubscribeEvent2 sender)
        {
            // LogBase.LogHzLkt.LogInfo(string.Format("更新微信关注状态{0} ", sender.FromUserName), GYLog.DataAccess.Define.ELogType.Info);

            #region 关注
            if (sender.Event == "subscribe")
            {
                bool isNewSub = false;
                string param = "";

                UpdateSubLazy(sender.FromUserName, (int)EnumWxSubscribe.Subscribe);

                #region 扫描二维码关注
                if (!string.IsNullOrEmpty(sender.EventKey) && sender.EventKey.StartsWith("qrscene_"))
                {
                    param = sender.EventKey.Substring("qrscene_".Length);
                    Dictionary<string, object> dict = new Dictionary<string, object>();
                    dict.Add(Dict_Fct_MemSubscribe.QrScene, param);
                    dict.Add(Dict_Fct_MemSubscribe.WxOpenId, sender.FromUserName);

                    ThreadPool.QueueUserWorkItem(_CreateMemSubscribe, dict);
                    //标记为第一次扫二维码关注
                    isNewSub = true;


                    ////发送模板
                    //string templeId = "mAsbg-aH9UpHgVbFUY9TH3PUBZNRd923fcxQVefWU6k";
                    //string json = null;
                    //string token = GetWXToken();
                    //var input = new SendTempletInput();
                    //input.template_id = templeId;
                    //input.touser = sender.FromUserName;
                    //input.topcolor = "#FF0000";
                    //input.url = "http://weixin.qq.com/download";
                    //input.data = new PaySuccessedMsgCls()
                    //{
                    //    first = new TempletMsgTextCls() { value = "测试数据微信扫二维码关注", color = "#FF0000" },
                    //    keyword1 = new TempletMsgTextCls() { value = "参数：" + param, color = "#FF0000" },
                    //    keyword2 = new TempletMsgTextCls() { value = "连接Url（测试：<a href=\"www.baidu.com\">测试</a>） :" + input.url, color = "#FF0000" },
                    //    keyword3 = new TempletMsgTextCls() { value = "发送目标微信号：" + sender.FromUserName, color = "#FF0000" },
                    //    remark = new TempletMsgTextCls() { value = "发送时间：" + sender.CreateTime, color = "#FF0000" },
                    //};
                    //var result = WxGetMethod.SendTempletMsg(input, token, ref json);
                }
                #endregion

                #region 特殊推送（提货卡用户或者小龙虾分期付款用户）
                //int id = 0;
                //string memo = "";
                //string msg0Url = "";
                //string msg0PicUrl = "";
                ////TODO 用户关注 
                ////string strSendSpecailMsg = LogicWx.GetSubscribeMsg(sender.FromUserName, out id, out memo, out msg0Url, out msg0PicUrl);
                //string strSendSpecailMsg = "";
                //if (!string.IsNullOrEmpty(strSendSpecailMsg) && id > 0)
                //{
                //    //小龙虾分期付款客户关注后推送图文消息
                //    if ("分期付款活动".Equals(memo))
                //    {
                //        //更新消息状态
                //        UpdateSendMsgStateLazy(id);
                //    }
                //    else //提货卡用户关注后推送文本消息
                //    {
                //        //发送文本消息
                //        Replytext text = new Replytext();
                //        text.FromUserName = sender.ToUserName;
                //        text.ToUserName = sender.FromUserName;
                //        text.CreateTime = sender.CreateTime;
                //        text.Content = HttpUtility.HtmlEncode(strSendSpecailMsg);
                //        // System.Web.HttpContext.Current.Server.HtmlEncode(strSendSpecailMsg);


                //        //更新消息状态
                //        UpdateSendMsgStateLazy(id);

                //        return text;
                //    }
                //}
                #endregion

                //获取图文消息
                //DataTable dt = DalWXMsgCommon.GetDefaultWXMsg();

                DataTable dt = null;
                //没有图文消息 使用默认的文本推送
                if (dt == null || dt.Rows.Count == 0)
                {
                    #region 默认的文本推送
                    Replytext text = new Replytext();
                    text.FromUserName = sender.ToUserName;
                    text.ToUserName = sender.FromUserName;
                    text.CreateTime = sender.CreateTime;

                    string strSend = WxHelloWorld;// ConfigurationManager.AppSettings["WXHellowWorld"];
                    if (string.IsNullOrEmpty(strSend))
                    {
                        strSend = @"感谢关注“浩泽净水科技”官方微信，<a href='http://www.oznerwater.com/wapnew/airport/LotteryCoupon.aspx'>点击链接</a>注册成为浩泽会员，万只智能杯任性送";
                    }
                    else
                    {
                        strSend = strSend.Replace("[", "<").Replace("]", ">");
                    }
                    text.Content = HttpUtility.HtmlEncode(strSend);
                    //System.Web.HttpContext.Current.Server.HtmlEncode(strSend);

                    //  LogBase.LogHzLkt.LogInfo(string.Format("更新微信关注状态{0} ", sender.FromUserName), GYLog.DataAccess.Define.ELogType.Info);

                    return text;
                    #endregion
                }
                else
                {
                    #region 推送图文消息

                    WxMsgModel msgModel = GYLib.Base.Utils.ModelUtils<WxMsgModel>.GetModel(dt);

                    if (!string.IsNullOrEmpty(msgModel.Content))
                    {
                        //int paramucode = ConvertUtils.ConvertToInt(param);
                        //if (paramucode <= 0)
                        //{
                        //    //凭微信openid获取代理商推荐码
                        //    DataTable dtMember = DalMemberCommon.GetMemberInfoByWeiXinOpenId(sender.FromUserName);
                        //    if (dtMember != null && dtMember.Rows.Count > 0)
                        //    {
                        //        paramucode = ConvertUtils.ConvertToInt(dtMember.Rows[0][Dict_Fct_Member.ParentCode]);

                        //    }
                        //}

                        ////获取H5渠道商定制化的图片路径
                        //DataTable dtShare = DalAgentMember.QueryAgentAtvByParentUcodeKeyValue(paramucode, (int)EnumAgentAtvKeyEnum.WXSubMsgFristBgText);
                        //if (dtShare == null || dtShare.Rows.Count == 0)
                        //{
                        //    dtShare = DalAgentMember.QueryAgentAtvByWxOpenIdKeyValue(sender.FromUserName, (int)EnumAgentAtvKeyEnum.WXSubMsgFristBgText);
                        //}

                        ContentModel content = new ContentModel();
                        XmlHelper.LoadFromXml(msgModel.Content, content);
                        msgModel.ContentModel = content;
                        if (content != null && content.News != null)
                        {
                            List<NewsCls> news = new List<NewsCls>();
                            for (int index = 0; index < content.News.Length; index++)
                            {
                                NewsModel model = content.News[index];
                                //如果有强制跳转则以强制跳转为准
                                if (!string.IsNullOrWhiteSpace(model.ForcedUrl))
                                {
                                    model.Url = model.ForcedUrl;
                                }

                                NewsModel newcls = model;
                                if (!string.IsNullOrWhiteSpace(newcls.Title))
                                {

                                    //图文消息第一个图片的链接
                                    if (index == 0)
                                    {
                                        //小龙虾分期付款客户的图片和跳转地址
                                        //if ("分期付款活动".Equals(memo))
                                        //{
                                        //    newcls.Title = strSendSpecailMsg;
                                        //    newcls.Url = msg0Url;
                                        //    newcls.PicUrl = msg0PicUrl;
                                        //}
                                        //else if (dtShare != null && dtShare.Rows.Count > 0)  //H5渠道商定制的图片路径（跳转地址为领取水探头页面.固定）
                                        //{
                                        //    //string url = ConvertUtils.ConvertToString(dtShare.Rows[0][Dict_Fct_AgentAtv.AtvContent]);
                                        //    //newcls.PicUrl = ConfigurationManager.AppSettings["WXWebAddress"] + ".." + url;
                                        //}
                                        //else
                                        //{
                                        //    newcls.PicUrl = ConfigurationManager.AppSettings["WXWebAddress"] + ".." + newcls.PicUrl;
                                        //}
                                    }
                                    else
                                    {
                                        newcls.PicUrl = WXWebAddress + ".." + newcls.PicUrl;
                                        //ConfigurationManager.AppSettings["WXWebAddress"] + ".." + newcls.PicUrl;
                                    }

                                    //由于数据关键字 和XML序列化的问题
                                    //移除所有字段的左括号和右括号
                                    //newcls.Title = LogicGrougPickUp.RemoveSqlSpecial(newcls.Title);
                                    //newcls.Url = LogicGrougPickUp.RemoveSqlSpecial(newcls.Url);
                                    //newcls.PicUrl = LogicGrougPickUp.RemoveSqlSpecial(newcls.PicUrl);
                                    //newcls.Description = LogicGrougPickUp.RemoveSqlSpecial(newcls.Description);
                                    //newcls.ForcedUrl = LogicGrougPickUp.RemoveSqlSpecial(newcls.ForcedUrl);

                                    //对URL加密
                                    newcls.Url = HttpUtility.HtmlEncode(newcls.Url);
                                    newcls.PicUrl = HttpUtility.HtmlEncode(newcls.PicUrl);
                                    newcls.ForcedUrl = HttpUtility.HtmlEncode(newcls.ForcedUrl);

                                    news.Add(newcls);
                                }
                            }

                            if (news.Count > 0)
                            {
                                //推送图文
                                GYWx.Reply.NormalReply.Replynews arictil = new Replynews();
                                arictil.FromUserName = sender.ToUserName;
                                arictil.ToUserName = sender.FromUserName;
                                arictil.CreateTime = sender.CreateTime;
                                arictil.MsgType = "news";
                                arictil.ArticleCount = news.Count + "";
                                arictil.Articles = new ArticleCls()
                                {
                                    item = news.ToArray()
                                };
                                return arictil;
                            }
                        }
                    }
                    #endregion
                }
            }
            #endregion

            #region 取消关注
            //取消订阅
            if (sender.Event == "unsubscribe")
            {
                //int result = DalWXMsgCommon.UpdateMemberSubscribe(sender.FromUserName, (int)EnumWxSubscribe.UnSubscribe);
                UpdateSubLazy(sender.FromUserName, (int)EnumWxSubscribe.UnSubscribe);
                //if (result < 0)
                //{
                //    LogBase.LogHzLkt.LogInfo(string.Format("更新微信取消关注状态失败{0} {1}", sender.FromUserName, result), GYLog.DataAccess.Define.ELogType.Info);
                //}

                //NormalReply.Replytext text = new Replytext();
                //text.FromUserName = sender.ToUserName;
                //text.ToUserName = sender.FromUserName;
                //text.CreateTime = sender.CreateTime;
                //text.Content = "Hello World";
                //if (!string.IsNullOrEmpty(sender.EventKey) && sender.EventKey.StartsWith("qrscene_"))
                //{
                //    string param = sender.EventKey.Substring("qrscene_".Length);
                //    ///扫描二维码关注
                //    ///
                //    text.Content = "Hello World(取消关注)";

                //    //发送模板
                //    string templeId = "mAsbg-aH9UpHgVbFUY9TH3PUBZNRd923fcxQVefWU6k";
                //    string json = null;
                //    string token = GetWXToken();
                //    var input = new SendTempletInput();
                //    input.template_id = templeId;
                //    input.touser = sender.FromUserName;
                //    input.topcolor = "#FF0000";
                //    input.url = "http://weixin.qq.com/download";
                //    input.data = new PaySuccessedMsgCls()
                //    {
                //        first = new TempletMsgTextCls() { value = "测试取消关注", color = "#FF0000" },
                //        keyword1 = new TempletMsgTextCls() { value = "参数：" + param, color = "#FF0000" },
                //        keyword2 = new TempletMsgTextCls() { value = "连接Url（测试：<a href=\"www.baidu.com\">测试</a>） :" + input.url, color = "#FF0000" },
                //        keyword3 = new TempletMsgTextCls() { value = "发送目标微信号：" + sender.FromUserName, color = "#FF0000" },
                //        remark = new TempletMsgTextCls() { value = "发送时间：" + sender.CreateTime, color = "#FF0000" },
                //    };
                //    var result = WxGetMethod.SendTempletMsg(input, token, ref json);
                //}

                //return text;
            }

            #endregion

            #region 关注后再扫描
            //扫描二维码
            if (sender.EventKey == "SCAN")
            {

            }
            #endregion

            return null;
        }

        /// <summary>
        /// 接收用户发送文字
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public ReplayMsgBase ProcessTextMsg(GYWx.Receive.NormalMsg.TextMsg sender)
        {
            return GetServiceResult(sender);
            //string isSend = ConfigurationManager.AppSettings["IsPostMsgCustomerService"];
            //if (isSend == "True")
            //{
            //    //发送多客服消息
            //    try
            //    {
            //        SendCustomerServiceLazy(sender);
            //        return null;
            //        //string result = HttpUtility.HtmlEncode("客官您好，浩小泽在这里恭候多时啦！\n<a href=\"http://www.oznerwater.com/lktnew/wap/mall/mallHomePage.aspx\">※了解浩泽产品></a>\n<a href=\"http://www.oznerwater.com/lktnew/wap/Member/MemberSpecial.aspx\">※了解浩泽会员权益></a>\n<a href=\"http://www.oznerwater.com/lktnew/wap/other/csoauth.aspx?flw=2\">※浩泽售后服务及建议></a>\n<a href=\"http://www.oznerwater.com/lktnew/wap/other/csoauth.aspx?flw=1\">※人工客服></a>");
            //        //string result = SendCustomerService(sender);
            //        //return null;
            //        //NormalReply.Replytext text = new Replytext();
            //        //text.FromUserName = sender.ToUserName;
            //        //text.ToUserName = sender.FromUserName;
            //        //text.CreateTime = sender.CreateTime;
            //        //text.Content = result;
            //        //return text;
            //    }
            //    catch (Exception ex)
            //    {
            //        LogBase.LogHzLkt.LogExErr(ex);
            //        return null;
            //    }
            //}
            //else
            //{
            //    //转发自带多客服
            //    ReplayMsgBase result = new ReplayMsgBase();
            //    result.FromUserName = sender.ToUserName;
            //    result.ToUserName = sender.FromUserName;
            //    result.CreateTime = sender.CreateTime;
            //    result.MsgType = "transfer_customer_service";
            //    return result;
            //}

            ////string result = string.Empty;

            ////try
            ////{
            ////    string strUrl = @"http://dkf.ozner.net/service/wxcustomer.aspx";
            ////    string date = XmlHelper.CreateXml(sender);
            ////    result = WebClientHelper.CallPost(strUrl, date, Encoding.UTF8);

            ////}
            ////catch (Exception ex)
            ////{
            ////    OznerMall.Common.Log.LogBase.LogHzLkt.LogExErr(ex);
            ////}


            ////if (string.IsNullOrWhiteSpace(result))
            ////{
            ////    result = "消息已收到！";
            ////}

            ////NormalReply.Replytext text = new Replytext();
            ////text.FromUserName = sender.ToUserName;
            ////text.ToUserName = sender.FromUserName;
            ////text.CreateTime = sender.CreateTime;
            ////text.Content = result;
            ////return text;
            //return null;
        }

        /// <summary>
        /// 接收用户发送音频
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public ReplayMsgBase ProcessVideoMsg(GYWx.Receive.NormalMsg.VideoMsg sender)
        {
            return GetServiceResult(sender);
            //string isSend = ConfigurationManager.AppSettings["IsPostMsgCustomerService"];
            //if (isSend == "True")
            //{
            //    //发送多客服消息
            //    try
            //    {
            //        SendCustomerServiceLazy(sender);
            //        return null;
            //        //string result = HttpUtility.HtmlEncode("客官您好，浩小泽在这里恭候多时啦！\n<a href=\"http://www.oznerwater.com/lktnew/wap/mall/mallHomePage.aspx\">※了解浩泽产品></a>\n<a href=\"http://www.oznerwater.com/lktnew/wap/Member/MemberSpecial.aspx\">※了解浩泽会员权益></a>\n<a href=\"http://www.oznerwater.com/lktnew/wap/other/csoauth.aspx?flw=2\">※浩泽售后服务及建议></a>\n<a href=\"http://www.oznerwater.com/lktnew/wap/other/csoauth.aspx?flw=1\">※人工客服></a>");
            //        //string result = SendCustomerService(sender);
            //        //return null;
            //        //NormalReply.Replytext text = new Replytext();
            //        //text.FromUserName = sender.ToUserName;
            //        //text.ToUserName = sender.FromUserName;
            //        //text.CreateTime = sender.CreateTime;
            //        //text.Content = result;
            //        //return text;
            //    }
            //    catch (Exception ex)
            //    {
            //        LogBase.LogHzLkt.LogExErr(ex);
            //        return null;
            //    }
            //}
            //else
            //{
            //    //转发自带多客服
            //    ReplayMsgBase result = new ReplayMsgBase();
            //    result.FromUserName = sender.ToUserName;
            //    result.ToUserName = sender.FromUserName;
            //    result.CreateTime = sender.CreateTime;
            //    result.MsgType = "transfer_customer_service";
            //    return result;
            //}
            //return null;
        }

        /// <summary>
        /// 接收用户发送语音
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public ReplayMsgBase ProcessVoiceMsg(GYWx.Receive.NormalMsg.VoiceMsg sender)
        {
            return GetServiceResult(sender);
            //string isSend = ConfigurationManager.AppSettings["IsPostMsgCustomerService"];
            //if (isSend == "True")
            //{
            //    //发送多客服消息
            //    try
            //    {
            //        SendCustomerServiceLazy(sender);
            //        return null;
            //        //string result = HttpUtility.HtmlEncode("客官您好，浩小泽在这里恭候多时啦！\n<a href=\"http://www.oznerwater.com/lktnew/wap/mall/mallHomePage.aspx\">※了解浩泽产品></a>\n<a href=\"http://www.oznerwater.com/lktnew/wap/Member/MemberSpecial.aspx\">※了解浩泽会员权益></a>\n<a href=\"http://www.oznerwater.com/lktnew/wap/other/csoauth.aspx?flw=2\">※浩泽售后服务及建议></a>\n<a href=\"http://www.oznerwater.com/lktnew/wap/other/csoauth.aspx?flw=1\">※人工客服></a>");
            //        //string result = SendCustomerService(sender);
            //        //return null;
            //        //NormalReply.Replytext text = new Replytext();
            //        //text.FromUserName = sender.ToUserName;
            //        //text.ToUserName = sender.FromUserName;
            //        //text.CreateTime = sender.CreateTime;
            //        //text.Content = result;
            //        //return text;
            //    }
            //    catch (Exception ex)
            //    {
            //        LogBase.LogHzLkt.LogExErr(ex);
            //        return null;
            //    }
            //}
            //else
            //{
            //    //转发自带多客服
            //    ReplayMsgBase result = new ReplayMsgBase();
            //    result.FromUserName = sender.ToUserName;
            //    result.ToUserName = sender.FromUserName;
            //    result.CreateTime = sender.CreateTime;
            //    result.MsgType = "transfer_customer_service";
            //    return result;
            //}

            //return null;
        }

        /// <summary>
        /// 群发消息
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public ReplayMsgBase ProcessBatchMsgEvent(GYWx.Receive.EventMsg.BMsgEvent sender)
        {
            return null;
        }

        /// <summary>
        /// 接收用户选择地理位置
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public ReplayMsgBase ProcessLocationSelectEvent(GYWx.Receive.EventMsg.LocationSelectEvent sender)
        {
            return null;
        }

        /// <summary>
        /// 接收用户发送图片
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public ReplayMsgBase ProcessPicEvent(GYWx.Receive.EventMsg.PicEvent sender)
        {
            return null;
        }

        /// <summary>
        /// 二维码扫描
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public ReplayMsgBase ProcessScancCodeEvent(GYWx.Receive.EventMsg.ScanCodeEvent sender)
        {
            //重复关注
            //int result = DalWXMsgCommon.UpdateMemberSubscribe(sender.FromUserName, (int)EnumWxSubscribe.Subscribe);
            UpdateSubLazy(sender.FromUserName, (int)EnumWxSubscribe.Subscribe);
            //if (result < 0)
            //{
            //    LogBase.LogHzLkt.LogInfo(string.Format("更新微信关注状态失败{0} {1}", sender.FromUserName, result), GYLog.DataAccess.Define.ELogType.Info);
            //}

            return null;
        }

        /// <summary>
        /// 模板消息
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public ReplayMsgBase ProcessTempletMsgEvent(GYWx.Receive.EventMsg.TempletMsgEvent sender)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public ReplayMsgBase AnnualRenewEvent(GYWx.Receive.EventMsg.VerifySuccessEvent sender)
        {
            return null;
        }

        #region 自定义语义
        /// <summary>
        /// 语义命名验证失败
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public ReplayMsgBase NamingVerifyFailEvent(GYWx.Receive.EventMsg.VerifyFailEvent sender)
        {
            return null;
        }

        /// <summary>
        /// 语义命名验证成功
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public ReplayMsgBase NamingVerifySuccessEvent(GYWx.Receive.EventMsg.VerifySuccessEvent sender)
        {
            return null;
        }

        /// <summary>
        ///  语义验证失败
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public ReplayMsgBase QualificationVerifyFailEvent(GYWx.Receive.EventMsg.VerifyFailEvent sender)
        {
            return null;
        }

        /// <summary>
        /// 语义验证成功
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public ReplayMsgBase QualificationVerifySuccessEvent(GYWx.Receive.EventMsg.VerifySuccessEvent sender)
        {
            return null;
        }

        /// <summary>
        /// 验证自定义消息
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        public ReplayMsgBase VerifyExpiredEvent(GYWx.Receive.EventMsg.VerifySuccessEvent sender)
        {
            return null;
        }
        #endregion

        #region 关注
        public void _CreateMemSubscribe(object insertDict)
        {
            try
            {
                //todo  增加关注
                //DalMemSubscribe.CreateSubscribeInfo((Dictionary<string, object>)insertDict);
            }
            catch (Exception ex)
            {
                //LogBase.LogHzLkt.LogExErr(ex, ELogExLevel.Higher);
            }
        }
        #endregion

        #region 多客服帮手

        /// <summary>
        /// 发送微信支付成功的信息推送
        /// </summary>
        /// <param name="wxopenid"></param>
        /// <returns></returns>
        public void SendCustomerServiceLazy(ReceiveMsgBase msg)
        {
            ParameterizedThreadStart parStart = new ParameterizedThreadStart(SendCustomerService);
            Thread oThread = new Thread(parStart);
            oThread.Start(msg);
        }

        /// <summary>
        /// 消息转发多客服
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="sendKey"></param>
        private void SendCustomerService(Object obj)
        {
            string result = string.Empty;
            string date = string.Empty;
            string strUrl = string.Empty;
            try
            {
                ReceiveMsgBase msg = obj as ReceiveMsgBase;

                if (msg == null)
                {
                    return;
                }


                if (msg is LinkMsg)
                {
                    LinkMsg link = msg as LinkMsg;
                    link.Title = "<![CDATA[" + link.Title + "]]>";
                    link.Description = "<![CDATA[" + link.Description + "]]>";
                    link.Url = "<![CDATA[" + link.Url + "]]>";
                }
                else if (msg is LocationMsg)
                {
                    LocationMsg link = msg as LocationMsg;
                    link.Location_X = "<![CDATA[" + link.Location_X + "]]>";
                    link.Location_Y = "<![CDATA[" + link.Location_Y + "]]>";
                    link.Scale = "<![CDATA[" + link.Scale + "]]>";
                    link.Label = "<![CDATA[" + link.Label + "]]>";
                }
                else if (msg is PicMsg)
                {
                    PicMsg link = msg as PicMsg;
                    link.PicUrl = "<![CDATA[" + link.PicUrl + "]]>";
                    link.MediaId = "<![CDATA[" + link.MediaId + "]]>";
                }
                else if (msg is TextMsg)
                {
                    TextMsg link = msg as TextMsg;
                    link.Content = "<![CDATA[" + link.Content + "]]>";
                }
                else if (msg is VideoMsg)
                {
                    VideoMsg link = msg as VideoMsg;
                    link.ThumbMediaId = "<![CDATA[" + link.ThumbMediaId + "]]>";
                    link.MediaId = "<![CDATA[" + link.MediaId + "]]>";
                }
                else if (msg is VoiceMsg)
                {
                    VoiceMsg link = msg as VoiceMsg;
                    link.MediaId = "<![CDATA[" + link.MediaId + "]]>";
                    link.Format = "<![CDATA[" + link.Format + "]]>";
                    link.Recognition = "<![CDATA[" + link.Recognition + "]]>";
                }

                //try
                //{
                //    DataTable dt = DalMemberCommon.GetMemberInfoByWeiXinOpenId(msg.FromUserName);
                //    if (dt != null && dt.Rows.Count > 0)
                //    {
                date = msg.ToXml();
                strUrl = @"http://dkf.ozner.net/service/wxcustomer.aspx";
            }
            catch (Exception ex)
            {
                string errorinfo = "错误堆栈:" + ex.StackTrace;

                if (obj != null)
                {
                    string addition = "";

                    string baseName = obj.GetType().Name;
                    if (obj is ReceiveMsgBase)
                    {
                        ReceiveMsgBase rmb = obj as ReceiveMsgBase;
                        Type type = rmb.GetType();

                        if (rmb is LinkMsg)
                        {
                            LinkMsg link = rmb as LinkMsg;
                            addition = link.Title + "|" + link.Description + "|" + link.Url;
                        }
                        else if (rmb is LocationMsg)
                        {
                            LocationMsg link = rmb as LocationMsg;
                            addition = link.Location_X + "|" + link.Location_Y + "|" + link.Scale + "|" + link.Label;
                        }
                        else if (rmb is PicMsg)
                        {
                            PicMsg link = rmb as PicMsg;
                            addition = link.PicUrl + "|" + link.MediaId;
                        }
                        else if (rmb is TextMsg)
                        {
                            TextMsg link = rmb as TextMsg;
                            addition = link.Content;
                        }
                        else if (rmb is VideoMsg)
                        {
                            VideoMsg link = rmb as VideoMsg;
                            addition = link.MediaId + "|" + link.ThumbMediaId;
                        }
                        else if (rmb is VoiceMsg)
                        {
                            VoiceMsg link = rmb as VoiceMsg;
                            addition = link.MediaId + "|" + link.Format + "|" + link.Recognition;
                        }
                    }
                    errorinfo = "类型：" + obj.GetType().Name + " 额外信息:" + addition + " 错误信息：" + errorinfo;
                }

                //LogBase.LogHzLkt.LogExErr("HzLktSite", "OznerMall", "SendCustomerService", errorinfo, ELogExLevel.Higher, errorinfo);
            }

            try
            {
                result = WebClientHelper.CallPost(strUrl, date, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                //LogBase.LogHzLkt.LogExErr(ex, ELogExLevel.Middle);
            }

        }
        #endregion

        #region 特殊推送

        /// <summary>
        /// 更新特殊推送状态
        /// </summary>
        /// <param name="id"></param>
        public void UpdateSendMsgStateLazy(int id)
        {
            ParameterizedThreadStart parStart = new ParameterizedThreadStart(UpdateSendMsgState);
            Thread oThread = new Thread(parStart);
            oThread.Start(id);
        }

        /// <summary>
        ///  更新特殊推送状态
        /// </summary>
        /// <param name="obj"></param>
        private void UpdateSendMsgState(object obj)
        {
            try
            {
                int id = ConvertUtils.ConvertToInt(obj);
                if (id > 0)
                {
                    //TODO 更新特殊推送状态
                    //LogicWx.UpdateSubscribeMsgState(id, 1, "");
                }
            }
            catch (Exception ex)
            {
                //LogBase.LogHzLkt.LogExErr(ex, ELogExLevel.Higher);
            }
        }
        #endregion

        #region 测试关注

        /// <summary>
        /// 更新关注状态
        /// </summary>
        /// <param name="id"></param>
        public void UpdateSubLazy(string wxopenid, int state)
        {
            //object[] p = new object[2];
            //p[0] = wxopenid;
            //p[1] = state;
            //ParameterizedThreadStart parStart = new ParameterizedThreadStart(UpdateSub);
            //Thread oThread = new Thread(parStart);
            //oThread.Start(p);
            try
            {
                //TODO  更新关注状态
                //LogicMemberBase.UpdateSubInfo(wxopenid, state);
            }
            catch (Exception ex)
            {
                //LogBase.LogHzLkt.LogExErr(ex, ELogExLevel.Lower);
            }
        }

        /// <summary>
        ///  更新关注状态
        /// </summary>
        /// <param name="obj"></param>
        private void UpdateSub(object obj)
        {
            try
            {
                object[] param = obj as object[];
                string wxopendid;
                int state = 0;

                if (param == null || param.Length != 2)
                {
                    return;
                }

                wxopendid = param[0] as string;
                state = ConvertUtils.ConvertToInt(param[1]);
                if (string.IsNullOrWhiteSpace(wxopendid))
                {
                    return;
                }
                //TODO 更新关注状态
                //LogicMemberBase.UpdateSubInfo(wxopendid, state);
            }
            catch (Exception ex)
            {
                //LogBase.LogHzLkt.LogExErr(ex, ELogExLevel.Higher);
            }
        }

        #endregion

        public ReplayMsgBase GetServiceResult(ReceiveMsgBase sender)
        {
            //todo  暂时没用这个功能
            int sendtype = 0;// ConvertUtils.ConvertToInt(ConfigurationManager.AppSettings["IsPostMsgCustomerService"]);

            switch (sendtype)
            {
                case 0:
                    //无反馈
                    return null;
                    break;
                //case 1:                     #region  update by xxb  2017/07/17  因需要对接外部的udesk客服回复，所以撤销此自动回复
                //    //反馈固定语句
                //    Replytext text = new Replytext();
                //    text.FromUserName = sender.ToUserName;
                //    text.ToUserName = sender.FromUserName;
                //    text.CreateTime = sender.CreateTime;
                //    string config = ConvertUtils.ConvertToString(ConfigurationManager.AppSettings["CustomerServiceMsg"]);
                //    if (!string.IsNullOrWhiteSpace(config))
                //    {
                //        text.Content = HttpUtility.HtmlEncode(string.Format(config.Replace("[", "<").Replace("]", ">"), sender.FromUserName));
                //        return text;
                //    }
                //    else
                //    {
                //        return null;
                //    }
                //    break;
                case 2:
                    //反馈转接微信自带多客服
                    ReplayMsgBase result = new ReplayMsgBase();
                    result.FromUserName = sender.ToUserName;
                    result.ToUserName = sender.FromUserName;
                    result.CreateTime = sender.CreateTime;
                    result.MsgType = "transfer_customer_service";
                    return result;
                    break;
                case 3:
                    //反馈转借麦信多客服
                    SendCustomerServiceLazy(sender);
                    return null;
                default:
                    break;
            }

            return null;

        }

    }
}
