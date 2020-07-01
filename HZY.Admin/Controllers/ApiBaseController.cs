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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HZY.Admin.Controllers
{
    using HZY.Admin.Core;
    using HZY.Admin.Services.Sys;
    using HZY.Toolkit;
    using HZY.Toolkit.HzyNetCoreUtil.Attributes;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Newtonsoft.Json;
    using System.Reflection;

    /// <summary>
    /// 接口 基类
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class ApiBaseController<TService> : ApiBaseController
        where TService : class
    {
        protected readonly TService service;

        public ApiBaseController(string menuId, Sys_MenuService _menuService, TService _service)
            : base(menuId, _menuService)
        {
            this.service = _service;
        }

    }

    /// <summary>
    /// 接口 基类
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(ApiVersionsEnum.Admin), IgnoreApi = true)]
    public class ApiBaseController : BaseController
    {
        protected readonly Guid MenuId;
        protected readonly Sys_MenuService menuService;

        public ApiBaseController(string menuId, Sys_MenuService _menuService)
        {
            this.MenuId = Guid.Parse(menuId);
            this.menuService = _menuService;
        }

        private List<string> Actions { get; set; } = new List<string>()
        {
            //"Save",
            //"Delete",
            //"ChangePwd",
        };

        /// <summary>
        /// Action 执行 前
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            #region 阻止编辑操作 CURD 【这块代码 可删除】

            var _RouteValues = context.ActionDescriptor.RouteValues;
            //var _AreaName = _RouteValues["area"];
            var _ControllerName = _RouteValues["controller"];
            var _ActionName = _RouteValues["action"];

            //阻止进行 添加 修改 删除
            if (Actions.Select(w => w.ToLower()).Contains(_ActionName.ToLower())) throw new MessageBox("更多操作请下载源代码本地运行!");

            #endregion

            #region 检查是否登录 授权

            //获取 token
            var token = AccountService.GetToken(context.HttpContext);

            if (string.IsNullOrWhiteSpace(token))
            {
                if (context.HttpContext.IsAjaxRequest())
                {
                    context.Result = Json(new ApiResult((int)StatusCodeEnum.未授权, StatusCodeEnum.未授权.ToString()));
                }
                else
                {
                    var Alert = $@"<script type='text/javascript'>
                                        alert('{StatusCodeEnum.未授权.ToString()}！请重新登录授权！');
                                        top.window.location='/Authorization/Index';
                                    </script>";
                    context.Result = new ContentResult() { Content = Alert, ContentType = "text/html;charset=utf-8;" };
                }
                return;
            }

            #endregion

            #region 检查页面权限信息

            if (MenuId == Guid.Empty) return;

            //判断是否 查找带回
            var isFindback = context.HttpContext.Request.Query.ContainsKey("findback");
            var power = new Dictionary<string, object>();
            if (isFindback)
            {
                //收集用户权限
                power = this.menuService.GetFindBackPower().Result;
            }
            else
            {
                //收集用户权限
                power = this.menuService.GetPowerStateByMenuId(this.MenuId).Result;

                if (!power["Have"].ToBool() && !context.HttpContext.IsAjaxRequest())
                {
                    context.Result = new ContentResult() { Content = "您无权访问!", ContentType = "text/html;charset=utf-8;" };
                    return;
                }
            }

            ViewData["power"] = JsonConvert.SerializeObject(power);
            ViewData["isFindback"] = isFindback ? 1 : 0;
            #endregion

            #region 检查是否需要 事务

            var methodInfo = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo;

            if (this.CheckTransactionAttribute(methodInfo))
            {
                this.menuService.Context.CommitOpen();
            }

            #endregion
        }

        /// <summary>
        /// Action 执行 后
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            var methodInfo = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo;

            if (this.CheckTransactionAttribute(methodInfo))
            {
                this.menuService.Context.Commit();
            }
        }

        private bool CheckTransactionAttribute(MethodInfo methodInfo)
        {
            //判断是否 有 事务标记
            var transactionAttribute = methodInfo.GetCustomAttribute<AppTransactionAttribute>();
            return transactionAttribute != null;
        }

    }





}