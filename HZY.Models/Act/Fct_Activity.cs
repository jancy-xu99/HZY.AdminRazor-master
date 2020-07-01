using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HZY.Models.Common;

namespace HZY.Models.Act
{
    [Dapper.Contrib.Extensions.Table(nameof(Fct_Activity))]
    [System.ComponentModel.DataAnnotations.Schema.Table("Fct_Activity")]

    public class Fct_Activity
    {
        public Fct_Activity()
        {
            this.Disabled = 0;
            this.CreateTime = Convert.ToDateTime(DateTime.Now);
            this.ModifyTime = Convert.ToDateTime(DateTime.Now);

        }


        /// <summary>
        /// 活动ID
        /// </summary>
        [Dapper.Contrib.Extensions.Key]
        [System.ComponentModel.DataAnnotations.Key]
        public int ActId { get; set; }

        /// <summary>
        /// 活动GUID
        /// </summary>
        public Guid ActGuId { get; set; } = Guid.NewGuid();



        /// <summary>
        /// 活动名称
        /// </summary>
        [Required(ErrorMessage = "活动名称不能为空!")]
        public string ActName { get; set; }
        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime ActStartTime { get; set; } = Convert.ToDateTime(DateTime.Now);


        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime ActEndTime { get; set; } = Convert.ToDateTime(DateTime.Now);

        /// <summary>
        /// 微信活动
        /// </summary>
        public int ActIsWechat { get; set; }

        /// <summary>
        /// 必须关注微信
        /// </summary>
        public int ActMustSubscribe { get; set; }

        /// <summary>
        /// 富文本框设计的内容
        /// 
        /// </summary>
        public string Design_Introduce { get; set; }
        /// <summary>
        /// 必须登陆
        /// </summary>
        public int ActMustLogin { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public int ActIsEnable { get; set; }

        /// <summary>
        /// 活动备注
        /// </summary>
        public string ActMemo { get; set; }



        /// <summary>
        /// 删除
        /// </summary>
        public int Disabled { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }


        /// <summary>
        /// 操作时间
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string ModifyBy { get; set; }


    }
}
