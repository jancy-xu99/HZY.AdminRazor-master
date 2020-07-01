using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HZY.Admin.Controllers.Sys
{
    using HZY.Toolkit;
    using HZY.Models.Sys;
    using HZY.Admin.Services.Sys;

    /// <summary>
    /// 功能管理
    /// </summary>
    public class FunctionController : ApiBaseController<Sys_FunctionService>
    {
        public FunctionController(Sys_MenuService _menuService, Sys_FunctionService _service)
            : base("d721fc55-2174-40eb-bb37-5c54a158525a", _menuService, _service)
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
        public async Task<ApiResult> FindListAsync(int Page, int Rows, [FromBody] Sys_Function Search)
            => this.ResultOk(await this.service.FindListAsync(Page, Rows, Search));

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        [HttpPost("Save"), Core.HZYAppCheckModel]
        public async Task<ApiResult> SaveAsync([FromBody]Sys_Function Model)
            => this.ResultOk(await this.service.SaveAsync(Model));

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        public async Task<ApiResult> DeleteAsync([FromBody] List<Guid> Ids)
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







    }
}