using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedisMapper
{
    [Serializable]
    public class RedisModelBase : IRedisModel
    {
        public string Id { get; set; }

        [QueryOrSortField]
        public DateTime CreateDateTime { get; set; }
    }
}
