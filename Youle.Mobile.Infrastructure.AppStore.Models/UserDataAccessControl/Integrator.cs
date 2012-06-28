using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Youle.Mobile.Core;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    [Serializable]
    public class Integrator : RedisModelBase
    {
        [Required]
        [QueryOrSortField]
        [RegularExpression(@"^[a-z0-9A-Z]+$", ErrorMessage = "集成商名称必须是英文或数字的组合")]
        public string Name { get; set; }

        public bool IsQuickUpdate { get; set; }

        public string Description { get; set; }
    }
}
