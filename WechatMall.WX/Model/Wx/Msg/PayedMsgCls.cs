
using GYWx.Post.JsonModel.TempletMsg.Input;
namespace WechatMall.WX.Model.Wx.Msg
{
    /// <summary>
    /// 登陆模板类
    /// </summary>
    public class PayedMsgCls : TempletMsgBaseCls
    {
        /// <summary>
        /// 标题
        /// </summary>
        public TempletMsgTextCls first { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        public TempletMsgTextCls orderMoneySum { get; set; }
        /// <summary>
        /// 支付商品名
        /// </summary>
        public TempletMsgTextCls orderProductName { get; set; }
        /// <summary>
        /// 结尾
        /// </summary>
        public TempletMsgTextCls remark { get; set; }
    }
}
