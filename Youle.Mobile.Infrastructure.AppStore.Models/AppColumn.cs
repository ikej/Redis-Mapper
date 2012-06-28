using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    [Serializable]
    public class AppColumn : AppList
    {
        public int ClientId { get; set; }

        public string ParentId { get; set; }

        public string ParentName { get; set; } 
    }
}
