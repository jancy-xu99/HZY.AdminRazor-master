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

    public class RoleFunctionController : ApiBaseController<Sys_RoleMenuFunctionService>
    {
        protected readonly Sys_RoleService roleService;

        public RoleFunctionController(Sys_MenuService _menuService, Sys_RoleMenuFunctionService _srevice, Sys_RoleService _roleService)
            : base("bd024f3a-99a7-4407-861c-a016f243f222", _menuService, _srevice)
        {
            this.roleService = _roleService;
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
        public async Task<ApiResult> FindListAsync(int Page, int Rows, [FromBody] Sys_Role Search)
            => this.ResultOk(await this.roleService.FindListAsync(Page, Rows, Search));

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        [AppTransaction]
        [HttpPost("Save"), Core.HZYAppCheckModel]
        public async Task<ApiResult> SaveAsync([FromBody]Sys_RoleMenuFunctionDto Model)
            => this.ResultOk(await this.service.SaveAsync(Model));

        ///// <summary>
        ///// 删除数据
        ///// </summary>
        ///// <param name="idsDto"></param>
        ///// <returns></returns>
        //[HttpPost("Delete")CheckFilter]
        //public async Task<ApiResult> DeleteAsync([FromBody]IdsDto<Guid> idsDto)
        //{
        //    await this.srevice.DeleteAsync(idsDto);
        //    return this.ResultOk();
        //}

        ///// <summary>
        ///// 根据Id 加载表单数据
        ///// </summary>
        ///// <param name="Id"></param>
        ///// <returns></returns>
        //[HttpPost("LoadForm/{Id}")CheckFilter]
        //public async Task<ApiResult> LoadFormAsync(Guid Id)
        //{
        //    await this.srevice.LoadFormAsync(Id);
        //    return this.ResultOk();
        //}

        #endregion

        #region 其他

        /// <summary>
        /// 获取菜单功能树
        /// </summary>
        /// <returns></returns>
        [HttpPost("RoleMenuFunctionTree/{RoleId}")]
        public async Task<ApiResult> RoleMenuFunctionTree(Guid RoleId)
        {
            var tuple = await this.service.GetRoleMenuFunctionTreeAsync(RoleId);

            return this.ResultOk(new
            {
                expandedRowKeys = tuple.Item1,
                list = tuple.Item2
            });
        }

        #endregion



    }
}