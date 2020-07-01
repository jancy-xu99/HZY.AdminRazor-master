using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Models.Sys
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(Sys_Menu))]
    public class Sys_Menu
    {

        [Key]
        public Guid Menu_ID { get; set; } = Guid.Empty;

        /// <summary>
        /// 编号
        /// </summary>
        [Required(ErrorMessage = "编号不能为空!")]
        public int? Menu_Num { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        [Required(ErrorMessage = "菜单名称不能为空!")]
        public string Menu_Name { get; set; }

        /// <summary>
        /// 菜单地址
        /// </summary>
        public string Menu_Url { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string Menu_Icon { get; set; }

        /// <summary>
        /// 上级菜单
        /// </summary>
        public Guid? Menu_ParentID { get; set; } = Guid.Empty;

        /// <summary>
        /// 是否显示 =>1 是 2 否
        /// </summary>
        public int Menu_IsShow { get; set; } = 1;

        /// <summary>
        /// 是否关闭 => 1=是 0=否 关闭选项卡
        /// </summary>
        public int Menu_IsClose { get; set; } = 1;

        /// <summary>
        /// 创建时间
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Menu_CreateTime { get; set; }

    }
}
