using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Admin.Dto.Sys
{
    public class Sys_RoleMenuFunctionDto
    {

        public Guid RoleId { get; set; }

        public Guid MenuId { get; set; }

        public List<Guid> FunctionIds { get; set; }


    }
}
