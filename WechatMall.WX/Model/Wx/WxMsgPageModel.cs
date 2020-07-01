using GYLib.Base.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WechatMall.WX.Model.Wx
{
    public class WxMsgPageModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 消息名
        /// </summary>
        [DataModel(Insert = true)]
        public string MsgID { get; set; }
        /// <summary>
        /// 消息名
        /// </summary>
        [DataModel(Insert = true)]
        public string PageId { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        [DataModel(Insert = true, Update = true)]
        public string Content { get; set; }
        /// <summary>
        /// 阅读原文链接
        /// </summary>
        [DataModel(Insert = true, Update = true)]
        public string BaseUrl { get; set; }
        /// <summary>
        /// 是否有效
        /// </summary>
        [DataModel(Insert = true, Update = true)]
        public int Disabled { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [DataModel(Insert = true)]
        public string CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataModel(Insert = true)]
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

    }
}
