using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HZY.Admin.Controllers.Sys
{
    using HZY.Toolkit;
    using HZY.Models.Sys;
    using HZY.Admin.Dto.Sys;
    using HZY.Admin.Services.Sys;
    using HZY.Toolkit.HzyNetCoreUtil.Attributes;

    public class UserController : ApiBaseController<Sys_UserService>
    {
        public UserController(Sys_MenuService _menuService, Sys_UserService _service)
            : base("38d864ff-f6e7-43af-8c5c-8bbcf9fa586d", _menuService, _service)
        {

        }

        #region 页面 Views

        [HttpGet(nameof(Index))]
        public IActionResult Index() => View();

        [HttpGet("Info/{Id?}")]
        public IActionResult Info(Guid Id) => View(Id);

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
        public async Task<ApiResult> FindListAsync(int Page, int Rows, [FromBody]Sys_User Search)
            => this.ResultOk(await this.service.FindListAsync(Page, Rows, Search));

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        [AppTransaction]
        [HttpPost("Save"), Core.HZYAppCheckModel]
        public async Task<ApiResult> SaveAsync(Sys_UserDto Model)
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

        #region 导出 Excel

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="Search"></param>
        /// <returns></returns>
        [HttpPost("ExportExcel")]
        public async Task<FileContentResult> ExportExcel([FromBody] Sys_User Search)
            => this.File(await this.service.ExportExcel(Search), Tools.GetFileContentType[".xls"].ToStr(), $"{Guid.NewGuid()}.xls");

        #endregion

        #region 其他

        /// <summary>
        /// 获取用户权限
        /// </summary>
        /// <param name="MenuId"></param>
        /// <returns></returns>
        [HttpPost("GetPowerState/{MenuId}")]
        public async Task<ApiResult> GetPowerState(Guid MenuId)
        {
            if (MenuId == Guid.Empty) MessageBox.Show("参数MenuId不能为空！");
            return this.ResultOk(new { powerState = await this.menuService.GetPowerStateByMenuId(MenuId) });
        }

        #endregion

    }
}