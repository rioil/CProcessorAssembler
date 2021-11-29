using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CProcessorAssembler.Utils
{
    internal static class StringUtil
    {
        public static int ParseNumericString(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) {
                throw new FormatException();
            }

            if (value.StartsWith("0x")) {
                return Convert.ToInt32(value, 16);
            }
            else if (value.StartsWith("0b")) {
                return Convert.ToInt32(value, 2);
            }
            else {
                return Convert.ToInt32(value);
            }
        }
    }
}
