using GYWx.Post.JsonModel.TempletMsg.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatMall.WX.Model.Wx.Msg
{
    public class OrderRemindMsgCls : TempletMsgBaseCls
    {

        //{{first.DATA}}
        //商品名称：{{keyword1.DATA}}
        //商品数量：{{keyword2.DATA}}
        //备注：{{keyword3.DATA}}
        //{{remark.DATA}}

        /// <summary>
        /// 标题
        /// </summary>
        public TempletMsgTextCls first { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public TempletMsgTextCls keyword1 { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public TempletMsgTextCls keyword2 { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public TempletMsgTextCls keyword3 { get; set; }
        /// <summary>
        /// 结尾
        /// </summary>
        public TempletMsgTextCls remark { get; set; }
    }
}

