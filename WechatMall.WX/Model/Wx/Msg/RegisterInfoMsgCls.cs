using GYWx.Post.JsonModel.TempletMsg.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatMall.WX.Model.Wx.Msg
{
    public class RegisterInfoMsgCls : TempletMsgBaseCls
    {

        //{{first.DATA}}
        //注册人：{{keyword1.DATA}}
        //注册号码：{{keyword2.DATA}}
        //注册时间：{{keyword3.DATA}}
        //{{remark.DATA}}

        /// <summary>
        /// 标题
        /// </summary>
        public TempletMsgTextCls first { get; set; }
        /// <summary>
        /// 注册人
        /// </summary>
        public TempletMsgTextCls keyword1 { get; set; }
        /// <summary>
        /// 注册号码
        /// </summary>
        public TempletMsgTextCls keyword2 { get; set; }
        /// <summary>
        /// 注册时间
        /// </summary>
        public TempletMsgTextCls keyword3 { get; set; }
        /// <summary>
        /// 结尾
        /// </summary>
        public TempletMsgTextCls remark { get; set; }
    }
}
