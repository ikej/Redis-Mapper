using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    [Serializable]
    public class Element:RedisModelBase
    {
        [Required]
        public string DisplayName { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
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

        private bool _isQueriable = true;
        public bool IsQueriable
        {
            get
            {
                return _isQueriable;
            }
            set
            {
                _isQueriable = value;
            }
        }

        public List<ElementDetail> ElementDetails { get; set; }

    }
}
