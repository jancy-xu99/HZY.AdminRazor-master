using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Models.Sys
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(Sys_User))]
    public class Sys_User
    {
        [Key]
        public Guid User_ID { get; set; } = Guid.Empty;

        /// <summary>
        /// 用户名称
        /// </summary>
        [Required(ErrorMessage = "用户名称不能为空!")]
        public string User_Name { get; set; }

        /// <summary>
        /// 登录名称
        /// </summary>
        [Required(ErrorMessage = "登录名称不能为空!")]
        public string User_LoginName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string User_Pwd { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Required(ErrorMessage = "邮件不能为空!"), EmailAddress(ErrorMessage = "邮件格式不正确!")]
        public string User_Email { get; set; }

        /// <summary>
        /// 是否删除 => 1 是 2 否
        /// </summary>
        public int User_IsDelete { get; set; } = 1;

        /// <summary>
        /// 创建时间
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime User_CreateTime { get; set; }
    }
}
