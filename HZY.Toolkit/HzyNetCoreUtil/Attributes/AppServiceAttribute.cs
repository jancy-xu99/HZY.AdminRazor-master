using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Toolkit.HzyNetCoreUtil.Attributes
{
    /// <summary>
    /// 服务标记 用于 程序 启动 扫描到后自动 注册服务
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AppServiceAttribute : Attribute
    {

        public ServiceType serviceType { get; set; } = ServiceType.Scoped;

        public AppServiceAttribute(ServiceType serviceType = ServiceType.Scoped)
        {

        }

    }

    public enum ServiceType
    {
        Scoped,
        Transient,
        Singleton
    }

}
