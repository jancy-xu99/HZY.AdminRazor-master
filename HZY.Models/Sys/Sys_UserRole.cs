using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Models.Sys
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(Sys_UserRole))]
    public class Sys_UserRole
    {
        [Key]
        public Guid UserRole_ID { get; set; } = Guid.Empty;

        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserRole_UserID { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public Guid UserRole_RoleID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UserRole_CreateTime { get; set; }
    }
}
