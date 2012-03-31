using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace RedisMapper
{
    public interface IRedisService
    {
        string Add<T>(T model) where T : IRedisModel;
        void UpdateWithRebuildIndex<T>(T originalModel, T updatedModel) where T : IRedisModel;
        void Update<T>(T model) where T : IRedisModel;
        void Delete<T>(T model, bool isRemoveSubModel = true) where T : IRedisModel;
        void SetActive<T>(bool isActive, string id) where T : IRedisModel;
        bool IsActive<T>(string id) where T : IRedisModel;
        T Get<T>(string id) where T : IRedisModel;
        List<string> GetAllActiveModelIds<T>() where T : IRedisModel;
        List<string> GetPagedModelIds<T>(int pageNum, int pageSize, string propertyName = "", bool isAsc = false) where T : IRedisModel;
        int GetAllCount<T>() where T : IRedisModel;
        bool IsExist<T>(string id) where T : IRedisModel;
        int NextId<T>() where T : IRedisModel;
        List<T> GetValuesByIds<T>(List<string> ids, bool needKeyFormat = false);

        bool SetSubModel<TModel, TSubModel>(string modelId, TSubModel subModel)
            where TModel : IRedisModel
            where TSubModel : IRedisModel;
        TSubModel GetSubModel<TModel, TSubModel>(string modelId, string subModelId, bool isFullSubModelKey = false)
            where TModel : IRedisModel
            where TSubModel : IRedisModel;
        bool DeleteSubModel<TModel, TSubModel>(string modelId, string subModelId)
            where TModel : IRedisModel
            where TSubModel : IRedisModel;
        List<string> GetAllSubModelIds<TModel>(string modelId)
            where TModel : IRedisModel;
        bool ExistSubModel<TModel, TSubModel>(string modelId, string subModelId)
            where TModel : IRedisModel
            where TSubModel : IRedisModel;
        List<TSubModel> GetAllSubModelsByType<TModel, TSubModel>(string modelId)
            where TModel : IRedisModel
            where TSubModel : IRedisModel;


        T GetModelWithCustomProperties<T, TCustomProperty>(string modelId)
            where T : IRedisModel, IRedisModelWithSubModel
            where TCustomProperty : IRedisModel, IRedisCustomProperty;
        void DeleteWithCustomProperties<T, TCustomProperty>(string modelId)
            where T : IRedisModel
            where TCustomProperty : IRedisModel, IRedisCustomProperty;
        TCustomProperty GetCustomPropertyFrom<T, TCustomProperty>(string modelId, string customPropertyId)
            where T : IRedisModel
            where TCustomProperty : IRedisModel, IRedisCustomProperty;
        void AddCustomPropertyFor<T, TCustomProperty>(string modelId, TCustomProperty customProperty)
            where T : IRedisModel
            where TCustomProperty : IRedisModel, IRedisCustomProperty;
        void UpdateCustomPropertyFor<T, TCustomProperty>(string modelId, TCustomProperty originalTCustomProperty, TCustomProperty updatedTCustomProperty)
            where T : IRedisModel
            where TCustomProperty : IRedisModel, IRedisCustomProperty;
        void DeleteCustomPropertyFor<T, TCustomProperty>(string modelId, TCustomProperty customProperty)
            where T : IRedisModel
            where TCustomProperty : IRedisModel, IRedisCustomProperty;


        void DoIndexBySet<T>(string idVal, string propertyName, string value, bool isRemoveIndex = false) where T : IRedisModel;
        void DoIndexBySortedSet<T>(string idVal, string propertyName, object value, bool isRemoveIndex = false) where T : IRedisModel;
        void BuildIndex<T>(T model, bool isRemoveIndex = false) where T : IRedisModel;

        List<string> Find<T>(Expression<Func<T, bool>> expression) where T : IRedisModel;
        List<string> FindIdsByConditions<T>(Dictionary<string, string> conditions) where T : IRedisModel;
        List<string> FindIdsByValueRange<T>(string propertyName, double? start, double? end) where T : IRedisModel;
        List<string> FindIdsByValueRange<T>(string propertyName, DateTime? start, DateTime? end) where T : IRedisModel;
        List<string> FuzzyFindIdsByCondition<T>(string property, string valuePattern) where T : IRedisModel;
        List<string> KeyFuzzyFind(string generalKeyPattern);

        void AddItemToQueue<T>(string queueId, T queueItem);
        T RetrieveItemFromQueue<T>(string queueId);
    }
}
