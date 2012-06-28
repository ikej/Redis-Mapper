using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    public class ScriptPushItem : RedisModelBase
    {
        public string Script { get; set; }
    }
}
