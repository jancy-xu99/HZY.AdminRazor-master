using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Models.Act
{
    public class SubscribeMsgModel
    {
        public string WxOpenId { get; set; }

        public int UserId { get; set; }

        /// <summary>
        /// 引导标题
        /// </summary>
        public string SendMsg { get; set; }

        /// <summary>
        /// 图文推送信息图片路径
        /// </summary>
        public string LinkName { get; set; }

        /// <summary>
        /// 图文推送跳转页面Url
        /// </summary>
        public string LinkUrl { get; set; }


        public string CreateBy { get; set; }

    }
}
