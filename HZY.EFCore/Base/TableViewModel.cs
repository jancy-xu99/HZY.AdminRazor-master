using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HZY.EFCore.Base
{
    public class TableViewModel
    {
        public TableViewModel() { }

        /// <summary>
        /// 列信息
        /// </summary>
        public List<TableViewCol> Cols { get; set; } = new List<TableViewCol>();

        /// <summary>
        /// 转换后数据
        /// </summary>
        public List<Hashtable> DataSource { get; set; } = new List<Hashtable>();

        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// 一页显示多少条
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; }
    }

    /// <summary>
    /// 列头信息
    /// </summary>
    public class TableViewCol
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string DataIndex { get; set; }

        /// <summary>
        /// 标题名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Show { get; set; } = true;

    }

}
