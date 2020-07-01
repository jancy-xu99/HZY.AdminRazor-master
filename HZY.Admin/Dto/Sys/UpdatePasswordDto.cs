using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Admin.Dto.Sys
{
    public class UpdatePasswordDto
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPwd { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string NewPwd { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        /// <value></value>
        public string QrPwd { get; set; }


    }
}
