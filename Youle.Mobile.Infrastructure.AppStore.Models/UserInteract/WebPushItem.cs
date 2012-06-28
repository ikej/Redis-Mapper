using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    public class WebPushItem :RedisModelBase
    {
        public string Url { get; set; }

        public bool IsDirect { get; set; }

        public string PromptMessage { get; set; }
    }
}
