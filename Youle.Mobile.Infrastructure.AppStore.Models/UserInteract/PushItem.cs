using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Youle.Mobile.Core;
using System.ComponentModel.DataAnnotations;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    public class PushItem : RedisModelBase
    {
        [QueryOrSortField]
        [Required]
        public string Name { get; set; }

        [Required]
        public string ConfigId { get; set; }

        [QueryOrSortField]
        [Required]
        public int CommandType { get; set; }

        public string CommandId { get; set; }
    }
}
