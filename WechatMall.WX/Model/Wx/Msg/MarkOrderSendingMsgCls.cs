using GYWx.Post.JsonModel.TempletMsg.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatMall.WX.Model.Wx.Msg
{
    public class MarkOrderSendingMsgCls : TempletMsgBaseCls
    {

        //{{first.DATA}}

        //订单金额：{{orderProductPrice.DATA}}
        //商品详情：{{orderProductName.DATA}}
        //订单编号：{{orderName.DATA}}
        //{{remark.DATA}}

        /// <summary>
        /// 标题
        /// </summary>
        public TempletMsgTextCls first { get; set; }
        /// <summary>
        /// 商品详情
        /// </summary>
        public TempletMsgTextCls orderProductPrice { get; set; }
        /// <summary>
        /// 商品详情
        /// </summary>
        public TempletMsgTextCls orderProductName { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public TempletMsgTextCls orderName { get; set; }
        /// <summary>
        /// 结尾
        /// </summary>
        public TempletMsgTextCls remark { get; set; }
    }
}
