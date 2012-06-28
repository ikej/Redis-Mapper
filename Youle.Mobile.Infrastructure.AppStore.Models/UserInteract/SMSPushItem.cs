using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    public class SMSPushItem: RedisModelBase
    {

        public string From { get; set; }

        public string Content { get; set; }
    }
}
