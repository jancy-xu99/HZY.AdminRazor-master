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
    using System.Security.Cryptography;
    using HZY.Admin.Dto.Sys;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Internal;
    using System.Collections;
    using HZY.EFCore;

    public class Sys_MenuService : ServiceBase<Sys_Menu>
    {
        protected readonly DefaultRepository<Sys_Function> dbFunction;
        protected readonly DefaultRepository<Sys_MenuFunction> dbMenuFunction;
        protected readonly DefaultRepository<Sys_RoleMenuFunction> dbRoleMenuFunction;
        protected readonly AccountService accountService;

        public Sys_MenuService(EFCoreContext _db,
            DefaultRepository<Sys_Function> _dbFunction,
            DefaultRepository<Sys_MenuFunction> _dbMenuFunction,
            DefaultRepository<Sys_RoleMenuFunction> _dbRoleMenuFunction,
            AccountService _accountService

            ) : base(_db)
        {
            this.dbFunction = _dbFunction;
            this.dbMenuFunction = _dbMenuFunction;
            this.dbRoleMenuFunction = _dbRoleMenuFunction;
            this.accountService = _accountService;

        }


        #region CURD 基础

        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Rows"></param>
        /// <param name="Search"></param>
        /// <returns></returns>
        public async Task<TableViewModel> FindListAsync(int Page, int Rows, Sys_Menu Search)
        {
            var query = (
                from menu in db.Sys_Menus
                from pmenu in db.Sys_Menus.Where(w => w.Menu_ID == menu.Menu_ParentID).DefaultIfEmpty()
                select new { menu, 父级菜单 = pmenu.Menu_Name }
                )
                .WhereIF(w => w.menu.Menu_ParentID == null || w.menu.Menu_ParentID == Guid.Empty, Search?.Menu_ParentID == Guid.Empty || Search.Menu_ParentID == null)
                .WhereIF(w => w.menu.Menu_ParentID == Search.Menu_ParentID, Search?.Menu_ParentID != Guid.Empty && Search?.Menu_ParentID != null)
                .WhereIF(w => w.menu.Menu_Name.Contains(Search.Menu_Name), !string.IsNullOrWhiteSpace(Search?.Menu_Name))
                .OrderBy(w => w.menu.Menu_Num)
                .Select(w => new
                {
                    w.menu.Menu_Num,
                    w.menu.Menu_Name,
                    w.menu.Menu_Url,
                    w.父级菜单,
                    w.menu.Menu_Icon,
                    Menu_IsShow = w.menu.Menu_IsShow == 2 ? "隐藏" : "显示",
                    Menu_CreateTime = w.menu.Menu_CreateTime.ToString("yyyy-MM-dd"),
                    _ukid = w.menu.Menu_ID
                })
                ;
            return await this.db.AsTableViewModelAsync(query, Page, Rows, typeof(Sys_Menu));
        }

        /// <summary>
        /// 新增\修改
        /// </summary>
        /// <param name="Dto"></param>
        /// <returns></returns>
        public async Task<Guid> SaveAsync(Sys_MenuDto Dto)
        {
            var model = Dto.Model;
            var functionIds = Dto.FunctionIds;

            model = await this.InsertOrUpdateAsync(model);
            //
            await dbMenuFunction.DeleteAsync(w => w.MenuFunction_MenuID == model.Menu_ID);
            if (functionIds.Count > 0)
            {
                var _Sys_MenuFunctionList = await dbMenuFunction.ToListAsync(w => w.MenuFunction_MenuID == model.Menu_ID);
                foreach (var item in functionIds)
                {
                    var _Sys_MenuFunction = _Sys_MenuFunctionList.FirstOrDefault(w => w.MenuFunction_FunctionID == item);

                    var menufuncModel = new Sys_MenuFunction();
                    menufuncModel.MenuFunction_ID = _Sys_MenuFunction == null ? Guid.NewGuid() : _Sys_MenuFunction.MenuFunction_ID;
                    menufuncModel.MenuFunction_FunctionID = item;
                    menufuncModel.MenuFunction_MenuID = model.Menu_ID;

                    await dbMenuFunction.InsertAsync(menufuncModel);
                }
            }

            return model.Menu_ID;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(List<Guid> Ids)
        {
            await dbRoleMenuFunction.DeleteAsync(w => Ids.Contains(w.RoleMenuFunction_MenuID));
            await dbMenuFunction.DeleteAsync(w => Ids.Contains(w.MenuFunction_MenuID));
            await this.DeleteAsync(w => Ids.Contains(w.Menu_ID));

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

            var AllFunctionList = await dbFunction.Query()
              .OrderBy(w => w.Function_Num)
              .Select(w => new { w.Function_ID, w.Function_Name, w.Function_ByName })
              .ToListAsync();

            var FunctionIds = await dbMenuFunction.Query()
                .Where(w => w.MenuFunction_MenuID == Id)
                .Select(w => w.MenuFunction_FunctionID)
                .ToListAsync();

            res[nameof(Id)] = Id;
            res[nameof(Model)] = Model.ToNewByNull();
            res[nameof(AllFunctionList)] = AllFunctionList;
            res[nameof(FunctionIds)] = FunctionIds;

            return res;
        }


        #endregion


        #region  创建系统左侧菜单

        /// <summary>
        /// 根据角色ID 获取菜单
        /// </summary>
        /// <returns></returns>
        public async Task<List<Sys_Menu>> GetMenuByRoleIDAsync()
        {
            var _Sys_MenuAllList = await this.Query()
                .Where(w => w.Menu_IsShow == 1)
                .OrderBy(w => w.Menu_Num)
                .ToListAsync();

            if (this.accountService.info.IsSuperManage) return _Sys_MenuAllList;

            var _Sys_MenuList = await (
                    from roleMenuFunction in db.Sys_RoleMenuFunctions
                        //左连接 function
                    from function in db.Sys_Functions.Where(w => w.Function_ID == roleMenuFunction.RoleMenuFunction_FunctionID).DefaultIfEmpty()
                        //左连接 menu
                    from menu in db.Sys_Menus.Where(w => w.Menu_ID == roleMenuFunction.RoleMenuFunction_MenuID).DefaultIfEmpty()
                    where this.accountService.info.RoleIDList.Contains(roleMenuFunction.RoleMenuFunction_RoleID) && function.Function_ByName == "Have"
                    select menu
                ).ToListAsync()
                ;

            var _New_Sys_MenuList = new List<Sys_Menu>();

            for (int i = 0; i < _Sys_MenuList.Count; i++)
            {
                var item = _Sys_MenuList[i];
                this.CheckUpperLevel(_Sys_MenuAllList, _Sys_MenuList, _New_Sys_MenuList, item);
                if (!_New_Sys_MenuList.Any(w => w.Menu_ID == item.Menu_ID)) _New_Sys_MenuList.Add(item);
            }

            return _New_Sys_MenuList.OrderBy(w => w.Menu_Num).ToList();
        }

        private void CheckUpperLevel(List<Sys_Menu> sys_MenuAllList, List<Sys_Menu> old_Sys_MenuList, List<Sys_Menu> new_Sys_MenuList, Sys_Menu menu)
        {
            if (!old_Sys_MenuList.Any(w => w.Menu_ID == menu.Menu_ParentID.ToGuid()) && !new_Sys_MenuList.Any(w => w.Menu_ID == menu.Menu_ParentID))
            {
                var _Menu = sys_MenuAllList.Find(w => w.Menu_ID == menu.Menu_ParentID);
                if (_Menu != null)
                {
                    new_Sys_MenuList.Add(_Menu);
                    this.CheckUpperLevel(sys_MenuAllList, old_Sys_MenuList, new_Sys_MenuList, _Menu);
                }
            }
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="_Sys_MenuList"></param>
        public List<Dictionary<string, object>> CreateMenus(Guid Id, List<Sys_Menu> _Sys_MenuList)
        {
            var _Menus = new List<Dictionary<string, object>>();
            List<Sys_Menu> _MenuItem = null;
            if (Id == Guid.Empty)
                _MenuItem = _Sys_MenuList.Where(w => w.Menu_ParentID == null || w.Menu_ParentID == Guid.Empty).ToList();
            else
                _MenuItem = _Sys_MenuList.Where(w => w.Menu_ParentID == Id).ToList();

            foreach (var item in _MenuItem)
            {
                _Menus.Add(new Dictionary<string, object>
                {
                    ["id"] = item.Menu_ID,
                    ["name"] = item.Menu_Name,
                    ["path"] = item.Menu_Url,
                    ["icon"] = item.Menu_Icon,
                    ["isClose"] = item.Menu_IsClose,
                    ["children"] = this.CreateMenus(item.Menu_ID.ToGuid(), _Sys_MenuList)
                });
            }
            return _Menus;
        }

        /// <summary>
        /// 获取拥有的菜单对象的权限
        /// </summary>
        /// <param name="_Sys_MenuList"></param>
        /// <returns></returns>
        public async Task<List<Dictionary<string, object>>> GetPowerState(List<Sys_Menu> _Sys_MenuList)
        {
            var _Sys_FunctionList = await dbFunction.Query().OrderBy(w => w.Function_Num).ToListAsync();

            var _Sys_MenuFunctionList = await dbMenuFunction.Query().ToListAsync();

            var _Sys_RoleMenuFunctionList = await dbRoleMenuFunction.ToListAsync(w => this.accountService.info.RoleIDList.Contains(w.RoleMenuFunction_RoleID));

            var _PowerStateList = new List<Dictionary<string, object>>();

            if (this.accountService.info.IsSuperManage)
            {
                foreach (var item in _Sys_MenuList)
                {
                    var _PowerState = new Dictionary<string, object>();

                    foreach (var _Sys_Function in _Sys_FunctionList)
                    {
                        var ispower = _Sys_MenuFunctionList.Any(w => w.MenuFunction_MenuID == item.Menu_ID && w.MenuFunction_FunctionID == _Sys_Function.Function_ID);
                        if (_Sys_Function.Function_ByName == "Have" | item.Menu_ParentID == AppConfig.AdminConfig.SysMenuID) ispower = true;
                        _PowerState.Add(_Sys_Function.Function_ByName, ispower);
                    }

                    _PowerState["MenuID"] = item.Menu_ID;
                    _PowerStateList.Add(_PowerState);
                }
                return _PowerStateList;
            }

            foreach (var item in _Sys_MenuList)
            {
                var _PowerState = new Dictionary<string, object>();

                foreach (var _Sys_Function in _Sys_FunctionList)
                {
                    if (this.accountService.info.RoleIDList?.Count > 0)
                    {
                        var ispower = _Sys_RoleMenuFunctionList.Any(w =>
                            this.accountService.info.RoleIDList.Contains(w.RoleMenuFunction_RoleID.ToGuid()) &&
                            w.RoleMenuFunction_MenuID == item.Menu_ID &&
                            w.RoleMenuFunction_FunctionID == _Sys_Function.Function_ID);
                        _PowerState.Add(_Sys_Function.Function_ByName, ispower);
                    }
                    else
                    {
                        _PowerState.Add(_Sys_Function.Function_ByName, false);
                    }
                }
                _PowerState["MenuID"] = item.Menu_ID;
                _PowerStateList.Add(_PowerState);
            }

            return _PowerStateList;
        }

        /// <summary>
        /// 获取查找带回权限
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, object>> GetFindBackPower()
        {
            var res = new Dictionary<string, object>();
            var _Sys_FunctionList = await dbFunction.Query().OrderBy(w => w.Function_Num).ToListAsync();
            foreach (var item in _Sys_FunctionList)
            {
                if (item.Function_ByName == "Have" || item.Function_ByName == "Search")
                {
                    res[item.Function_ByName] = true;

                }
                else
                {
                    res[item.Function_ByName] = false;
                }
            }

            return res;
        }

        /// <summary>
        /// 根据菜单获取权限
        /// </summary>
        /// <param name="MenuId"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, object>> GetPowerStateByMenuId(Guid MenuId)
        {
            var _Sys_Menu = await this.FindByIdAsync(MenuId);

            var _Sys_FunctionList = await dbFunction.Query().OrderBy(w => w.Function_Num).ToListAsync();

            var _Sys_MenuFunctionList = await dbMenuFunction.ToListAsync(w => w.MenuFunction_MenuID == _Sys_Menu.Menu_ID);

            var _Sys_RoleMenuFunctionList = await dbRoleMenuFunction.ToListAsync(w => this.accountService.info.RoleIDList.Contains(w.RoleMenuFunction_RoleID));

            var _PowerState = new Dictionary<string, object>();

            if (this.accountService.info.IsSuperManage)
            {
                foreach (var item in _Sys_FunctionList)
                {
                    var ispower = _Sys_MenuFunctionList.Any(w => w.MenuFunction_MenuID == _Sys_Menu.Menu_ID && w.MenuFunction_FunctionID == item.Function_ID);
                    if (item.Function_ByName == "Have" || _Sys_Menu.Menu_ParentID == AppConfig.AdminConfig.SysMenuID) ispower = true;
                    _PowerState.Add(item.Function_ByName, ispower);
                }
                return _PowerState;
            }

            foreach (var item in _Sys_FunctionList)
            {
                if (this.accountService.info.RoleIDList?.Count > 0)
                {
                    var ispower = _Sys_RoleMenuFunctionList.Any(w =>
                        this.accountService.info.RoleIDList.Contains(w.RoleMenuFunction_RoleID.ToGuid()) &&
                        w.RoleMenuFunction_MenuID == _Sys_Menu.Menu_ID &&
                        w.RoleMenuFunction_FunctionID == item.Function_ID);
                    _PowerState.Add(item.Function_ByName, ispower);
                }
                else
                {
                    _PowerState.Add(item.Function_ByName, false);
                }
            }

            return _PowerState;
        }

        public async Task<Sys_Menu> GetMenuByIdAsync(Guid MenuId)
            => await this.FindAsync(w => w.Menu_ID == MenuId);

        #endregion  左侧菜单

        #region 创建菜单 功能 树

        public async Task<(List<object>, List<Guid>, List<string>)> GetMenuFunctionTreeAsync()
        {
            var _Sys_MenuList = await this.Query().OrderBy(w => w.Menu_Num).ToListAsync();
            var _Sys_FunctionList = await dbFunction.Query().OrderBy(w => w.Function_Num).ToListAsync();
            var _Sys_MenuFunctionList = await dbMenuFunction.Query().OrderBy(w => w.MenuFunction_CreateTime).ToListAsync();
            List<Guid> DefaultExpandedKeys = new List<Guid>();
            List<string> DefaultCheckedKeys = new List<string>();
            var tree = this.CreateMenuFunctionTree(Guid.Empty, _Sys_MenuList, _Sys_FunctionList, _Sys_MenuFunctionList, DefaultExpandedKeys, DefaultCheckedKeys);
            return (tree, DefaultExpandedKeys, DefaultCheckedKeys);
        }

        /// <summary>
        /// 获取菜单与功能树
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="_Sys_MenuList"></param>
        /// <param name="_Sys_FunctionList"></param>
        /// <param name="_Sys_MenuFunctionList"></param>
        /// <param name="_DefaultExpandedKeys"></param>
        /// <param name="_DefaultCheckedKeys"></param>
        /// <returns></returns>
        private List<object> CreateMenuFunctionTree(Guid Id, List<Sys_Menu> _Sys_MenuList, List<Sys_Function> _Sys_FunctionList, List<Sys_MenuFunction> _Sys_MenuFunctionList, List<Guid> _DefaultExpandedKeys, List<string> _DefaultCheckedKeys)
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
                if (_Sys_MenuList.Any(w => w.Menu_ParentID == item.Menu_ID))
                {
                    _DefaultExpandedKeys.Add(item.Menu_ID);

                    _children = this.CreateMenuFunctionTree(item.Menu_ID, _Sys_MenuList, _Sys_FunctionList, _Sys_MenuFunctionList, _DefaultExpandedKeys, _DefaultCheckedKeys);
                }
                else
                {
                    //if (string.IsNullOrWhiteSpace(item.Menu_Url)) continue;
                    //遍历功能
                    foreach (var _Function in _Sys_FunctionList)
                    {
                        //判断是否 该菜单下 是否勾选了 该功能
                        var _Sys_MenuFunction_Any = _Sys_MenuFunctionList.Any(w =>
                         w.MenuFunction_FunctionID == _Function.Function_ID &&
                         w.MenuFunction_MenuID == item.Menu_ID);

                        var id = $"{item.Menu_ID}${_Function.Function_ID}";
                        if (_Sys_MenuFunction_Any) _DefaultCheckedKeys.Add(id);

                        _children.Add(new
                        {
                            key = id,
                            title = $"{_Function.Function_Name}-{_Function.Function_ByName}-{ _Function.Function_Num }",
                            disabled = true,
                            children = new ArrayList()
                        });
                    }

                }

                _Menus.Add(new
                {
                    key = item.Menu_ID,
                    title = $"{item.Menu_Name}-{item.Menu_Num}",
                    disableCheckbox = true,
                    children = _children
                });
            }

            return _Menus;
        }

        #endregion

    }
}
