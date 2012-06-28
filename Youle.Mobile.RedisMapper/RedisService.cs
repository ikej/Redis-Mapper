using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;
using System.Reflection;
using Youle.Mobile.Core;
using Newtonsoft.Json;
using Youle.Mobile.Core.Helpers;
using System.Linq.Expressions;
using System.Collections;
using System.Net.Sockets;
using System.Threading;

namespace Youle.Mobile.RedisMapper
{
    public class RedisService : IRedisService, IDisposable
    {

        #region Redis Client instance
        private static IRedisClientsManager _redisClientManager;
        public static IRedisClientsManager RedisClientManager
        {
            get
            {
                if (_redisClientManager == null)
                {
                    var redisConfig = new RedisClientManagerConfig
                    {
                        MaxWritePoolSize = AppConfigKeys.MAX_WRITE_POOL_SIZE.ConfigValue().ToInt32(),
                        MaxReadPoolSize = AppConfigKeys.MAX_READ_POOL_SIZE.ConfigValue().ToInt32(),
                        AutoStart = true
                    };


                    string[] readWriteHosts = AppConfigKeys.REDIS_READ_WRITE_SERVERS.ConfigValue().Split(';');
                    string[] readOnlyHosts = AppConfigKeys.REDIS_READONLY_SERVERS.ConfigValue().Split(';');

                    _redisClientManager = new PooledRedisClientManager(readWriteHosts, readOnlyHosts, redisConfig);
                }

                return _redisClientManager;
            }
        }
        #endregion

        private string _currentMode = "AppStore";
        public string CurrentMode
        {
            get
            {
                return _currentMode;
            }
            set
            {
                _currentMode = value;
            }
        }

        #region CRUD operations
        public virtual string Add<T>(T model) where T : IRedisModel
        {
            using (var Redis = RedisClientManager.GetClient())
            {
                if (model == null) return null;
                if (string.IsNullOrWhiteSpace(model.Id))
                {
                    model.Id = NextId<T>().ToString();
                }
                if (Get<T>(model.Id) != null) return null;

                string modelKey = GetKey<T>(model);

                model.ModuleName = CurrentMode;

                Redis.Set<T>(modelKey, model);

                Redis.AddItemToSortedSet(RedisKeyFactory.ListAllKeys<T>(), modelKey, model.CreateDateTime.Ticks);

                Redis.IncrementValue(RedisKeyFactory.ListAllNumKeys<T>());

                Redis.IncrementValue(RedisKeyFactory.NextKey<T>());

                BuildIndex<T>(model);

                return model.Id;
            }
        }

        /// <summary>
        /// since we need remove all index key for originalApp, we need pass in app instance before update
        /// </summary>
        /// <param name="originalApp">app instance before update</param>
        /// <param name="updatedApp">app instance after update</param>
        public virtual void UpdateWithRebuildIndex<T>(T originalModel, T updatedModel) where T : IRedisModel
        {
            using (var Redis = RedisClientManager.GetClient())
            {
                if (originalModel == null) throw new ArgumentNullException("originalModel");
                if (updatedModel == null) throw new ArgumentNullException("updatedModel");

                if (!originalModel.Id.EqualsOrdinalIgnoreCase(updatedModel.Id)) throw new ArgumentException("The two model have different ids.");

                Delete<T>(originalModel, false);
                Add<T>(updatedModel);
            }
        }

        public virtual void Update<T>(T model) where T : IRedisModelBase
        {
            using (var Redis = RedisClientManager.GetClient())
            {
                string modelKey = GetKey<T>(model);
                Redis.Set<T>(modelKey, model);
            }
        }

        public virtual void Delete<T>(T model, bool IsRemoveSubModel = true) where T : IRedisModelBase
        {
            using (var Redis = RedisClientManager.GetClient())
            {
                if (model != null)
                {
                    string modelKey = GetKey<T>(model);

                    Redis.Remove(modelKey);

                    Redis.RemoveItemFromSortedSet(RedisKeyFactory.ListAllKeys<T>(), modelKey);

                    Redis.IncrementValueBy(RedisKeyFactory.ListAllNumKeys<T>(), -1);

                    if (GetAllCount<T>() == 0)
                    {
                        Redis.Remove(RedisKeyFactory.ListAllNumKeys<T>());
                    }

                    BuildIndex<T>(model, true);

                    if (IsRemoveSubModel)
                        Redis.Remove(RedisKeyFactory.SubModelKey<T>(model.Id));
                }
            }
        }

        public void SetActive<T>(bool isActive, string id) where T : IRedisModel
        {
            using (var Redis = RedisClientManager.GetClient())
            {
                if (isActive)
                {
                    var model = Get<T>(id);
                    if (model != null)
                    {
                        Redis.AddItemToSortedSet(RedisKeyFactory.ListAllKeys<T>(), GetKey<T>(id), model.CreateDateTime.Ticks);
                    }
                }
                else
                {
                    Redis.RemoveItemFromSortedSet(RedisKeyFactory.ListAllKeys<T>(), GetKey<T>(id));
                }
            }
        }

        public bool IsActive<T>(string id) where T : IRedisModelBase
        {
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {
                return Redis.SortedSetContainsItem(RedisKeyFactory.ListAllKeys<T>(), GetKey<T>(id));
            }
        }

        public bool IsExist<T>(string id)
            where T : IRedisModelBase
        {
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {
                return Redis.ContainsKey(GetKey<T>(id));
            }
        }


        public T Get<T>(string id) where T : IRedisModelBase
        {
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {
                return Redis.Get<T>(GetKey<T>(id));
            }
        }

        public int GetAllCount<T>()
            where T : IRedisModelBase
        {
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {
                return Redis.Get<int>(RedisKeyFactory.ListAllNumKeys<T>());
            }
        }

        public int GetAllCountByMode<T>(List<string> allowedModes)
            where T : IRedisModel
        {
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {
                return GetAllActiveModelIds<T>().IdsToValues<T>().FilterByUIMode<T>(allowedModes).Count;
            }
        }

        public int NextId<T>()
             where T : IRedisModelBase
        {
            using (var Redis = RedisClientManager.GetClient())
            {
                int id = Redis.Get<int>(RedisKeyFactory.NextKey<T>()) + 1;

                while (IsExist<T>(id.ToString()))
                {
                    id++;
                    Redis.IncrementValue(RedisKeyFactory.NextKey<T>());
                }

                return id;
            }
        }

        public List<string> GetAllActiveModelIds<T>() where T : IRedisModelBase
        {
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {
                return Redis.GetRangeFromSortedSetDesc(RedisKeyFactory.ListAllKeys<T>(), 0, -1);
            }
        }

        public List<string> GetPagedModelIds<T>(int pageNum, int pageSize, string propertyName = "", bool isAsc = false) where T : IRedisModelBase
        {
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {
                int start = (pageNum - 1) * pageSize;
                int end = pageNum * pageSize - 1;

                if (pageNum == 0) // get all
                {
                    start = 0;
                    end = -1;
                }

                if (string.IsNullOrWhiteSpace(propertyName))
                {
                    if (isAsc)
                    {
                        return Redis.GetRangeFromSortedSet(RedisKeyFactory.ListAllKeys<T>(), start, end);
                    }
                    else
                    {
                        return Redis.GetRangeFromSortedSetDesc(RedisKeyFactory.ListAllKeys<T>(), start, end);
                    }
                }
                else
                {
                    string queryKey = RedisKeyFactory.QueryKeyWithProperty<T>(propertyName);
                    if (isAsc)
                    {
                        return Redis.GetRangeFromSortedSet(queryKey, start, end);
                    }
                    else
                    {
                        return Redis.GetRangeFromSortedSetDesc(queryKey, start, end);
                    }
                }
            }
        }

        public List<T> GetValuesByIds<T>(List<string> ids, bool needKeyFormat = false)
        {
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {
                List<T> results = new List<T>();
                if (needKeyFormat)
                {
                    for (int i = 0; i < ids.Count; i++)
                    {
                        ids[i] = RedisKeyFactory.ModelKey<T>(ids[i]);
                    }
                    results = Redis.GetValues<T>(ids);
                }
                else
                {
                    results = Redis.GetValues<T>(ids);
                }

                return results == null ? new List<T>() : results;
            }
        }
        #endregion

        #region SUB Model (one to many relation)
        public virtual bool SetSubModel<TModel, TSubModel>(string modelId, TSubModel subModel)
            where TModel : IRedisModelBase
            where TSubModel : IRedisModelBase
        {
            using (var Redis = RedisClientManager.GetClient())
            {
                return Redis.SetEntryInHash(RedisKeyFactory.SubModelKey<TModel>(modelId), GetKey<TSubModel>(subModel), JsonConvert.SerializeObject(subModel));
            }
        }

        public bool SetEntryInHash<T>(string modelId, string subModelId, T subModel)
        {
            using (var Redis = RedisClientManager.GetClient())
            {
                return Redis.SetEntryInHash(modelId, subModelId, JsonConvert.SerializeObject(subModel));
            }
        }

        public bool DeleteEntryFromHash(string modelId, string subModelId)
        {
            using (var Redis = RedisClientManager.GetClient())
            {
                return Redis.RemoveEntryFromHash(modelId, subModelId);
            }
        }

        public void SetRangeInHash<T>(string modelId, List<KeyValuePair<string, T>> keyValuePairs)
        {
            List<KeyValuePair<string, string>> keyValues = new List<KeyValuePair<string, string>>();

            if (keyValuePairs == null)
                return;

            foreach (var kv in keyValuePairs)
            {
                keyValues.Add(new KeyValuePair<string, string>(kv.Key, JsonConvert.SerializeObject(kv.Value)));
            }

            using (var Redis = RedisClientManager.GetClient())
            {
                Redis.SetRangeInHash(modelId, keyValues as IEnumerable<KeyValuePair<string, string>>);
            }
        }

        public bool IsExist(string key)
        {
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {
                return Redis.ContainsKey(key);
            }
        }

        public bool IsExistInHash(string modelId, string subModelId)
        {
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {
                return Redis.HashContainsEntry(modelId, subModelId);
            }
        }

        public T GetValueFromHash<T>(string modelId, string subModelId)
        {
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {
                var subModelJSONString = Redis.GetValueFromHash(modelId, subModelId);
                if (string.IsNullOrWhiteSpace(subModelJSONString))
                {
                    return default(T);
                }
                else
                {
                    return JsonConvert.DeserializeObject<T>(subModelJSONString);
                }
            }
        }

        public List<T> GetAllValuesFromHash<T>(string modelId)
        {
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {

                var all = Redis.GetHashValues(modelId);
                if (all != null && all.Count > 0)
                {
                    List<T> returnValues = new List<T>();
                    foreach (var item in all)
                    {
                        returnValues.Add(JsonConvert.DeserializeObject<T>(item));
                    }
                    return returnValues;
                }
                else
                {
                    return default(List<T>);
                }
            }
        }

        public List<string> GetAllKeysFromHash(string modelId)
        {
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {
                return Redis.GetHashKeys(modelId);
            }
        }


        public TSubModel GetSubModel<TModel, TSubModel>(string modelId, string subModelId, bool isFullSubModelKey = false)
            where TModel : IRedisModelBase
            where TSubModel : IRedisModelBase
        {
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {
                var subModelJSONString = Redis.GetValueFromHash(RedisKeyFactory.SubModelKey<TModel>(modelId), isFullSubModelKey ? subModelId : GetKey<TSubModel>(subModelId));
                if (string.IsNullOrWhiteSpace(subModelJSONString))
                {
                    return default(TSubModel);
                }
                else
                {
                    return JsonConvert.DeserializeObject<TSubModel>(subModelJSONString);
                }
            }
        }

        public List<string> GetAllSubModelIds<TModel>(string modelId)
            where TModel : IRedisModelBase
        {
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {
                return Redis.GetHashKeys(RedisKeyFactory.SubModelKey<TModel>(modelId));
            }
        }

        public List<string> GetAllSubModelIdsByType<TModel, TSubModel>(string modelId)
            where TModel : IRedisModelBase
            where TSubModel : IRedisModelBase
        {
            return GetAllSubModelIds<TModel>(modelId).FilterByType<TSubModel>();
        }

        public List<TSubModel> GetAllSubModelsByType<TModel, TSubModel>(string modelId)
            where TModel : IRedisModelBase
            where TSubModel : IRedisModelBase
        {

            List<TSubModel> subModels = new List<TSubModel>();
            var subModelIds = GetAllSubModelIdsByType<TModel, TSubModel>(modelId).ToArray();
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {
                List<string> values = Redis.GetValuesFromHash(RedisKeyFactory.SubModelKey<TModel>(modelId), subModelIds);
                foreach (var v in values)
                {
                    if (!string.IsNullOrWhiteSpace(v))
                    {
                        subModels.Add(JsonConvert.DeserializeObject<TSubModel>(v));
                    }
                }

                return subModels;
            }
        }

        public virtual bool DeleteSubModel<TModel, TSubModel>(string modelId, string subModelId)
            where TModel : IRedisModelBase
            where TSubModel : IRedisModelBase
        {
            using (var Redis = RedisClientManager.GetClient())
            {
                return Redis.RemoveEntryFromHash(RedisKeyFactory.SubModelKey<TModel>(modelId), GetKey<TSubModel>(subModelId));
            }
        }

        public bool ExistSubModel<TModel, TSubModel>(string modelId, string subModelId)
            where TModel : IRedisModelBase
            where TSubModel : IRedisModelBase
        {
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {
                return Redis.HashContainsEntry(RedisKeyFactory.SubModelKey<TModel>(modelId), GetKey<TSubModel>(subModelId));
            }
        }
        #endregion

        #region Custom Properties

        public T GetModelWithCustomProperties<T, TCustomProperty>(string modelId)
            where T : IRedisModelBase, IRedisModelWithSubModel
            where TCustomProperty : IRedisModelBase, IRedisCustomProperty
        {
            var model = Get<T>(modelId);
            if (model == null)
            {
                return default(T);
            }
            model.CustomProperties = new Dictionary<string, object>();

            var allCustomProperties = GetAllSubModelIdsByType<T, TCustomProperty>(modelId);
            foreach (var p in allCustomProperties)
            {
                var customProperty = GetSubModel<T, TCustomProperty>(modelId, p, true);
                model.CustomProperties[customProperty.Id] = customProperty.Value;
            }

            return model;
        }

        public TCustomProperty GetCustomPropertyFrom<T, TCustomProperty>(string modelId, string customPropertyId)
            where T : IRedisModelBase
            where TCustomProperty : IRedisModelBase, IRedisCustomProperty
        {
            return GetSubModel<T, TCustomProperty>(modelId, customPropertyId);
        }

        public void AddCustomPropertyFor<T, TCustomProperty>(string modelId, TCustomProperty customProperty)
            where T : IRedisModelBase
            where TCustomProperty : IRedisModelBase, IRedisCustomProperty
        {
            SetSubModel<T, TCustomProperty>(modelId, customProperty);

            // BUILD INDEX
            BuildIndexForDynamicElements<T, TCustomProperty>(modelId, customProperty);
        }

        public void UpdateCustomPropertyFor<T, TCustomProperty>(string modelId, TCustomProperty originalTCustomProperty, TCustomProperty updatedTCustomProperty)
            where T : IRedisModelBase
            where TCustomProperty : IRedisModelBase, IRedisCustomProperty
        {
            DeleteCustomPropertyFor<T, TCustomProperty>(modelId, originalTCustomProperty);
            AddCustomPropertyFor<T, TCustomProperty>(modelId, updatedTCustomProperty);
        }

        public void DeleteCustomPropertyFor<T, TCustomProperty>(string modelId, TCustomProperty customProperty)
            where T : IRedisModelBase
            where TCustomProperty : IRedisModelBase, IRedisCustomProperty
        {
            if (customProperty == null) return;

            DeleteSubModel<T, TCustomProperty>(modelId, customProperty.Id);

            // DELETE INDEX
            BuildIndexForDynamicElements<T, TCustomProperty>(modelId, customProperty, true);
        }

        public void DeleteWithCustomProperties<T, TCustomProperty>(string modelId)
            where T : IRedisModelBase
            where TCustomProperty : IRedisModelBase, IRedisCustomProperty
        {
            var model = Get<T>(modelId);

            if (model != null)
            {
                var allCustomProperties = GetAllSubModelsByType<T, TCustomProperty>(modelId);
                foreach (var p in allCustomProperties)
                {
                    DeleteCustomPropertyFor<T, TCustomProperty>(modelId, p);
                }
                Delete<T>(model);
            }
        }
        #endregion

        #region Query Interface

        public List<string> FindIdsByConditions<T>(Dictionary<string, string> conditions)
            where T : IRedisModelBase
        {

            List<string> conditionSets = new List<string>();
            foreach (var key in conditions.Keys)
            {
                conditionSets.Add(RedisKeyFactory.QueryKeyWithPropertyAndValue<T>(key, conditions[key]));
            }
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {
                return Redis.GetIntersectFromSets(conditionSets.ToArray()).ToList();
            }
        }

        public List<string> FindIdsByConditions<T>(List<KeyValuePair<string, string>> conditions)
            where T : IRedisModelBase
        {
            List<string> conditionSets = new List<string>();
            foreach (var c in conditions)
            {
                conditionSets.Add(RedisKeyFactory.QueryKeyWithPropertyAndValue<T>(c.Key, c.Value));
            }
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {
                return Redis.GetIntersectFromSets(conditionSets.ToArray()).ToList();
            }
        }

        public List<string> FindIdsByValueRange<T>(string propertyName, double? start, double? end)
            where T : IRedisModelBase
        {

            if (!start.HasValue)
            {
                start = double.MinValue;
            }

            if (!end.HasValue)
            {
                end = double.MaxValue;
            }
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {
                IDictionary<string, double> appIds =
                    Redis.GetRangeWithScoresFromSortedSetByLowestScore(RedisKeyFactory.QueryKeyWithProperty<T>(propertyName), start.GetValueOrDefault(), end.GetValueOrDefault());

                return appIds.Keys.ToList();
            }

        }

        public List<string> FindIdsByValueRange<T>(string propertyName, DateTime? start, DateTime? end)
            where T : IRedisModelBase
        {

            if (!start.HasValue)
            {
                start = DateTime.MinValue;
            }

            if (!end.HasValue)
            {
                end = DateTime.MaxValue;
            }
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {
                IDictionary<string, double> appIds =
                    Redis.GetRangeWithScoresFromSortedSetByLowestScore(RedisKeyFactory.QueryKeyWithProperty<T>(propertyName), start.GetValueOrDefault().Ticks, end.GetValueOrDefault().Ticks);

                return appIds.Keys.ToList();
            }
        }

        public List<string> FuzzyFindIdsByCondition<T>(string property, string valuePattern)
            where T : IRedisModelBase
        {
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {
                return Redis.GetUnionFromSets(KeyFuzzyFind(RedisKeyFactory.QueryKeyWithPropertyAndValue<T>(property, valuePattern)).ToArray()).ToList();
            }
        }

        public List<string> KeyFuzzyFind(string generalKeyPattern)
        {
            using (var Redis = RedisClientManager.GetReadOnlyClient())
            {
                return Redis.SearchKeys(generalKeyPattern);
            }
        }
        #endregion

        #region Index
        public void DoIndexBySet<T>(string idVal, string propertyName, string value, bool isRemoveIndex = false)
            where T : IRedisModelBase
        {
            string queryKey = RedisKeyFactory.QueryKeyWithPropertyAndValue<T>(propertyName, value);
            using (var Redis = RedisClientManager.GetClient())
            {
                if (isRemoveIndex)
                {
                    Redis.RemoveItemFromSet(queryKey, idVal);
                }
                else
                {
                    Redis.AddItemToSet(queryKey, idVal);
                }
            }
        }

        public void DoIndexBySortedSet<T>(string idVal, string propertyName, object value, bool isRemoveIndex = false)
            where T : IRedisModelBase
        {
            string queryKey = RedisKeyFactory.QueryKeyWithProperty<T>(propertyName);
            using (var Redis = RedisClientManager.GetClient())
            {
                if (isRemoveIndex)
                {
                    Redis.RemoveItemFromSortedSet(queryKey, idVal);
                }
                else
                {
                    if (value.GetType().Equals(typeof(DateTime)))
                    {
                        Redis.AddItemToSortedSet(queryKey, idVal, ((DateTime)value).Ticks);
                    }
                    else
                    {
                        Redis.AddItemToSortedSet(queryKey, idVal, Convert.ToDouble(value));
                    }
                }
            }
        }

        public void BuildIndex<T>(T model, bool isRemoveIndex = false)
            where T : IRedisModelBase
        {
            string entityIdValue = GetKey<T>(model);

            foreach (PropertyInfo prop in model.GetType().GetProperties())
            {
                var value = prop.GetValue(model, null);
                if (value == null) continue;

                foreach (object attribute in prop.GetCustomAttributes(true))
                {
                    if (attribute is QueryOrSortFieldAttribute)
                    {
                        if (prop.PropertyType.IsEnumerableType())
                        {
                            var list = value as IEnumerable;
                            if (list == null) continue;

                            foreach (var v in list)
                            {
                                DoIndexBySet<T>(entityIdValue, prop.Name, v.ToString(), isRemoveIndex);
                            }
                        }
                        else if (value.GetType().IsValueType && !value.GetType().Equals(typeof(bool)))
                        {
                            DoIndexBySortedSet<T>(entityIdValue, prop.Name, value, isRemoveIndex);
                        }
                        else
                        {
                            DoIndexBySet<T>(entityIdValue, prop.Name, value.ToString(), isRemoveIndex);
                        }

                        break;
                    }
                }

            }
        }
        #endregion

        #region Helpers
        private bool TryPing(string strIpAddressWithPort, int nTimeoutMsec)
        {
            var tempArray = strIpAddressWithPort.Split(':');
            if (tempArray == null || tempArray.Length < 2)
            {
                return false;
            }

            var strIpAddress = tempArray[0];
            var intPort = tempArray[1].ToInt32();
            Socket connSocket = null;
            try
            {
                connSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                connSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, false);


                IAsyncResult result = connSocket.BeginConnect(strIpAddress, intPort, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(nTimeoutMsec, true);

                return connSocket.Connected;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (null != connSocket)
                    connSocket.Close();
            }
        }

        private void BuildIndexForDynamicElements<T, TCustomProperty>(string modelId, TCustomProperty customProperty, bool isRemoveIndex = false)
            where T : IRedisModelBase
            where TCustomProperty : IRedisModelBase, IRedisCustomProperty
        {
            if (!customProperty.IsQueriable)
            {
                return;
            }

            if (customProperty.Value != null)
            {
                if (customProperty.Value.GetType().IsEnumerableType())
                {
                    foreach (var v in customProperty.Value as IEnumerable)
                    {
                        DoIndexBySet<T>(RedisKeyFactory.ModelKey<T>(modelId), customProperty.Id, v.ToString(), isRemoveIndex);
                    }
                }
                else if (customProperty.Value.GetType().IsValueType && !customProperty.Value.GetType().Equals(typeof(bool)))
                {
                    DoIndexBySortedSet<T>(RedisKeyFactory.ModelKey<T>(modelId), customProperty.Id, customProperty.Value, isRemoveIndex);
                }
                else
                {
                    DoIndexBySet<T>(RedisKeyFactory.ModelKey<T>(modelId), customProperty.Id, customProperty.Value.ToString(), isRemoveIndex);
                }
            }

        }

        protected string GetKey<T>(T model) where T : IRedisModelBase
        {
            return RedisKeyFactory.ModelKey<T>(model.Id);
        }

        private string GetKey<T>(string id)
        {
            return RedisKeyFactory.ModelKey<T>(id);
        }


        #endregion

        #region Queue
        public void AddItemToQueue<T>(string queueId, T queueItem)
        {
            using (var Redis = RedisClientManager.GetClient())
            {
                Redis.AddItemToList(RedisKeyFactory.QueueKey<T>(queueId), JsonConvert.SerializeObject(queueItem));
            }
        }

        public T RetrieveItemFromQueue<T>(string queueId)
        {
            using (var Redis = RedisClientManager.GetClient())
            {
                var result = Redis.BlockingDequeueItemFromList(RedisKeyFactory.QueueKey<T>(queueId), new TimeSpan(0));

                if (result != null)
                {
                    return JsonConvert.DeserializeObject<T>(result);
                }

                return default(T);
            }
        }
        #endregion

        #region Misc
        public bool IsAvailable(int connectionTimeoutMillesecs = 200)
        {
            var redisServers = AppConfigKeys.REDIS_READ_WRITE_SERVERS.ConfigValue().Split(';');
            if (redisServers.Length > 0)
            {
                if (TryPing(redisServers[0], connectionTimeoutMillesecs))
                {
                    using (var Redis = RedisClientManager.GetReadOnlyClient())
                    {
                        if (((RedisClient)Redis).Ping())
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        #endregion

        public void FlushAll()
        {
            using (var Redis = RedisClientManager.GetClient())
            {
                Redis.FlushAll();
            }
        }

        #region Dispose
        ~RedisService()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                using (var Redis = RedisClientManager.GetClient())
                {
                    Redis.Dispose();
                }
            }
        }
        #endregion
    }
}
