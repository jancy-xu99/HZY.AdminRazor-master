using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Admin.Dto.Sys
{
    using Microsoft.AspNetCore.Mvc;
    using AutoMapper;
    using HZY.Admin.Dto.Core;
    using HZY.Models.Sys;

    public class Sys_UserDto
    {
        public List<Guid> RoleIds { get; set; }

        public Sys_User Model { get; set; }

        // public Sys_User To_Sys_User => this.MapTo<Sys_UserDto, Sys_User>();


    }
}
