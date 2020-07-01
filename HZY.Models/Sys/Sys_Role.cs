using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Models.Sys
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(Sys_Role))]
    public class Sys_Role
    {
        [Key]
        public Guid Role_ID { get; set; } = Guid.Empty;

        /// <summary>
        /// 编号
        /// </summary>
        public string Role_Num { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [Required(ErrorMessage = "角色名称不能为空!")]
        public string Role_Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Role_Remark { get; set; }

        /// <summary>
        /// 是否可以删除=>1 是 2 否
        /// </summary>
        public int Role_IsDelete { get; set; } = 1;

        /// <summary>
        /// 创建时间
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Role_CreateTime { get; set; }
    }
}
