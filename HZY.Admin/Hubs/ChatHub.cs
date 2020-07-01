using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HZY.Admin.Hubs
{
    using HZY.EFCore.Repository;
    using HZY.Admin.Services.Sys;
    using Microsoft.AspNetCore.SignalR;

    public class ChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task Register(string userId)
        {
            var user = this.Clients.User(userId);
            await user.SendAsync("Register", "注册成功!");
        }

        public async Task SendMessage(string user, string message)
        {
            var context = this.Context.GetHttpContext();
            var token = AccountService.GetToken(context);

            await Clients.All.SendAsync("ReceiveMessage", user, $"{message}  token = {token}");
        }
    }



}
