using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Youle.Mobile.Core;
using System.ComponentModel.DataAnnotations;


namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    [Serializable]
    public class ElementDetail:RedisModelBase
    {
        [QueryOrSortField]
        public string ElementId { get; set; }

        [Required]
        public object Value { get; set; }

        private int _status = 1;
        public int Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }
    }
}
