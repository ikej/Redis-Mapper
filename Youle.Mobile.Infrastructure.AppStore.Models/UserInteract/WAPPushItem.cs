using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    public class WAPPushItem :RedisModelBase
    {
        public string AvailableNetworkType { get; set; }

        public string RealUrl { get; set; }

        public string From { get; set; }

        public string Content { get; set; }
    }
}
