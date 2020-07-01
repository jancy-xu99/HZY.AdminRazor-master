using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HZY.Admin.Controllers
{
    using HZY.Admin.Services.Sys;

    public class HomeController : ApiBaseController<AccountService>
    {
        public HomeController(Sys_MenuService _menuService, AccountService _accountService)
            : base("0b7f8e2c-9faa-4496-9068-80b87ba1b64e", _menuService, _accountService)
        {

        }

        [HttpGet(nameof(Index)), Route(""), Route("/")]
        public async Task<IActionResult> Index()
        {
            var allList = await this.menuService.GetMenuByRoleIDAsync();

            ViewData["menuList"] = this.menuService.CreateMenus(Guid.Empty, allList);
            ViewData["UserName"] = this.service.info.UserName;

            return View();
        }

        [HttpGet(nameof(Main))]
        public IActionResult Main()
        {
            return View();
        }

    }
}
