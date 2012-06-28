using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Youle.Mobile.Core;
using System.ComponentModel.DataAnnotations;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    [Serializable]
    public class DeviceModel : RedisModelBase, IRedisModelWithSubModel
    {
        [Required]
        [QueryOrSortField]
        public string ModelName { get; set; }

        public string Comment { get; set; }

        // sub model
        public Dictionary<string, object> CustomProperties { get; set; }

        // colomn keys | AppList ids 
        private Dictionary<string, string> _columns = new Dictionary<string, string>();
        public Dictionary<string, string> Columns
        {
            get
            {
                return _columns;
            }
            set
            {
                _columns = value;
            }
        }

        private List<string> _parentColumns = new List<string>();
        public List<string> ParentColumns
        {
            get
            {
                return _parentColumns;
            }
            set
            {
                _parentColumns = value;
            }
        }

        private List<DeviceModelItem> _children = new List<DeviceModelItem>();
        public List<DeviceModelItem> Children
        {
            get
            {
                return _children;
            }
            set
            {
                _children = value;
            }
        }

        public string ParentId { get; set; }
    }

    [Serializable]
    public class DeviceModelItem
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
