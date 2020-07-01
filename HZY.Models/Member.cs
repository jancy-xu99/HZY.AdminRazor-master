using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(nameof(Member))]
    public class Member
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        [Key]
        public Guid Member_ID { get; set; } = Guid.Empty;

        /// <summary>
        /// 编号
        /// </summary>
        public string Member_Num { get; set; }

        /// <summary>
        /// 会员名称
        /// </summary>
        [Required(ErrorMessage = "会员名称不能为空!")]
        public string Member_Name { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Required(ErrorMessage = "电话不能为空!")]
        [RegularExpression(@"^\+?\d{0,4}?[1][3-8]\d{9}$", ErrorMessage = "手机号码格式错误")]
        [StringLength(11, ErrorMessage = "电话长度只能11位!")]
        public string Member_Phone { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Member_Sex { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Member_Birthday { get; set; } = DateTime.Now;

        /// <summary>
        /// 头像
        /// </summary>
        [Required(ErrorMessage = "请上传头像!")]
        public string Member_Photo { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid Member_UserID { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        [Required(ErrorMessage = "简介不能为空!")]
        public string Member_Introduce { get; set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        public string Member_FilePath { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Member_CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Member_CreateBy { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Member_ModifyTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string Member_ModifyBy { get; set; }

    }

}
