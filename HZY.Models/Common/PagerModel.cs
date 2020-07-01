using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Models.Common
{
    public class PagerModel
    {
        /// <summary>
        /// 获取或设置当前页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 获取或设置每页记录数
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 获取或设置记录总数
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 设置或获取排序字段
        /// </summary>
        public string SortField
        {

            set
            {

                _SortField = (string.IsNullOrEmpty(value) ? "ModifyTime" : value);
            }
            get { return _SortField; }
        }
        /// <summary>
        /// 是否返回第一页(查找)
        /// </summary>
        public bool IsFirstPage { get; set; }

        #region 局部刷新参数
        /// <summary>
        /// 要刷新的URL
        /// </summary>
        public string RefreshUrl { get; set; }
        /// <summary>
        /// 刷新div的ID
        /// </summary>
        public string DivId { get; set; }
        /// <summary>
        /// from表单ID
        /// </summary>
        public string FormSubmitId { get; set; }

        /// <summary>
        /// 刷新参数
        /// </summary>
        public string ParameterName { get; set; }
        /// <summary>
        /// 刷新参数值
        /// </summary>
        public string ParameterValue { get; set; }
        /// <summary>
        /// 当前页面是否只有一个表单
        /// </summary>
        public string FormIsSingle { get; set; }

        /// <summary>
        /// 正序倒序
        /// </summary>
        #endregion
        string _SortDirection = "asc";

        /// <summary>
        /// 排序字段 
        /// </summary>
        string _SortField = "ModifyTime";
        /// <summary>
        /// 设置或获取排序方式
        /// </summary>
        public string SortDirection
        {
            set
            {
                value = value.Trim().ToLower();
                _SortDirection = value == "desc" ? "desc" : "asc";
            }
            get { return _SortDirection; }
        }
        /// <summary>
        /// 获取页数
        /// </summary>
        public int PageCount
        {
            get
            {
                int count = RecordCount / PageSize;
                if (RecordCount % PageSize > 0)
                {
                    count++;
                }

                return count;
            }
        }
        public PagerModel()
        {
            PageIndex = 1;
            PageSize = 15;
            RecordCount = 0;
            FormIsSingle = "No";
            _SortDirection = " DESC ";
        }

        public static DateTime TryParse(string inputText, DateTime defaultValue)
        {
            if (string.IsNullOrEmpty(inputText))
                return defaultValue;
            DateTime result;
            return DateTime.TryParse(inputText, out result) ? result : defaultValue;
        }


    }
    public class PagerModel<T> : PagerModel
    {
        /// <summary>
        /// 获取或设置数据
        /// </summary>
        public T Data { get; set; }
    }
}
