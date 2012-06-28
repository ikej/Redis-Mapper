using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Youle.Mobile.Core;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    [Serializable]
    public class AppSettingsForAppList : IRedisModel
    {
        //BelongsToAppId
        public string Id { get; set; }

        public DateTime CreateDateTime { get; set; }

        // the smaller, the higher priority
        public int ScoreForSort { get; set; }

        public Dictionary<string, object> CustomProperties { get; set; }


        public string ModuleName
        {
            get;
            set;
        }
    }
}
