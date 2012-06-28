using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Youle.Mobile.Core;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    [Serializable]
    public class Tag : RedisModelBase
    {
        [QueryOrSortField]
        public string Name { get; set; }
    }
}
