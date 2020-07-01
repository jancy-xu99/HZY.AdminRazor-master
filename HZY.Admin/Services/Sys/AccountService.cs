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

            //��ȡ cookie
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
            //    }Toolkit.ReadXmlSummary.XMLFromName(System.Type, char, string) (λ�� [ReadXmlSummary.cs] ��)(navigate-to-context
            //}
        }

        /// <summary>
        /// ��ȡ Token
        /// </summary>
        /// <returns></returns>
        public string GetToken() => this.httpContext.GetCookie(this.Key);

        /// <summary>
        /// ��ȡ Token
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string GetToken(HttpContext httpContext)
        {
            var key = $"Authorization_{httpContext.Request.Host.Host}_{httpContext.Request.Host.Port}";
            return httpContext.GetCookie(key);
        }

        /// <summary>
        /// �Ƴ� Token
        /// </summary>
        public void RemoveToken() => this.httpContext.RemoveCookie(this.Key);

        /// <summary>
        /// �˺ż��
        /// </summary>
        /// <param name="uName"></param>
        /// <param name="uPwd"></param>
        /// <param name="loginCode"></param>
        public async Task<string> CheckedAsync(string uName, string uPwd, string loginCode)
        {
            if (string.IsNullOrEmpty(uName)) MessageBox.Show("�������û�����");
            if (string.IsNullOrEmpty(uPwd)) MessageBox.Show("���������룡");
            //if (string.IsNullOrEmpty(loginCode)) throw new MessageBox("��������֤��");

            var _Sys_User = await this.FindAsync(w => w.User_LoginName == uName);

            if (_Sys_User == null) MessageBox.Show("�û������ڣ�");
            //Tools.MD5Encrypt(userpwd)))//
            if (_Sys_User.User_Pwd.ToStr().Trim() != uPwd) MessageBox.Show("�������");
            //string code = Tools.GetCookie("loginCode");
            //if (string.IsNullOrEmpty(code)) throw new MessageBox("��֤��ʧЧ");
            //if (!code.ToLower().Equals(loginCode.ToLower())) throw new MessageBox("��֤�벻��ȷ");

            return new JwtTokenUtil().GetToken(_Sys_User.User_ID.ToStr(), AppConfig.AdminConfig.JwtSecurityKey, AppConfig.AdminConfig.JwtKeyName);
        }

        /// <summary>
        /// �����û���Ϣ��ȡ Account ����
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
            //����ǳ�������Ա �ʻ�
            _Account.IsSuperManage = _Sys_UserRole.Any(w => w.UserRole_RoleID == AppConfig.AdminConfig.AdminRoleID);
            return _Account;
        }

        /// <summary>
        /// �޸�����
        /// </summary>
        /// <param name="oldpwd"></param>
        /// <param name="newpwd"></param>
        /// <returns></returns>
        public async Task<int> ChangePwd(string oldpwd, string newpwd)
        {
            if (string.IsNullOrEmpty(oldpwd)) MessageBox.Show("�����벻��Ϊ�գ�"); 
            if (string.IsNullOrEmpty(newpwd)) MessageBox.Show("�����벻��Ϊ�գ�");
            var _Sys_User = await this.FindByIdAsync(info.UserID);
            if (_Sys_User.User_Pwd != oldpwd) MessageBox.Show("�����벻��ȷ��");

            _Sys_User.User_Pwd = newpwd;
            return await this.UpdateByIdAsync(_Sys_User);
        }

        /// <summary>
        /// д�������־
        /// </summary>
        public async Task InsertAppLogAsync()
        {
            var _QueryString = httpContext.Request.QueryString.ToString();
            var _ApiUrl = httpContext.Request.Path;
            var _IP = httpContext.Connection.RemoteIpAddress.ToString();

            //��������¼
            // if (_IP == "::1") return;

            var body = string.Empty;
            var form = string.Empty;

            //body
            try
            {
                //��ȡ body ��Ϣ
                var reader = new StreamReader(httpContext.Request.Body);
                body = await reader.ReadToEndAsync();
                httpContext.Request.Body.Position = 0;//�������

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
                //��ȡ �� ��Ϣ
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