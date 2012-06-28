using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Youle.Mobile.Core;
using System.ComponentModel.DataAnnotations;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    [Serializable]
    public class AppList : RedisModelBase
    {
        [QueryOrSortField]
        [Required]
        public string Name { get; set; }
     
        public int CurrentVersion { get; set; }
    }
}
