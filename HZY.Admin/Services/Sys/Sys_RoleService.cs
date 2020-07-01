using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Admin.Services.Sys
{
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Microsoft.AspNetCore.Http;
    using HZY.EFCore.Repository;
    using HZY.Models.Sys;
    using HZY.Toolkit;
    using System.Linq;
    using HZY.EFCore.Base;
    using HZY.Admin.Services.Core;
    using HZY.EFCore;

    public class Sys_RoleService : ServiceBase<Sys_Role>
    {
        public Sys_RoleService(EFCoreContext _db)
            : base(_db)
        {

        }

        #region CURD 基础

        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Rows"></param>
        /// <param name="Search"></param>
        /// <returns></returns>
        public async Task<TableViewModel> FindListAsync(int Page, int Rows, Sys_Role Search)
        {
            var query = this.Query()
                .WhereIF(w => w.Role_Name.Contains(Search.Role_Name), !string.IsNullOrWhiteSpace(Search?.Role_Name))
                .Select(w => new
                {
                    w.Role_Num,
                    w.Role_Name,
                    Role_IsDelete = (w.Role_IsDelete == 1 ? "是" : "否"),
                    Role_CreateTime = w.Role_CreateTime.ToString("yyyy-MM-dd"),
                    _ukid = w.Role_ID
                })
                ;

            return await this.db.AsTableViewModelAsync(query, Page, Rows, typeof(Sys_Role));
        }

        /// <summary>
        /// 新增\修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Guid> SaveAsync(Sys_Role model)
        {
            await this.InsertOrUpdateAsync(model);

            return model.Role_ID;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(List<Guid> Ids)
            => await this.DeleteAsync(w => Ids.Contains(w.Role_ID) && w.Role_IsDelete != 2);

        /// <summary>
        /// 加载表单 数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, object>> LoadFormAsync(Guid Id)
        {
            var res = new Dictionary<string, object>();

            var Model = await this.FindByIdAsync(Id);

            res[nameof(Id)] = Id;
            res[nameof(Model)] = Model.ToNewByNull();

            return res;
        }


        #endregion






    }
}
