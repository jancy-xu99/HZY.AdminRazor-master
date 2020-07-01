


using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Toolkit.ToolKits
{

    /// <summary>
    /// sql相关操作辅助类
    /// </summary>
    public static class SqlHelp
    {

        /// <summary>
        /// 获取分页查询数据SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="model"></param>
        /// <returns></returns>
//        public static string GetPageSql(string sql, PagerModel model)
//        {
//            int skipCount = (model.PageSize * (model.PageIndex - 1));
//            int takeCount = (model.PageSize * model.PageIndex);
//            string reSQL = string.Empty;
//            string tempStr = @"
//With t_rowtable As (Select row_number() over( order by  {0} {5} ) As row_number,* From (   {1})
//as tmp ) Select * From t_rowtable Where row_number> {2} And row_number <={3} order by  {4} {5} ";
//            reSQL = string.Format(tempStr, model.SortField, sql, skipCount, takeCount, model.SortField, model.SortDirection);
//            return reSQL;
//        }
    }
}
