using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Youle.Mobile.Core;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    [Serializable]
    public class Category : RedisModelBase
    {
        public string Name { get; set; }

        [QueryOrSortField]
        public string ParentId { get; set; }

        [QueryOrSortField]
        public string status { get; set; }

    }
}
