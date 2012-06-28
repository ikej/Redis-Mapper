using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    /// <summary>
    /// Which platform to run the application
    /// </summary>
    public enum PlatformType
    {
        Java = 1,
        Lua = 2,
        C = 4,
        Txt = 3,
        Android = 8
    }
}
