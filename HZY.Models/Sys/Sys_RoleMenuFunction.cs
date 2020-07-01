using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Models.Sys
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(Sys_RoleMenuFunction))]
    public class Sys_RoleMenuFunction
    {
        [Key]
        public Guid RoleMenuFunction_ID { get; set; } = Guid.Empty;

        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid RoleMenuFunction_RoleID { get; set; }

        /// <summary>
        /// 功能ID
        /// </summary>
        public Guid RoleMenuFunction_FunctionID { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public Guid RoleMenuFunction_MenuID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime RoleMenuFunction_CreateTime { get; set; }
    }
}
