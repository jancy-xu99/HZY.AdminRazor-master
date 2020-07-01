using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HZY.Admin.Controllers.Sys
{
    using HZY.Toolkit;
    using HZY.Admin.Dto.Sys;
    using HZY.Admin.Services.Sys;

    public class ChangePasswordController : ApiBaseController<AccountService>
    {
        public ChangePasswordController(Sys_MenuService _menuService, AccountService _service)
            : base("f35d64ae-ecb7-4d06-acfb-d91595966d9e", _menuService, _service)
        {

        }

        #region 页面 Views

        [HttpGet(nameof(Index))]
        public IActionResult Index() => View();

        #endregion

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost("Save")]
        public async Task<ApiResult> UpdatePassword([FromBody]UpdatePasswordDto Model)
            => this.ResultOk(await this.service.ChangePwd(Model.OldPwd, Model.NewPwd));


    }
}