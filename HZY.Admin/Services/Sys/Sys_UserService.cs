using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HZY.Admin.Services.Sys
{
    using Newtonsoft.Json;
    using Microsoft.AspNetCore.Http;
    using HZY.EFCore.Repository;
    using HZY.Models.Sys;
    using HZY.Toolkit;
    using System.Linq;
    using HZY.EFCore.Base;
    using HZY.Admin.Dto.Sys;
    using Microsoft.EntityFrameworkCore;
    using HZY.Admin.Services.Core;
    using HZY.EFCore;

    public class Sys_UserService : ServiceBase<Sys_User>
    {
        protected readonly DefaultRepository<Sys_UserRole> dbUserRole;

        public Sys_UserService(EFCoreContext _db, DefaultRepository<Sys_UserRole> _dbUserRole)
            : base(_db)
        {
            this.dbUserRole = _dbUserRole;
        }

        #region CURD 基础

        public async Task<TableViewModel> FindListAsync(int Page, int Rows, Sys_User Search)
        {
            var query = this.Query()
                .WhereIF(w => w.User_LoginName.Contains(Search.User_LoginName), !string.IsNullOrWhiteSpace(Search?.User_LoginName))
                .WhereIF(w => w.User_Name.Contains(Search.User_Name), !string.IsNullOrWhiteSpace(Search?.User_Name))
                .Select(w => new
                {
                    w.User_Name,
                    w.User_LoginName,
                    当前角色 = string.Join("、", from userRoles in db.Sys_UserRoles
                                            join roles in db.Sys_Roles on userRoles.UserRole_RoleID equals roles.Role_ID
                                            where userRoles.UserRole_UserID == w.User_ID
                                            select roles.Role_Name),
                    w.User_Email,
                    User_CreateTime = w.User_CreateTime.ToString("yyyy-MM-dd"),
                    _ukid = w.User_ID
                })
                ;

            return await db.AsTableViewModelAsync(query, Page, Rows);
        }

        /// <summary>
        /// 新增\修改
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        public async Task<Guid> SaveAsync(Sys_UserDto Dto)
        {
            var model = Dto.Model;
            var roleIds = Dto.RoleIds;

            if (string.IsNullOrWhiteSpace(model.User_Pwd)) MessageBox.Show("密码不能为空！");

            if (model.User_ID == Guid.Empty)
            {
                model.User_Pwd = string.IsNullOrWhiteSpace(model.User_Pwd) ? "123" : model.User_Pwd; //Tools.MD5Encrypt("123");
                model = await this.InsertAsync(model);
            }
            else
            {
                await this.UpdateByIdAsync(model);
            }

            //变更用户角色
            if (roleIds.Count > 0)
            {
                var _Sys_UserRoleList = await dbUserRole.ToListAsync(w => w.UserRole_UserID == model.User_ID);

                await dbUserRole.DeleteAsync(w => w.UserRole_UserID == model.User_ID);
                foreach (var item in roleIds)
                {
                    var _Sys_UserRole = _Sys_UserRoleList.FirstOrDefault(w => w.UserRole_RoleID == item);

                    var userRoleModel = new Sys_UserRole();
                    userRoleModel.UserRole_ID = _Sys_UserRole == null ? Guid.NewGuid() : _Sys_UserRole.UserRole_ID;
                    userRoleModel.UserRole_RoleID = item;
                    userRoleModel.UserRole_UserID = model.User_ID;
                    await dbUserRole.InsertAsync(userRoleModel);
                }
            }

            return model.User_ID;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(List<Guid> Ids)
        {
            foreach (var item in Ids)
            {
                var userModel = await this.FindByIdAsync(item);
                if (userModel.User_IsDelete == 2) MessageBox.Show("该信息不能删除！");
                await dbUserRole.DeleteAsync(w => w.UserRole_UserID == item);
                await this.DeleteAsync(userModel);
            }

            return 1;
        }

        /// <summary>
        /// 加载表单 数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, object>> LoadFormAsync(Guid Id)
        {
            var res = new Dictionary<string, object>();

            var Model = await this.FindByIdAsync(Id);

            var RoleIds = await (
                from userRole in db.Sys_UserRoles
                join role in db.Sys_Roles on userRole.UserRole_RoleID equals role.Role_ID
                where userRole.UserRole_UserID == Id
                orderby userRole.UserRole_CreateTime
                select userRole.UserRole_RoleID
                ).ToListAsync();

            var AllRoleList = await (
                from role in db.Sys_Roles
                orderby role.Role_Num
                select new { role.Role_ID, role.Role_Num, role.Role_Name }
                ).ToListAsync();

            res[nameof(Id)] = Id;
            res[nameof(Model)] = Model.ToNewByNull();
            res[nameof(RoleIds)] = RoleIds;
            res[nameof(AllRoleList)] = AllRoleList;

            return res;
        }


        #endregion

        #region 导出 Excel

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="Search"></param>
        /// <returns></returns>
        public async Task<byte[]> ExportExcel(Sys_User Search)
        {
            var tableViewModel = await this.FindListAsync(1, 999999, Search);
            return this.ExportExcelByTableViewModel(tableViewModel);
        }

        #endregion







    }
}
