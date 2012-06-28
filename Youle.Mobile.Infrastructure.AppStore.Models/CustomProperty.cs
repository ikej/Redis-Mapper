using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Youle.Mobile.Core;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    [Serializable]
    public class CustomProperty : RedisModelBase, IRedisCustomProperty
    {
        public object Value { get; set; }

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
    }
}
