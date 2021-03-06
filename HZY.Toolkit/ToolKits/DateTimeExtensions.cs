﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZY.Toolkit.ToolKits
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime MinDate = new DateTime(1900, 1, 1);
        private static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);

        [DebuggerStepThrough]
        public static bool IsValid(this DateTime target)
        {
            return (target >= MinDate) && (target <= MaxDate);
        }
    }
}
