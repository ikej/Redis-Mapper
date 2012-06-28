using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    [Serializable]
    public class ClientImageInfo : ImageInfo
    {
        public string TypeId { get; set; }

        public string TypeDescription { get; set; }

    }
}
