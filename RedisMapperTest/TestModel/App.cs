using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RedisMapper;

namespace RedisMapperTest.TestModel
{
    [Serializable]
    public class App : RedisModelBase, IRedisModelWithSubModel
    {
        [QueryOrSortField]
        public string Name { get; set; }

        [QueryOrSortField]
        public Double Price { get; set; }

        public string Summary { get; set; }

        public int SummaryVer { get; set; }

        public string StorageName { get; set; }

        public string Drive { get; set; }

        public int IsHide { get; set; }

        [QueryOrSortField]
        public int Status { get; set; }

        public string CurrentVer { get; set; }

        public Dictionary<string, object> CustomProperties { get; set; }

        public string AppProjectId { get; set; }
        public string AppNo { get; set; }
    }
}
