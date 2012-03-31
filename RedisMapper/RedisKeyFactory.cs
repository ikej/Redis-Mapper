using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedisMapper
{
    public class RedisKeyFactory
    {
        const string KEY_PATTERN = "{0}:{1}";
        const string QUERY_WITH_PROPERTY_AND_VALUE_KEY_PATTERN = "Query:{0}:{1}:{2}";
        const string QUERY_WITH_PROPERTY_KEY_PATTERN = "Query:{0}:{1}";
        const string SUBMODEL_KEY_PATTERN = "SubModel:{0}:{1}";
        const string LIST_ALL = "ListAll:{0}";
        const string LIST_ALL_NUM = "ListAllNum:{0}";
        const string NEXT_KEY_NUM = "NextKey:{0}";
        const string WAITING_QUEUE = "Queue:{0}:{1}";

        public static string QueueKey<T>(string queueName)
        {
            return string.Format(WAITING_QUEUE, typeof(T).Name, queueName);
        }

        public static string ModelKey<T>(string id)
        {
            return string.Format(KEY_PATTERN, typeof(T).Name, id);
        }

        public static string NextKey<T>()
        {
            return string.Format(NEXT_KEY_NUM, typeof(T).Name);
        }

        public static string ListAllKeys<T>()
        {
            return string.Format(LIST_ALL, typeof(T).Name);
        }

        public static string ListAllNumKeys<T>()
        {
            return string.Format(LIST_ALL_NUM, typeof(T).Name);
        }

        public static string QueryKeyWithPropertyAndValue<T>(string property, string value)
        {
            return string.Format(QUERY_WITH_PROPERTY_AND_VALUE_KEY_PATTERN, typeof(T).Name, property, value);
        }

        public static string QueryKeyWithProperty<T>(string property)
        {
            return string.Format(QUERY_WITH_PROPERTY_KEY_PATTERN, typeof(T).Name, property);
        }

        public static string SubModelKey<TModel>(string id)
        {
            return string.Format(SUBMODEL_KEY_PATTERN, typeof(TModel).Name, id);
        }

    }
}
