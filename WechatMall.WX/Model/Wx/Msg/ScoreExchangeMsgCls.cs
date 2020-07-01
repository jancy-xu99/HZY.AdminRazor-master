using GYWx.Post.JsonModel.TempletMsg.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatMall.WX.Model.Wx.Msg
{
    public class ScoreExchangeMsgCls : TempletMsgBaseCls
    {
        ///CAt52URs8O6bgQZcEkThHHAWhf-ohEYy2qGPJTgRrco

        /// <summary>
        /// 标题
        /// </summary>
        public TempletMsgTextCls first { get; set; }
        /// <summary>
        /// 兑换凭证
        /// </summary>
        public TempletMsgTextCls accountType { get; set; }
        /// <summary>
        /// 凭证内容
        /// </summary>
        public TempletMsgTextCls account { get; set; }
        /// <summary>
        /// 充值金额
        /// </summary>
        public TempletMsgTextCls amount { get; set; }
        /// <summary>
        /// 充值状态
        /// </summary>
        public TempletMsgTextCls result { get; set; }
        /// <summary>
        /// 结尾
        /// </summary>
        public TempletMsgTextCls remark { get; set; }
    }
}
