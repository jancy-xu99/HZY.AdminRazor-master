using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HZY.Admin.Controllers
{
    using HZY.Toolkit;
    using HZY.Admin.Core;
    using HZY.Admin.Dto.Sys;
    using HZY.Admin.Services.Sys;

    /// <summary>
    /// 授权
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(ApiVersionsEnum.Admin))]
    public class AuthorizationController : BaseController
    {
        protected readonly AccountService accountService;

        public AuthorizationController(AccountService _accountService)
        {
            this.accountService = _accountService;

        }

        [HttpGet(nameof(Index))]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpGet(nameof(Out))]
        public IActionResult Out()
        {
            this.accountService.RemoveToken();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// 检查帐户并获取 token
        /// </summary>
        /// <param name="authUserDto">Dto</param>
        /// <returns></returns>
        [HttpPost(nameof(Check))]
        public async Task<ApiResult> Check([FromBody]AuthUserDto authUserDto)
        {
            var _token = await this.accountService.CheckedAsync(authUserDto.UserName, authUserDto.UserPassword, authUserDto.LoginCode);

            var _tokenType = "Bearer ";

            return this.ResultOk(new
            {
                token = _token,
                tokenType = _tokenType
            });
        }


    }
}