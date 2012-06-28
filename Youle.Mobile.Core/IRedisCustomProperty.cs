using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Youle.Mobile.Core
{
    public interface IRedisCustomProperty
    {
        object Value { get; set; }
        bool IsQueriable { get; set; }
    }
}
