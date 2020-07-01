/*
 * *******************************************************
 *
 * 作者：hzy
 *
 * 开源地址：https://gitee.com/hzy6
 *
 * *******************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Models.Sys
{
    public class AccountInfo
    {
        public AccountInfo()
        {
            this.IsSuperManage = false;
        }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public List<Guid> RoleIDList { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserID { get; set; }
        
        /// <summary>
        /// 当前登录人 是否是 超级管理员
        /// </summary>
        public bool IsSuperManage { get; set; }

        /// <summary>
        /// Sys_User
        /// </summary>
        public Sys_User _Sys_User { get; set; }

    }
}
