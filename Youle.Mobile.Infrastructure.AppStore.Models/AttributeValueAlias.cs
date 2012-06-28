using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Youle.Mobile.Core;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    [Serializable]
    public class AttributeValueAlias : RedisModelBase
    {
        [QueryOrSortField]
        public string Value { get; set; }

        public List<string> AliasValues { get; set; } 
    }
}
