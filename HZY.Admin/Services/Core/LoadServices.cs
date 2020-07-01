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
using System.Reflection;
using System.Threading.Tasks;

namespace HZY.Admin.Services.Core
{
    using HZY.Toolkit.HzyNetCoreUtil.Attributes;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// 加载服务
    /// </summary>
    public static class LoadServices
    {
        /// <summary>
        /// Service 服务启动
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="type"></param>
        public static void ServiceStart(this IServiceCollection serviceCollection, Type type)
        {
            if (type == null) throw new ArgumentException(" 参数 type null 异常!");

            var classList = type.Assembly.ExportedTypes
                //.Where(w => w.GetCustomAttribute(typeof(AppServiceAttribute)) != null || w.BaseType.GetCustomAttribute(typeof(AppServiceAttribute)) != null)
                .ToList();

            foreach (var item in classList)
            {
                var appService = item.GetCustomAttribute<AppServiceAttribute>();

                if (appService == null && item.BaseType != null)
                {
                    appService = item.BaseType.GetCustomAttribute<AppServiceAttribute>();
                }
                if (appService != null)
                {
                    switch (appService.serviceType)
                    {
                        case ServiceType.Scoped:
                            serviceCollection.AddScoped(item);
                            break;
                        case ServiceType.Transient:
                            serviceCollection.AddTransient(item);
                            break;
                        case ServiceType.Singleton:
                            serviceCollection.AddSingleton(item);
                            break;
                        default:
                            throw new NotSupportedException();
                            //break;
                    }

                }


            }

        }

    }
}
