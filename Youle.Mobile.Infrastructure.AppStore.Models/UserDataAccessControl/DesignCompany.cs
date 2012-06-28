using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Youle.Mobile.Core;
using System.ComponentModel.DataAnnotations;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    [Serializable]
    public class DesignCompany : RedisModelBase
    {
        [Required]
        [QueryOrSortField]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
