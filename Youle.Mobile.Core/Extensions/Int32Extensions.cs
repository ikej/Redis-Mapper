using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Youle.Mobile.Core
{
    public static class Int32Extensions
    {
        public static T ToEnum<T>(this int value, T defaultValue) 
        {
            var type = typeof(T);
            if (type.IsEnum)
            {
                if (Enum.IsDefined(type, value))
                    return (T)Enum.Parse(type, value.ToString());
            }
            return defaultValue;
        }


        public static Boolean ToBoolean(this int trueOrFalse)
        {
            return trueOrFalse == 1;
        }
    }
}
