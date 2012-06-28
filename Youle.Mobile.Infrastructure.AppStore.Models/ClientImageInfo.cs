using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    [Serializable]
    public class ImageInfo : RedisModelBase
    {
        public string BelongsToAppId { get; set; }

        private string _fileUrl = string.Empty;
        public string FileUrl
        {
            get 
            {
                return _fileUrl;
            }
            set
            {
                _fileUrl = value;
            }
        }

        public string FileName 
        {
            get
            {
                return Path.GetFileName(FileUrl);
            }
        }

        public string Extension 
        {
            get
            {
                return Path.GetExtension(FileUrl);
            }
        }


    }
}
