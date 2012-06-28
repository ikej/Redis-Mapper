using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Youle.Mobile.Core;

namespace Youle.Mobile.RedisMapper
{
    public static class RedisServiceExtensions
    {
        public static List<T> IdsToValues<T>(this List<string> ids, bool needKeyFormat = false)
        {
            if (needKeyFormat)
            {
                for (int i = 0; i < ids.Count; i++)
                {
                    ids[i] = RedisKeyFactory.ModelKey<T>(ids[i]);
                }
            }
           
            return GetValues<T>(ids);
        }

        public static List<T> FilterByUIMode<T>(this List<T> modelList, List<string> allowedModes)
            where T:IRedisModel
        {
            List<T> ret = new List<T>();
            if (modelList == null)
            {
                return ret;
            }

            foreach (var m in modelList)
            {
                var modName = m.ModuleName == null ? string.Empty : m.ModuleName;
                if (allowedModes.Contains(modName))
                {
                    ret.Add(m);
                }
            }

            return ret;
        }

        private static List<T> GetValues<T>(List<string> ids)
        {
            using (var Redis = RedisService.RedisClientManager.GetReadOnlyClient())
            {
                return Redis.GetValues<T>(ids);
            }
        }

        public static PagedList<T> ToPagedList<T>(this List<T> models, int totalCount, int pageNum, int pageSize)
        {
            PagedList<T> pList = new PagedList<T>(models, totalCount, pageNum, pageSize);
            return pList;
        }

        public static List<string> ToIdsWithNewPrefix<TOrigin, TUpdate>(this List<string> ids)
        {
            for (int i = 0; i < ids.Count; i++)
            {
                ids[i] = ids[i].Replace(typeof(TOrigin).Name, typeof(TUpdate).Name);
            }
            return ids;
        }

        public static List<string> ToIdsWithNoPrefix<TOrgin>(this List<string> ids)
        {
            for (int i = 0; i < ids.Count; i++)
            {
                ids[i] = ids[i].Replace(typeof(TOrgin).Name + ":", string.Empty);
            }
            return ids;
        }

        public static List<string> FilterByType<T>(this List<string> ids)
        {
            List<string> filtedIds = new List<string>();
            for (int i = 0; i < ids.Count; i++)
            {
                if (ids[i].Contains(typeof(T).Name + ":"))
                {
                    filtedIds.Add(ids[i]);
                }
            }
            return filtedIds;
        }
        
    }
}
