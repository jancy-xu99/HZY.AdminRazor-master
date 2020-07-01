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
    using HZY.Admin.Dto.Sys;
    using Microsoft.EntityFrameworkCore;
    using HZY.EFCore;

    public class Sys_RoleMenuFunctionService : ServiceBase<Sys_RoleMenuFunction>
    {
        protected readonly DefaultRepository<Sys_Menu> dbMenu;
        protected readonly DefaultRepository<Sys_Role> dbRole;
        protected readonly DefaultRepository<Sys_Function> dbFunction;
        protected readonly DefaultRepository<Sys_MenuFunction> dbMenuFunction;
        protected readonly AccountService accountService;

        public Sys_RoleMenuFunctionService(EFCoreContext _db,
            DefaultRepository<Sys_Menu> _dbMenu,
            DefaultRepository<Sys_Role> _dbRole,
            DefaultRepository<Sys_Function> _dbFunction,
            DefaultRepository<Sys_MenuFunction> _dbMenuFunction,
            AccountService _accountService

            ) : base(_db)
        {
            this.dbMenu = _dbMenu;
            this.dbRole = _dbRole;
            this.dbFunction = _dbFunction;
            this.dbMenuFunction = _dbMenuFunction;
            this.accountService = _accountService;

        }


        #region CURD 基础

        /// <summary>
        /// 新增\修改
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        public async Task<Guid> SaveAsync(Sys_RoleMenuFunctionDto Dto)
        {
            var RoleId = Dto.RoleId;
            var MenuId = Dto.MenuId;
            var FunctionIds = Dto.FunctionIds;

            await this.DeleteAsync(w => w.RoleMenuFunction_RoleID == RoleId & w.RoleMenuFunction_MenuID == MenuId);

            foreach (var item in Dto.FunctionIds)
            {
                var model = new Sys_RoleMenuFunction();
                model.RoleMenuFunction_MenuID = MenuId;
                model.RoleMenuFunction_RoleID = RoleId;
                model.RoleMenuFunction_FunctionID = item;

                await this.InsertAsync(model);
            }

            return RoleId;
        }

        /// <summary>
        /// 加载表单 数据
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, object>> LoadFormAsync()
        {
            var res = new Dictionary<string, object>();

            var _Sys_RoleList = await dbRole.Query()
                .OrderBy(w => w.Role_Num)
                .Select(w => new
                {
                    w.Role_ID,
                    w.Role_Num,
                    w.Role_Name,
                    w.Role_Remark,
                    w.Role_IsDelete,
                    w.Role_CreateTime,
                    State = 0
                })
                .ToListAsync<dynamic>();

            foreach (var item in _Sys_RoleList)
            {
                if (_Sys_RoleList.IndexOf(item) == 0)
                    item.State = 1;
                else
                    break;
            }

            res[nameof(_Sys_RoleList)] = _Sys_RoleList;

            return res;
        }


        #endregion

        #region 角色菜单功能 Tree

        private List<Guid> Ids = new List<Guid>();

        public async Task<(List<Guid>, List<object>)> GetRoleMenuFunctionTreeAsync(Guid RoleId)
        {
            var _Sys_MenuList = await dbMenu.Query().OrderBy(w => w.Menu_Num).ToListAsync();
            var _Sys_FunctionList = await dbFunction.Query().OrderBy(w => w.Function_Num).ToListAsync();
            var _Sys_MenuFunctionList = await dbMenuFunction.Query().OrderBy(w => w.MenuFunction_CreateTime).ToListAsync();
            var _Sys_RoleMenuFunctionList = await this.ToListAsync(w => w.RoleMenuFunction_RoleID == RoleId);

            return (Ids, this.CreateRoleMenuFuntionTree(Guid.Empty, _Sys_MenuList, _Sys_FunctionList, _Sys_MenuFunctionList, _Sys_RoleMenuFunctionList));
        }

        /// <summary>
        /// 角色权限树
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="_Sys_MenuList"></param>
        /// <param name="_Sys_FunctionList"></param>
        /// <param name="_Sys_MenuFunctionList"></param>
        /// <param name="_Sys_RoleMenuFunctionList"></param>
        /// <returns></returns>
        public List<object> CreateRoleMenuFuntionTree(Guid Id, List<Sys_Menu> _Sys_MenuList, List<Sys_Function> _Sys_FunctionList, List<Sys_MenuFunction> _Sys_MenuFunctionList, List<Sys_RoleMenuFunction> _Sys_RoleMenuFunctionList)
        {
            var _Menus = new List<object>();
            List<Sys_Menu> _MenuItem = null;
            if (Id == Guid.Empty)
                _MenuItem = _Sys_MenuList.Where(w => w.Menu_ParentID == null || w.Menu_ParentID == Guid.Empty).ToList();
            else
                _MenuItem = _Sys_MenuList.Where(w => w.Menu_ParentID == Id).ToList();

            foreach (var item in _MenuItem)
            {
                var _children = new List<object>();
                var _Functions = new List<object>();
                var _CheckFunction = new List<object>();
                if (_Sys_MenuList.Any(w => w.Menu_ParentID == item.Menu_ID))
                {
                    _children = this.CreateRoleMenuFuntionTree(item.Menu_ID, _Sys_MenuList, _Sys_FunctionList, _Sys_MenuFunctionList, _Sys_RoleMenuFunctionList);
                }
                else
                {
                    //if (string.IsNullOrWhiteSpace(item.Menu_Url)) continue;
                    //遍历功能
                    foreach (var _Function in _Sys_FunctionList)
                    {
                        //判断是否 该菜单下 是否勾选了 该功能
                        var _Sys_MenuFunction_Any = _Sys_MenuFunctionList.Any(w =>
                         w.MenuFunction_MenuID == item.Menu_ID &&
                         w.MenuFunction_FunctionID == _Function.Function_ID);

                        if (!_Sys_MenuFunction_Any) continue;

                        var id = $"{_Function.Function_ID}";

                        _Functions.Add(new
                        {
                            id,
                            //label = $"{_Function.Function_Name}-{_Function.Function_ByName}-{_Function.Function_Num}"
                            label = _Function.Function_Name
                        });

                        //判断该角色 对应的菜单和功能是否存在
                        var _Any = _Sys_RoleMenuFunctionList.Any(w =>
                          w.RoleMenuFunction_MenuID == item.Menu_ID &&
                          w.RoleMenuFunction_FunctionID == _Function.Function_ID);

                        if (_Any) _CheckFunction.Add(id);

                    }

                }

                //
                if (_children.Count > 0) Ids.Add(item.Menu_ID);
                _Menus.Add(new
                {
                    id = item.Menu_ID,
                    label = $"{item.Menu_Name}-{item.Menu_Num}",
                    children = _children.Count == 0 ? null : _children,
                    functions = _Functions,
                    checkFunction = _CheckFunction
                });
            }

            return _Menus;
        }

        #endregion




    }
}
