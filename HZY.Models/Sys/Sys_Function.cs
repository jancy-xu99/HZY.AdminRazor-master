using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Models.Sys
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(Sys_Function))]
    public class Sys_Function
    {

        [Key]
        public Guid Function_ID { get; set; } = Guid.Empty;

        /// <summary>
        /// 编号
        /// </summary>
        [Required(ErrorMessage = "编号不能为空!")]
        public int? Function_Num { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "功能名称不能为空!")]
        public string Function_Name { get; set; }

        /// <summary>
        /// 英文名称
        /// </summary>
        [Required(ErrorMessage = "功能英文名称不能为空!")]
        public string Function_ByName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Function_CreateTime { get; set; }


    }
}
