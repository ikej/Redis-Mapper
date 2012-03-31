using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedisMapper
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
                return RedisService.Redis.GetValues<T>(ids);
            }
            else
            {
                return RedisService.Redis.GetValues<T>(ids);
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
