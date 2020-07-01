using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HZY.Admin.Controllers.Base
{
    using HZY.Admin.Services.Sys;

    public class OtherController : ApiBaseController
    {
        public OtherController(Sys_MenuService _menuService)
            : base("7c34c2fd-98ed-4655-aa04-bb00b915a751", _menuService)
        {

        }

        #region 页面 Views

        [HttpGet(nameof(Index))]
        public IActionResult Index()
        {
            return View();
        }

        #endregion




    }
}