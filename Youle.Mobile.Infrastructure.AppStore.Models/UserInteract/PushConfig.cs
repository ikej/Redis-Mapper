using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    public class PushConfig: RedisModelBase
    {
        [Required]
        public string Name { get; set; }

        public int? DelaySeconds { get; set; }

        [Required]
        public int ExecuteTimes { get; set; }

        [Required]
        public int IntervalSeconds { get; set; }

        [Required]
        public DateTime AllowedPeriodFrom { get; set; }

        [Required]
        public DateTime AllowedPeriodTo { get; set; }

        public int? SuccessGotoActionId { get; set; }

        public int? FailGotoActionId { get; set; }
    }
}
