using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace HZY.Admin.Services.Sys
{
    using Newtonsoft.Json;
    using Microsoft.AspNetCore.Http;
    using HZY.EFCore.Repository;
    using HZY.Models.Sys;
    using HZY.Toolkit;
    using System.Linq;
    using HZY.Admin.Services.Core;
    using HZY.EFCore;

    public class AccountService : ServiceBase<Sys_User>
    {
        public HttpContext httpContext { get; }
        public AccountInfo info { get; }
        protected readonly DefaultRepository<Sys_UserRole> dbUserRole;
        protected readonly DefaultRepository<Sys_AppLog> dbAppLog;
        protected readonly string Key;

        public AccountService(EFCoreContext _db,
            DefaultRepository<Sys_UserRole> _dbUserRole,
            DefaultRepository<Sys_AppLog> _dbAppLog,
            IHttpContextAccessor iHttpContextAccessor)
            : base(_db)
        {
            this.dbUserRole = _dbUserRole;
            this.dbAppLog = _dbAppLog;
            this.httpContext = iHttpContextAccessor.HttpContext;

            this.Key = $"Authorization_{httpContext.Request.Host.Host}_{httpContext.Request.Host.Port}";

            //获取 cookie
            var token = this.GetToken();

            if (!string.IsNullOrWhiteSpace(token))
            {
                var Id = new JwtTokenUtil().ReadJwtToken(token).ToGuid();

                this.info = this.GetAccountByUserId(Id.ToGuid()).Result;
            }

            //if (httpContext.User != null)
            //{
            //    var claimsIdentity = httpContext.User.Identity as System.Security.Claims.ClaimsIdentity;
            //    if (claimsIdentity.Name != null)
            //    {
            //        var Id = claimsIdentity.Name;
            //        this.info = this.GetAccountByUserId(Id.ToGuid()).Result;
            //    }Toolkit.ReadXmlSummary.XMLFromName(System.Type, char, string) (位于 [ReadXmlSummary.cs] 中)(navigate-to-context
            //}
        }

        /// <summary>
        /// 获取 Token
        /// </summary>
        /// <returns></returns>
        public string GetToken() => this.httpContext.GetCookie(this.Key);

        /// <summary>
        /// 获取 Token
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string GetToken(HttpContext httpContext)
        {
            var key = $"Authorization_{httpContext.Request.Host.Host}_{httpContext.Request.Host.Port}";
            return httpContext.GetCookie(key);
        }

        /// <summary>
        /// 移除 Token
        /// </summary>
        public void RemoveToken() => this.httpContext.RemoveCookie(this.Key);

        /// <summary>
        /// 账号检查
        /// </summary>
        /// <param name="uName"></param>
        /// <param name="uPwd"></param>
        /// <param name="loginCode"></param>
        public async Task<string> CheckedAsync(string uName, string uPwd, string loginCode)
        {
            if (string.IsNullOrEmpty(uName)) MessageBox.Show("请输入用户名！");
            if (string.IsNullOrEmpty(uPwd)) MessageBox.Show("请输入密码！");
            //if (string.IsNullOrEmpty(loginCode)) throw new MessageBox("请输入验证码");

            var _Sys_User = await this.FindAsync(w => w.User_LoginName == uName);

            if (_Sys_User == null) MessageBox.Show("用户不存在！");
            //Tools.MD5Encrypt(userpwd)))//
            if (_Sys_User.User_Pwd.ToStr().Trim() != uPwd) MessageBox.Show("密码错误！");
            //string code = Tools.GetCookie("loginCode");
            //if (string.IsNullOrEmpty(code)) throw new MessageBox("验证码失效");
            //if (!code.ToLower().Equals(loginCode.ToLower())) throw new MessageBox("验证码不正确");

            return new JwtTokenUtil().GetToken(_Sys_User.User_ID.ToStr(), AppConfig.AdminConfig.JwtSecurityKey, AppConfig.AdminConfig.JwtKeyName);
        }

        /// <summary>
        /// 根据用户信息获取 Account 对象
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<AccountInfo> GetAccountByUserId(Guid Id)
        {
            var _Sys_User = await this.FindByIdAsync(Id);
            var _Account = new AccountInfo();
            var _Sys_UserRole = await dbUserRole.ToListAsync(w => w.UserRole_UserID == _Sys_User.User_ID);
            //
            _Account.RoleIDList = _Sys_UserRole.Select(w => w.UserRole_RoleID).ToList();
            _Account.UserID = _Sys_User.User_ID.ToGuid();
            _Account.UserName = _Sys_User.User_Name;
            _Account._Sys_User = _Sys_User;
            //如果是超级管理员 帐户
            _Account.IsSuperManage = _Sys_UserRole.Any(w => w.UserRole_RoleID == AppConfig.AdminConfig.AdminRoleID);
            return _Account;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oldpwd"></param>
        /// <param name="newpwd"></param>
        /// <returns></returns>
        public async Task<int> ChangePwd(string oldpwd, string newpwd)
        {
            if (string.IsNullOrEmpty(oldpwd)) MessageBox.Show("旧密码不能为空！"); 
            if (string.IsNullOrEmpty(newpwd)) MessageBox.Show("新密码不能为空！");
            var _Sys_User = await this.FindByIdAsync(info.UserID);
            if (_Sys_User.User_Pwd != oldpwd) MessageBox.Show("旧密码不正确！");

            _Sys_User.User_Pwd = newpwd;
            return await this.UpdateByIdAsync(_Sys_User);
        }

        /// <summary>
        /// 写入操作日志
        /// </summary>
        public async Task InsertAppLogAsync()
        {
            var _QueryString = httpContext.Request.QueryString.ToString();
            var _ApiUrl = httpContext.Request.Path;
            var _IP = httpContext.Connection.RemoteIpAddress.ToString();

            //本机不记录
            // if (_IP == "::1") return;

            var body = string.Empty;
            var form = string.Empty;

            //body
            try
            {
                //读取 body 信息
                var reader = new StreamReader(httpContext.Request.Body);
                body = await reader.ReadToEndAsync();
                httpContext.Request.Body.Position = 0;//必须存在

                //Stream stream = httpContext.Request.Body;
                //byte[] buffer = new byte[httpContext.Request.ContentLength.Value];
                //await stream.ReadAsync(buffer, 0, buffer.Length);
                //body = Encoding.UTF8.GetString(buffer);
            }
            catch (Exception)
            {

            }
            //form
            try
            {
                //读取 表单 信息
                var _Form = httpContext.Request.Form;
                if (_Form != null)
                {
                    var _Dictionary = new Dictionary<string, object>();
                    foreach (var key in _Form.Keys)
                    {
                        _Dictionary[key] = _Form[key];
                    }

                    form = JsonConvert.SerializeObject(_Dictionary);
                }

            }
            catch (Exception) { }

            var appLogModel = new Sys_AppLog();

            appLogModel.AppLog_Api = _ApiUrl;
            appLogModel.AppLog_IP = _IP;
            appLogModel.AppLog_Form = form;
            appLogModel.AppLog_QueryString = _QueryString;
            appLogModel.AppLog_FormBody = body;
            appLogModel.AppLog_UserID = info?.UserID;

            await dbAppLog.InsertAsync(appLogModel);
        }


    }
}