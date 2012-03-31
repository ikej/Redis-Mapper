using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedisMapper
{
    public interface IRedisCustomProperty
    {
        object Value { get; set; }
        bool IsQueriable { get; set; }
    }
}
