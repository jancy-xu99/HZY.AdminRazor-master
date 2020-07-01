using GYWx.Post.JsonModel.TempletMsg.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatMall.WX.Model.Wx.Msg
{
    public class RemindRedPackMsgCls : TempletMsgBaseCls
    {

        //{{first.DATA}}
        //领取金额：{{keyword1.DATA}}
        //领取时间：{{keyword2.DATA}}
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
        /// 结尾
        /// </summary>
        public TempletMsgTextCls remark { get; set; }
    }
}
