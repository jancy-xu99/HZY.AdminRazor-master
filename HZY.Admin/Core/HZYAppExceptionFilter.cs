using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HZY.Admin.Core
{
    using System.Net;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Toolkit;

    /// <summary>
    /// 异常过滤
    /// </summary>
    public class HZYAppExceptionFilter : IExceptionFilter, IOrderedFilter
    {

        public int Order { get; set; } = int.MaxValue - 10;

        public void OnException(ExceptionContext context)
        {
            var _Exception = context.Exception;
            if (_Exception is MessageBox error)
            {
                context.ExceptionHandled = true;
                context.HttpContext.Response.StatusCode = 200;
                context.Result = new JsonResult(error.apiResult);
            }
            else
            {
                if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    context.ExceptionHandled = true;
                    context.HttpContext.Response.StatusCode = 200;
                    context.Result = new JsonResult(new ApiResult((int)StatusCodeEnum.未授权, $"{StatusCodeEnum.未授权.ToString()}"));
                }
                else
                {
                    Tools.Log.Write(_Exception, context.HttpContext.Connection.RemoteIpAddress.ToString());//nlog 写入日志到 txt
                    context.Result = new JsonResult(new ApiResult((int)StatusCodeEnum.程序异常, $"服务端出现异常![异常消息：{_Exception.Message}]"));
                }

            }
        }

    }
}
