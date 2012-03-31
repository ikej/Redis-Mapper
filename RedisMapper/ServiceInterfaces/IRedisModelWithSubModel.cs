using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedisMapper
{
    public interface IRedisModelWithSubModel
    {
        Dictionary<string, object> CustomProperties { get; set; }
    }
}
