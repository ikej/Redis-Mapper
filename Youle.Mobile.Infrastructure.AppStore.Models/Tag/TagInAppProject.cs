using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Youle.Mobile.Core;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    [Serializable]
    public class TagInAppProject : IRedisModelBase
    {
        public string Id
        {
            get;
            set;
        }
    }
}
