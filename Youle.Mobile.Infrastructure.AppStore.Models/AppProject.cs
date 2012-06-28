using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Youle.Mobile.Core;
using System.ComponentModel.DataAnnotations;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    [Serializable]
    public class AppProject : AppList
    {
        [QueryOrSortField]
        [Required]
        public string Creator { get; set; }

        public string LogoFile { get; set; }

        [QueryOrSortField]
        [Required]
        public string AppNo { get; set; }

        [QueryOrSortField]
        public float Rate { get; set; }

        [QueryOrSortField]
        public int ReviewCount { get; set; }

        [QueryOrSortField]
        public string PackageName { get; set; }
    }
}
