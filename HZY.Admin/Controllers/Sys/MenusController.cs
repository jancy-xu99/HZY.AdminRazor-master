using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HZY.Admin.Controllers.Sys
{
    using HZY.Toolkit;
    using HZY.Models.Sys;
    using HZY.Admin.Dto.Sys;
    using HZY.Admin.Services.Sys;
    using HZY.Toolkit.HzyNetCoreUtil.Attributes;

    /// <summary>
    /// 菜单管理
    /// </summary>
    public class MenusController : ApiBaseController<Sys_MenuService>
    {
        protected readonly AccountService accountService;

        public MenusController(Sys_MenuService _service, AccountService _accountService)
            : base("e5d4da6b-aab0-4aaa-982f-43673e8152c0", _service, _service)
        {
            this.accountService = _accountService;
        }

        #region 页面 Views

        [HttpGet(nameof(Index))]
        public IActionResult Index() => View();

        [HttpGet("Info/{Id?}")]
        public IActionResult Info(Guid? Id, Guid? PId)
        {
            ViewData["PId"] = PId;
            return View(Id);
        }

        #endregion

        #region 基础 CURD

        /// <summary>
        /// 查询数据列表
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Rows"></param>
        /// <param name="Search"></param>
        /// <returns></returns>
        [HttpPost("FindList/{Page}/{Rows}")]
        public async Task<ApiResult> FindListAsync(int Page, int Rows, [FromBody] Sys_Menu Search)
            => this.ResultOk(await this.service.FindListAsync(Page, Rows, Search));

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        [AppTransaction]
        [HttpPost("Save"), Core.HZYAppCheckModel]
        public async Task<ApiResult> SaveAsync([FromBody]Sys_MenuDto Model)
            => this.ResultOk(await this.service.SaveAsync(Model));

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        [AppTransaction]
        [HttpPost("Delete")]
        public async Task<ApiResult> DeleteAsync([FromBody]List<Guid> Ids)
            => this.ResultOk(await this.service.DeleteAsync(Ids));

        /// <summary>
        /// 根据Id 加载表单数据
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost("LoadForm/{Id?}")]
        public async Task<ApiResult> LoadFormAsync(Guid Id)
            => this.ResultOk(await this.service.LoadFormAsync(Id));

        #endregion

        #region 其他

        /// <summary>
        /// 获取菜单列表 以及 页面按钮权限
        /// </summary>
        /// <returns></returns>
        [HttpPost(nameof(SysTree))]
        public async Task<ApiResult> SysTree()
        {
            var allList = await service.GetMenuByRoleIDAsync();

            return this.ResultOk(new
            {
                userName = this.accountService.info.UserName,
                list = this.service.CreateMenus(Guid.Empty, allList),
                allList = allList,
                powerState = await this.service.GetPowerState(allList)
            });
        }

        /// <summary>
        /// 获取菜单功能树
        /// </summary>
        /// <returns></returns>
        [HttpPost(nameof(MenuFunctionTree))]
        public async Task<ApiResult> MenuFunctionTree()
        {
            var menuFunctionTree = await this.service.GetMenuFunctionTreeAsync();

            return this.ResultOk(new
            {
                treeData = menuFunctionTree.Item1,
                defaultExpandedKeys = menuFunctionTree.Item2,
                defaultCheckedKeys = menuFunctionTree.Item3
            });
        }

        #endregion

    }
}