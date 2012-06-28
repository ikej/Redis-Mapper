using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Youle.Mobile.Core;
using System.ComponentModel.DataAnnotations;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    [Serializable]
    public class App : RedisModelBase, IRedisModelWithSubModel
    {
        [Required]
        [QueryOrSortField]
        public string Name { get; set; }

        [QueryOrSortField]
        public Double Price { get; set; }

        private ImageInfo _logo = new ImageInfo();
        public ImageInfo Logo
        {
            get
            {
                return _logo;
            }
            set
            {
                _logo = value;
            }
        }

        private List<ImageInfo> _screenShot = new List<ImageInfo>();
        public List<ImageInfo> ScreenShot 
        {
            get
            {
                return _screenShot;
            }
            set
            {
                _screenShot = value;
            }
        }

        private List<ImageInfo> _screenShotLarger = new List<ImageInfo>();
        public List<ImageInfo> ScreenShotLarger
        {
            get
            {
                return _screenShotLarger;
            }
            set
            {
                _screenShotLarger = value;
            }
        }

        private List<ClientImageInfo> _clientLogos = new List<ClientImageInfo>();
        public List<ClientImageInfo> ClientLogos
        {
            get
            {
                return _clientLogos;
            }
            set
            {
                _clientLogos = value;
            }
        }

        public string Summary { get; set; }

        public int SummaryVer { get; set; }
         
        public string StorageName { get; set; }

        public string Drive { get; set; }

        public int IsHide { get; set; }
  
        [QueryOrSortField]
        public int Status { get; set; }

        [QueryOrSortField]
        public int PlatformType { get; set; }

        public string CurrentVer { get; set; }

        public bool UseGreaterVersion { get; set; }

        public string CurrentTestVersion { get; set; }

        private Dictionary<string, object> _customProperties = new Dictionary<string, object>();
        public Dictionary<string, object> CustomProperties 
        { 
            get
            {
                return _customProperties;
            }
            set
            {
                _customProperties = value;
            }
        }

        public string AppProjectId { get; set; }
        [Required]
        public string AppNo { get; set; }

        public int Rank { get; set; }

        public string Comment { get; set; }

        public string CategoryId { get; set; } // UI input

        public string CategoryName { get; set; }// UI input

        public string SubCategoryId { get; set; }// UI input

        public string ProductNo { get; set; }// UI input

        public int ReviewCount { get; set; }

        public List<string> PermittedIMEIs { get; set; }

        public int OrderNumber { get; set; }

    }
}
