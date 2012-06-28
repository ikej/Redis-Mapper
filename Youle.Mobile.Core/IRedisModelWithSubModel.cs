using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Youle.Mobile.Core
{
    public interface IRedisModelWithSubModel
    {
        Dictionary<string, object> CustomProperties { get; set; }
    }
}
