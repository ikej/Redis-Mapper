using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedisMapper
{
    public static class TypeExtensions
    {
        public static bool IsEnumerableType(this Type type)
        {
            foreach (var interfaceType in type.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                    && interfaceType.GetGenericArguments().Length == 1 && !interfaceType.GetGenericArguments()[0].Equals(typeof(char)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
