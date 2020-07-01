using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZY.Toolkit.ToolKits
{
    public static class SystemTime
    {
        private static Func<DateTime> now = () => DateTime.UtcNow;

        public static DateTime Now
        {
            [DebuggerStepThrough]
            get
            {
                return now();
            }

            [DebuggerStepThrough]
            set
            {
                now = () => value;
            }
        }
    }
}
