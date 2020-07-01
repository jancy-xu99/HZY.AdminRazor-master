using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZY.Toolkit.ToolKits
{
    public static class Culture
    {
        public static CultureInfo Current
        {
            [DebuggerStepThrough]
            get
            {
                return CultureInfo.CurrentUICulture;
            }
        }

        public static CultureInfo Invariant
        {
            [DebuggerStepThrough]
            get
            {
                return CultureInfo.InvariantCulture;
            }
        }
    }
}
