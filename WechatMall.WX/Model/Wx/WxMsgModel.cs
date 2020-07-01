using GYLib.Base.Utils;
using GYWx.Reply.NormalReply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatMall.WX.Model.Wx
{
    public class WxMsgModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int MsgId { get; set; }
        /// <summary>
        /// 消息名
        /// </summary>
        [DataModel(Insert = true, Update = true)]
        public string MsgName { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        [DataModel(Insert = true, Update = true)]
        public string Content { get; set; }
        /// <summary>
        /// 消息累心
        /// </summary>
        [DataModel(Insert = true, Update = true)]
        public int MsgType { get; set; }
        /// <summary>
        /// 是否作废
        /// </summary>
        [DataModel(Insert = true, Update = true)]
        public int IsActive { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        [DataModel(Insert = true, Update = true)]
        public int Disabled { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [DataModel(Insert = true, Update = true)]
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataModel(Insert = true, Update = true)]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改人 
        /// </summary>
        [DataModel(Insert = true, Update = true)]
        public string ModifyBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [DataModel(Insert = true, Update = true)]
        public DateTime ModifyTime { get; set; }

        public ContentModel ContentModel { get; set; }

        public WxMsgModel()
        {
            ContentModel = new ContentModel();
        }
    }
}
