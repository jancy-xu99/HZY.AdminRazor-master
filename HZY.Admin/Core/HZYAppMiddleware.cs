using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HZY.Admin.Core
{
    using HZY.Admin.Services.Sys;
    using HZY.Toolkit;
    using System.Diagnostics;

    public class HZYAppMiddleware : IMiddleware
    {
        protected AccountService service { get; }

        public HZYAppMiddleware(AccountService _service)
        {
            this.service = _service;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //记录 api 执行耗时
            Stopwatch sw = new Stopwatch();
            sw.Restart();

            context.Request.EnableBuffering();//启用倒带功能，就可以让 Request.Body 可以再次读取
            await service.InsertAppLogAsync();
            context.Request.Body.Position = 0;//必须存在

            await next.Invoke(context);

            sw.Stop();

            Tools.Log.Write($"请求：{context.Request.Path} 耗时：{sw.ElapsedMilliseconds} 毫秒!");
        }

    }
}