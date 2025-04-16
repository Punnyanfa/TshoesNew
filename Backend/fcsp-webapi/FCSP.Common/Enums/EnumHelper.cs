using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCSP.Common.Enums
{
    public static class EnumHelper
    {
        public static string GetEnumName<TEnum>(int value) where TEnum : Enum
        {
            if (Enum.IsDefined(typeof(TEnum), value))
            {
                return Enum.GetName(typeof(TEnum), value) ?? "Unknown";
            }
            return $"Invalid {typeof(TEnum).Name} value: {value}";
        }
    }
}
