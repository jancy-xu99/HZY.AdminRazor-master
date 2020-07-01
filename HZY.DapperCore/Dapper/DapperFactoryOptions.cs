using System;
using System.Collections.Generic;

namespace HZY.DapperCore.Dapper
{
    public class DapperFactoryOptions
    {
        public IList<Action<ConnectionConfig>> DapperActions { get; } = new List<Action<ConnectionConfig>>();
    }
}