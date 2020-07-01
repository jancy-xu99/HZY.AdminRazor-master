using GYWx.Post.JsonModel.TempletMsg.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatMall.WX.Model.Wx.Msg
{
    public class NewOrderMsgCls : TempletMsgBaseCls
    {
        ///eXh7etRKRfD_3UDOwyVjcnoYh9r2Rz8xYwnG6NxPlO8

        /// <summary>
        /// 标题
        /// </summary>
        public TempletMsgTextCls first { get; set; }
        /// <summary>
        /// 商品明细
        /// </summary>
        public TempletMsgTextCls keyword1 { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        public TempletMsgTextCls keyword2 { get; set; }
        /// <summary>
        /// 配送地址
        /// </summary>
        public TempletMsgTextCls keyword3 { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public TempletMsgTextCls keyword4 { get; set; }
        /// <summary>
        /// 付款状态
        /// </summary>
        public TempletMsgTextCls keyword5 { get; set; }
        /// <summary>
        /// 结尾
        /// </summary>
        public TempletMsgTextCls remark { get; set; }
    }
}
