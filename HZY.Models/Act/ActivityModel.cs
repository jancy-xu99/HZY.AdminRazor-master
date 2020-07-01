using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Models.Act
{
    public class ActivityModel
    {
        /// <summary>
        /// 活动Id
        /// </summary>
        public int ActId { get; set; }

        /// <summary>
        /// 活动名称
        /// </summary>
        public string ActName { get; set; }

        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime ActStartTime { get; set; }

        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime ActEndTime { get; set; }

        /// <summary>
        /// 微信活动
        /// </summary>
        public bool ActIsWechat { get; set; }

        /// <summary>
        /// 必须关注微信
        /// </summary>
        public bool ActMustSubscribe { get; set; }

        /// <summary>
        /// 必须登陆
        /// </summary>
        public bool ActMustLogin { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool ActIsEnable { get; set; }

        /// <summary>
        /// 活动备注
        /// </summary>
        public string ActMemo { get; set; }
    }
}
