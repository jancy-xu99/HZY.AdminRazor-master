using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Models.Sys
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(Sys_MenuFunction))]
    public class Sys_MenuFunction
    {
        [Key]
        public Guid MenuFunction_ID { get; set; } = Guid.Empty;

        /// <summary>
        /// 菜单ID
        /// </summary>
        public Guid MenuFunction_MenuID { get; set; } = Guid.Empty;

        /// <summary>
        /// 功能ID
        /// </summary>
        public Guid MenuFunction_FunctionID { get; set; } = Guid.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime MenuFunction_CreateTime { get; set; }
        
    }
}
