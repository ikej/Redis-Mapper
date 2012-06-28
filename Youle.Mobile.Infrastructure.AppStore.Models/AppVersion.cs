using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    [Serializable]
    public class AppVersion : RedisModelBase
    {
        [Required]
        public string FileUrl { get; set; }

        public DateTime PublishDateTime { get; set; }

        public int UpdateTypeId { get; set; }

        public int FileSize { get; set; }

        public string Tip { get; set; }

        public int Status { get; set; }

        public string VersionName { get; set; }

    }
}
