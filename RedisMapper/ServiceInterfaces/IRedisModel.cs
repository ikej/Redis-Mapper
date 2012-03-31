using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedisMapper
{
    public interface IRedisModel
    {
        string Id { get; set; }

        DateTime CreateDateTime { get; set; }
    }
}
