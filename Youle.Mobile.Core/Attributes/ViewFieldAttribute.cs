using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Youle.Mobile.Core
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ViewFieldAttribute : Attribute
    {
        public ViewFieldAttribute()
        {

        }

        public string DisplayName { get; set; }

        public bool IsDisplay
        {
            get
            {
                return _isDisplay;
            }
            set
            {
                _isDisplay = value;
            }
        } private bool _isDisplay = true;
    }

}
