using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HZY.Models.Mem
{
    [Table(nameof(Fct_MemSubscribe))]
    public class Fct_MemSubscribe
    {
        
        /// <summary>
        /// 关注ID
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 关注GUID
        /// </summary>
        public Guid SubscibeGuId { get; set; } = Guid.Empty;

        /// <summary>
        /// 微信OPENID
        /// </summary>
        public string WxOpenId { get; set; }



        /// <summary>
        /// 通过哪个二维码
        /// </summary>
        public string QrScene { get; set; }


        /// <summary>
        /// 活动ID
        /// </summary>
        public int ActId { get; set; }

        /// <summary>
        /// 是否删除
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
        /// 修改时间
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string ModifyBy { get; set; }
    }
}
