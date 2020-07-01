using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Models.Sys
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(Sys_AppLog))]
    public class Sys_AppLog
    {
        [Key]
        public Guid AppLog_ID { get; set; } = Guid.Empty;

        /// <summary>
        /// Api
        /// </summary>
        public string AppLog_Api { get; set; }

        /// <summary>
        /// Ip
        /// </summary>
        public string AppLog_IP { get; set; }

        /// <summary>
        /// UserID
        /// </summary>
        public Guid? AppLog_UserID { get; set; }

        /// <summary>
        /// 表单信息
        /// </summary>
        public string AppLog_Form { get; set; }

        /// <summary>
        /// body 信息
        /// </summary>
        /// <value></value>
        public string AppLog_FormBody { get; set; }

        /// <summary>
        /// 地址栏信息
        /// </summary>
        /// <value></value>
        public string AppLog_QueryString { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime AppLog_CreateTime { get; set; }


    }
}
