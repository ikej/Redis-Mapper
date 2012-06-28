using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    public class DownloadAPKPushItem : RedisModelBase
    {
        public string ApkUrl { get; set; }

        public string Name { get; set; }

        public string VersionCode { get; set; }

        public string VersionName { get; set; }

        public string PackageName { get; set; }

        public bool AutoInstall { get; set; }
    }
}
