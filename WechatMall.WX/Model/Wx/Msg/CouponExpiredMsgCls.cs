using GYWx.Post.JsonModel.TempletMsg.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatMall.WX.Model.Wx.Msg
{
    public class CouponExpiredMsgCls : TempletMsgBaseCls
    {
        ///DHJOcMr8B4nXLZDMKKmsI9A8HE23LVc5bqT0bVYFm5U

        /// <summary>
        /// 标题
        /// </summary>
        public TempletMsgTextCls first { get; set; }
        /// <summary>
        /// 咨询名称
        /// </summary>
        public TempletMsgTextCls orderTicketStore { get; set; }
        /// <summary>
        /// 消息回复
        /// </summary>
        public TempletMsgTextCls orderTicketRule { get; set; }
        /// <summary>
        /// 结尾
        /// </summary>
        public TempletMsgTextCls remark { get; set; }
    }
}
