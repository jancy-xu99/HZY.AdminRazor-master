using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Threading.Tasks;
using GYWx.Post.JsonModel;
using HZY.Admin.Services.Act;
using HZY.Admin.Services.Sys;
using HZY.DapperCore.Dapper;
using HZY.EFCore.Repository;
using HZY.Models.Act;
using HZY.Toolkit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HZY.Admin.Controllers.Base
{

    //微信端活动模块
    public class ActivityWebController : Controller // :ActOauthController
    {
        protected readonly IWebHostEnvironment webHostEnvironment;
        protected readonly string webRootPath;
        private readonly DapperClient dapper;
        //public ActivityWebController(IDapperFactory dapperFactory, Sys_MenuService _menservice, ActivityService _service, IWebHostEnvironment _webHostEnvironment)
        ////: base("CAA298AC-104E-4D31-A303-08D7FBCAC280", _menservice, _service)
        //{
        //    this.webHostEnvironment = _webHostEnvironment;
        //    this.webRootPath = _webHostEnvironment.WebRootPath;
        //    //appsettings.json 和  Startup 下配置多个连接字符串
        //    //这样可以操作多个数据库
        //    dapper = dapperFactory.CreateClient("MSSQL1");
        //}

        public ActivityWebController(IDapperFactory dapperFactory, Sys_MenuService _menservice, ActivityService _service, IWebHostEnvironment _webHostEnvironment)
        {
            this.webHostEnvironment = _webHostEnvironment;
            this.webRootPath = _webHostEnvironment.WebRootPath;
            //appsettings.json 和  Startup 下配置多个连接字符串
            //这样可以操作多个数据库
            dapper = dapperFactory.CreateClient("MSSQL1");
        }

        //public IActionResult Index()
        //{
        //    var data = Request.Cookies["UserCookieModel"];
        //    if (_activity.ActMustSubscribe)
        //    {
        //        WxUserInfo usermodel = JsonConvert.DeserializeObject<WxUserInfo>(data);
        //        if (usermodel.subscribe != "1") //未关注
        //        {
        //            //引导关注 推送一条活动图文
        //            GoUrlUnsubscribe(usermodel);
        //        }
        //        //}
        //        //if (_activity.ActMustLogin)
        //        //{ 

        //        //}
        //    }
        //    return View();
        //}

        /// <summary>
        /// id 为活动ID， 标注是从哪个活动进来的
        /// </summary>
        /// <returns></returns>
        [HttpGet("ActivityRegister/{Id?}")]
        public IActionResult ActivityRegister()
        {

            //AppConfig.LDAP.

            return View();
        }

        [HttpPost("ActivityRegister/{Id?}")]
        public async Task<ApiResult> ActivityRegister(int Id, [FromForm] RegisterModel Model)
        {
            var res = new Dictionary<string, object>();
            if (Model == null || string.IsNullOrEmpty(Model.AccountName) || string.IsNullOrEmpty(Model.AccountPwd))
            {
                return null;
            }
            Tools.Log.Write("ActivityRegister 页面开始验证信息：" + Model.AccountName + "  密码：" + Model.AccountPwd);
            bool validatasuccess = ValidataByLDAP(Model.AccountName, Model.AccountPwd);
            Tools.Log.Write("ActivityRegister 页面验证结果：" + validatasuccess);
            res[nameof(Id)] = Id;

            return new ApiResult(StatusCodeEnum.成功, "", res);

            //AppConfig.LDAP.


        }


        /// <summary>
        /// 根据Id 加载表单数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost("LoadForm/{Id?}")]
        public async Task<ApiResult> LoadFormAsync(int Id)
        {
            int ActId = Id;
            var res = new Dictionary<string, object>();
            Fct_Activity Model = await dapper.GetByIDAsync<Fct_Activity>(Id);
            res[nameof(ActId)] = ActId;
            res[nameof(Model)] = Model.ToNewByNull();
            return new ApiResult(StatusCodeEnum.成功, "", res);
        }



        [HttpGet("ActivityWap/{Id?}")]
        public IActionResult ActivityWap(int Id) =>

            View((int)Id);

        private void GoUrlUnsubscribe(WxUserInfo usermodel)
        {
            //ConfigModel config = LogicConfig.GetConfigByCode("0155", "3");
            //string linkurl = ConfigurationManager.AppSettings["WXWebAddressNew"] + "Lottery/ActAirportIndex.aspx?atvid=" + ActID + "&param=" + ParamUcode;

            SubscribeMsgModel model = new SubscribeMsgModel();
            model.WxOpenId = usermodel.openid;
            // model.UserId = usermodel. > 0 ? WebSession.UCode : -1;
            //model.SendMsg = config.Value3;
            //model.LinkUrl = linkurl;
            //model.LinkName = config.Value2;
            model.CreateBy = "活动关注";

            //写关注数据
            //string result = LotteryBase.InsertSubscribeMsg(model);

            //string GoUrl = string.Empty;
            //if (string.IsNullOrEmpty(result))
            //{
            //    GoUrl = "http://mp.weixin.qq.com/s?biz=MzA5MzcwMDYwNQ==&mid=203814070&idx=1&sn=1b2a8da4d5831129415e753c68598a8f#rd";
            //    Response.Redirect(GoUrl);

            //}
            //else
            //{
            //    //插入数据失败 记录日志

            //}
        }


        /// <summary>
        /// LDAP  验证用户名称与密码
        /// </summary>
        /// <param name="useraccount"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ValidataByLDAP(string useraccount, string password)
        {
            //AppConfig.LDAP.ConnUrl;
            var host = AppConfig.LDAP.ConnUrl;
            var baseDN = AppConfig.LDAP.BaseDN;
            var adminName = AppConfig.LDAP.AdminName;
            var adminPass = AppConfig.LDAP.AdminPass;
            var testUser = string.IsNullOrEmpty(useraccount) ? AppConfig.LDAP.PName : useraccount;
            var testPass = string.IsNullOrEmpty(password) ? AppConfig.LDAP.PassWd : password;

            var ldapPath = host + "/" + baseDN; // LDAP必须要大写，好像是.NET的特色
            DirectoryEntry de = new DirectoryEntry(ldapPath, adminName, adminPass, AuthenticationTypes.FastBind);
            DirectorySearcher searcher = new DirectorySearcher(de);
            searcher.Filter = "(uid=" + testUser + ")";
            searcher.SearchScope = SearchScope.Subtree;
            searcher.PropertiesToLoad.Add("uid");
            searcher.PropertiesToLoad.Add("cn");

            var result = searcher.FindOne();

            // 输出几个查询的属性值
            foreach (string n in result.Properties.PropertyNames)
            {
                Console.WriteLine("{0}: {1}", n, result.Properties[n][0].ToString());
                Tools.Log.Write("输出几个查询的属性值" + result.Properties[n][0].ToString());

            }
            try
            {
                int pos = result.Path.LastIndexOf('/');
                string uid = result.Path.Remove(0, pos + 1);

                // 二次连接，使用需要认证的用户密码尝试连接
                DirectoryEntry deUser = new DirectoryEntry(ldapPath, uid, testPass, AuthenticationTypes.FastBind);
                var connected = deUser.NativeObject;
                Tools.Log.Write("二次连接 认证成功" + deUser.Name + deUser.NativeObject);
                Console.WriteLine("### 认证成功！");
                return true;
            }
            catch
            {
                Console.WriteLine("认证失败~~~");
                return false;
            }
            return false;

        }
    }

}
