using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Youle.Mobile.Core
{
    public interface IDelimiter
    {
        string Property { get; set; }

        string Line { get; set; }

        string Third { get; set; }

        bool AppendDelimiterDefinitions { get; set; }

        bool? AppendPropertyNames { get; set; }
    }
}
