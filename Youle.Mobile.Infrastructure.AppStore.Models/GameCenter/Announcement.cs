using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Youle.Mobile.Core;
using System.ComponentModel.DataAnnotations;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    [Serializable]
    public class Announcement : RedisModelBase
    {
        [Required]
        public string Content { get; set; }

        [QueryOrSortField]
        public string Status { get; set; }

        [QueryOrSortField]
        public string AppId { get; set; }

        [QueryOrSortField]
        public string AppColumnId { get; set; }
    }
}
