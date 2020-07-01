using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Models.Mem
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table(nameof(Fct_Member))]
    public class Fct_Member
    {
        //[Required(ErrorMessage = "会员名称不能为空!")]
        /// <summary>
        /// 会员ID
        /// </summary>
        [Key]
        public int UserId { get; set; }

    /// <summary>
    /// 会员GUID
    /// </summary>
        public Guid UserGuId { get; set; } = Guid.Empty;    

        /// <summary>
        /// 账号
        /// </summary>
        public string UserAccount { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string UserPassword { get; set; }


        /// <summary>
        /// 昵称
        /// </summary>
        public string UserLinkName { get; set; }




        /// <summary>
        /// 真实姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户性别
        /// </summary>
        public int UserSex { get; set; }



        /// <summary>
        /// 微信图像
        /// </summary>
        //[Required(ErrorMessage = "请上传头像!")]
        public string UserWxImageUrl { get; set; }

        /// <summary>
        /// 微信OPENID
        /// </summary>
        public string UserWxOpenId { get; set; }

        /// <summary>
        /// 用户是否关注微信
        /// </summary>
        public int UserWxIsSubScribe { get; set; }


        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// 用户手机号
        /// </summary>
        /// [Required(ErrorMessage = "电话不能为空!")]
        [RegularExpression(@"^\+?\d{0,4}?[1][3-8]\d{9}$", ErrorMessage = "手机号码格式错误")]
        [StringLength(11, ErrorMessage = "电话长度只能11位!")]
        public string UserMobile { get; set; }


        /// <summary>
        /// 简介
        /// </summary>
        //[Required(ErrorMessage = "简介不能为空!")]
        public string UserIntroduce { get; set; }

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
