using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Toolkit.ToolKits
{
    public static class TimeHelp
    {
        /// <summary>
        /// 获取日期字符串
        /// </summary>
        public static string DateStr = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();

        /// <summary>
        /// 获取时间字符串
        /// </summary>
        public static string TimeStr = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
    }
    
}
