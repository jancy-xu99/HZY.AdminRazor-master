using GYWx.Post.JsonModel.TempletMsg.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatMall.WX.Model.Wx.Msg
{
    public class NewConsultationMsgCls : TempletMsgBaseCls
    {
        ///O-YCKUNVwAX-JNb1-hofliyJ0xgqLXweLd3YLMmbmYg

        /// <summary>
        /// 标题
        /// </summary>
        public TempletMsgTextCls first { get; set; }
        /// <summary>
        /// 咨询名称
        /// </summary>
        public TempletMsgTextCls keyword1 { get; set; }
        /// <summary>
        /// 消息回复
        /// </summary>
        public TempletMsgTextCls keyword2 { get; set; }
        /// <summary>
        /// 结尾
        /// </summary>
        public TempletMsgTextCls remark { get; set; }
    }
}
