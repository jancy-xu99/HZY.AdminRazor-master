using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HZY.Admin.Controllers.Base
{
    using HZY.Admin.Hubs;
    using Microsoft.AspNetCore.SignalR;

    public class SignalRChatController : Controller
    {
        public readonly IHubContext<ChatHub> chatHub;
        public SignalRChatController(IHubContext<ChatHub> _chatHub)
        {
            this.chatHub = _chatHub;
        }

        public IActionResult Index()
        {
            return View();
        }












    }
}