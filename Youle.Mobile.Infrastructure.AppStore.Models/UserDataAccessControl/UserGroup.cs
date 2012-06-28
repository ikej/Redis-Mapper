using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Youle.Mobile.Core;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    [Serializable]
    public class UserGroup : RedisModelBase
    {
        [Required]
        [QueryOrSortField]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
