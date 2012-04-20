using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RedisMapper;
using Newtonsoft.Json;
using Xunit;
using System.Collections.Specialized;
using Moq;
using RedisMapperTest.TestModel;

namespace RedisMapperTest
{
    public class RedisMapperTest
    {
        #region Service instance
        private static volatile IRedisService redisInstance;
        public static IRedisService Redis
        {
            get
            {
                if (redisInstance == null)
                {
                   redisInstance = new RedisService();
                }

                return redisInstance;
            }
        }
        #endregion


        [Fact]
        public void Same_model_to_save_twice_will_update_the_existed_values()
        {
            var app = new App();
            app.Id = "Same_model_to_save_twice_will_update_the_existed_values";
            app.Name = "SameAppToSaveTwice";
            app.Price = 111;

            using (var redis = new RedisService())
            {
                var exited = redis.Get<App>(app.Id);
                redis.Delete<App>(exited);

                redis.Add<App>(app);

                app.Name = "Name1";
                redis.Add<App>(app);

                var getApp = redis.Get<App>(app.Id);

                Assert.NotEmpty(getApp.Name);

                redis.Delete<App>(exited);

            }
        }

        [Fact]
        public void update_model_should_throw_exception_if_they_have_differentIds()
        {
            using (var redis = new RedisService())
            {
                var app = new App();
                app.Id = "update_model_should_throw_exception_if_they_have_differentIds";
                app.Name = "test_update";

                redis.Delete<App>(app);

                redis.Add<App>(app);

                //change the property
                var newApp = new App();
                newApp.Id = "new_update_model_should_throw_exception_if_they_have_differentIds";

                var ex = Assert.Throws<ArgumentException>(() => redis.UpdateWithRebuildIndex<App>(app, newApp));
                Assert.True(ex.Message.Contains("different"));

                redis.Delete<App>(app);

            }
        }

        //[Fact]
        //public void RedisMapperIntergrationTest()
        //{
        //    Redis.AddItemToQueue<string>("qlist1", "task");
        //    var waitResult = Redis.RetrieveItemFromQueue<string>("qlist1");
        //    Console.WriteLine(waitResult);

        //    #region App Add & Get
        //    App app = new App();
        //    app.Id = "10002";
        //    app.Name = "App2";
        //    app.Price = 10.55;

        //    app.CreateDateTime = new DateTime(2011, 11, 23, 1, 1, 1);

        //    App app1 = new App();
        //    app1.Id = "10001";
        //    app1.Name = "App1";
        //    app1.Price = 25.88;
        //    app1.CreateDateTime = new DateTime(2011, 12, 1, 1, 1, 1);

        //    Redis.Add<App>(app);
        //    PrintDebug("app id:10001 added");
        //    Redis.Add<App>(app1);
        //    PrintDebug("app id:10002 added");

        //    PrintDebug("Get app info for id:10001 : " + Redis.Get<App>("10001").Name);

        //    Console.WriteLine("Get one page of app : " + Redis.GetPagedModelIds<App>(1, 2).IdsToValues<App>().Count);
        //    Console.WriteLine("Get one page of app sorted by price: " + Redis.GetPagedModelIds<App>(1, 2, "Price", true).IdsToValues<App>().Count);
        //    #endregion

        //    #region App Element settings
        //    Element ele = new Element();
        //    ele.Id = "1";
        //    ele.Name = "分辨率";
        //    ele.Type = (int)ElementType.CheckBox;
        //    Redis.Add<Element>(ele);

        //    ElementDetail eleDetl = new ElementDetail();
        //    eleDetl.Id = "2";
        //    eleDetl.Value = "176x220";
        //    eleDetl.ElementId = "1";
        //    Redis.Add<ElementDetail>(eleDetl);

        //    ElementDetail eleDetl1 = new ElementDetail();
        //    eleDetl1.Id = "3";
        //    eleDetl1.Value = "240x320";
        //    eleDetl1.ElementId = "1";
        //    Redis.Add<ElementDetail>(eleDetl1);

        //    ElementDetail eleDetl2 = new ElementDetail();
        //    eleDetl2.Id = "4";
        //    eleDetl2.Value = "240x400";
        //    eleDetl2.ElementId = "1";
        //    Redis.Add<ElementDetail>(eleDetl2);


        //    var collection = Redis.GetAllActiveModelIds<Element>().IdsToValues<Element>();
        //    PrintDebug("Element count: " + collection.Count + "  " + Redis.GetAllCount<Element>());
        //    if (collection.Count > 0)
        //    {
        //        PrintDebug("Element name: " + collection[0].Name);
        //    }


        //    var collection2 = AppStoreSvc.GetElementDetailList("1");
        //    PrintDebug("ElementDetail count for Element[1]: " + collection2.Count);

        //    var originModel = CloneHelper.DeepClone<Element>(ele);
        //    ele.Name = "resolution";
        //    Redis.UpdateWithRebuildIndex<Element>(originModel, ele);
        //    PrintDebug("Element name: " + Redis.Get<Element>("1").Name);


        //    if (Redis.Get<Element>("1") == null)
        //    {
        //        PrintDebug("Element[1] deleted.");
        //    }
        //    #endregion

        //    #region App Dynamic Property
        //    #region Element Settings
        //    Element ele2 = new Element();
        //    ele2.Id = "2";
        //    ele2.Name = "CustomProperty";
        //    ele2.Type = (int)ElementType.CheckBox;
        //    ele2.IsQueriable = true;
        //    Redis.Add<Element>(ele2);

        //    Element ele5 = new Element();
        //    ele5.Id = "3";
        //    ele5.Name = "Resolution";
        //    ele5.Type = (int)ElementType.CheckBox;
        //    ele5.IsQueriable = true;
        //    Redis.Add<Element>(ele5);

        //    Element ele4 = new Element();
        //    ele4.Id = "4";
        //    ele4.Name = "IsTouchable";
        //    ele4.Type = (int)ElementType.TextField;
        //    ele4.IsQueriable = true;
        //    Redis.Add<Element>(ele4);

        //    ElementDetail elementDetail1 = new ElementDetail();
        //    elementDetail1.ElementId = "2";
        //    elementDetail1.Value = "CustomPropertyValue1";

        //    ElementDetail elementDetail2 = new ElementDetail();
        //    elementDetail2.ElementId = "2";
        //    elementDetail2.Value = "CustomPropertyValue2";

        //    Element ele3 = new Element();
        //    ele3.Id = "3";
        //    ele3.Name = "IntCustomProperty";
        //    ele3.Type = (int)ElementType.TextField;
        //    ele3.IsQueriable = true;
        //    Redis.Add<Element>(ele3);

        //    ElementDetail elementDetail3 = new ElementDetail();
        //    elementDetail3.ElementId = "3";
        //    elementDetail3.Value = 3.6;
        //    #endregion

        //    // Set up dynamic value for App 
        //    CustomProperty customProperty = new CustomProperty();
        //    customProperty.Id = ele2.Name;
        //    customProperty.Value = new List<string>() { elementDetail1.Value.ToString(), elementDetail2.Value.ToString() };
        //    customProperty.IsQueriable = ele2.IsQueriable;

        //    CustomProperty customProperty2 = new CustomProperty();
        //    customProperty2.Id = ele3.Name;
        //    customProperty2.Value = elementDetail3.Value;
        //    customProperty2.IsQueriable = ele3.IsQueriable;

        //    CustomProperty customProperty3 = new CustomProperty();
        //    customProperty3.Id = ele3.Name;
        //    customProperty3.Value = elementDetail3.Value;
        //    customProperty3.IsQueriable = ele3.IsQueriable;

        //    CustomProperty resolutionProperty = new CustomProperty();
        //    resolutionProperty.Id = ele5.Name;
        //    resolutionProperty.Value = new List<string>() { "240*320", "320*480" };

        //    CustomProperty isTouchableProperty = new CustomProperty();
        //    isTouchableProperty.Id = ele4.Name;
        //    isTouchableProperty.Value = true;

        //    Redis.AddCustomPropertyFor<App, CustomProperty>("10001", customProperty);
        //    Redis.AddCustomPropertyFor<App, CustomProperty>("10001", customProperty2);
        //    Redis.AddCustomPropertyFor<App, CustomProperty>("10001", resolutionProperty);
        //    Redis.AddCustomPropertyFor<App, CustomProperty>("10001", isTouchableProperty);
        //    resolutionProperty.Value = new List<string>() { "320*480" };
        //    Redis.AddCustomPropertyFor<App, CustomProperty>("10002", resolutionProperty);
        //    isTouchableProperty.Value = false;
        //    Redis.AddCustomPropertyFor<App, CustomProperty>("10002", isTouchableProperty);
        //    PrintDebug("add App's custom property: " + Redis.GetCustomPropertyFrom<App, CustomProperty>("10001", "CustomProperty").Id);

        //    Dictionary<string, string> newConditions = new Dictionary<string, string>();
        //    newConditions.Add("Resolution", "320*480");
        //    newConditions.Add("IsTouchable", bool.TrueString);
        //    newConditions.Add("CustomProperty", "CustomPropertyValue2");
        //    PrintDebug("found app by dynamic property : " + Redis.FindIdsByConditions<App>(newConditions).Count);
        //    PrintDebug("found app by dynamic property (double value) : " + Redis.FindIdsByValueRange<App>("IntCustomProperty", 3.58, 3.62).Count);
        //    PrintDebug("find Custom Properties count for app[10001] :  " + Redis.GetModelWithCustomProperties<App, CustomProperty>("10001").CustomProperties.Count);

        //    var originalCustomProperty = CloneHelper.DeepClone<CustomProperty>(customProperty);
        //    customProperty.IsQueriable = false;
        //    Redis.UpdateCustomPropertyFor<App, CustomProperty>("10001", originalCustomProperty, customProperty);
        //    PrintDebug("found app by dynamic property [CustomProperty:CustomPropertyValue2] after set IsQueriable to false : " + Redis.FindIdsByConditions<App>(newConditions).Count);

        //    //AppStoreSvc.DeleteCustomPropertyForApp("10001", customProperty);
        //    //PrintDebug("delete App's CustomProperty: " + (AppStoreSvc.GetCustomPropertyForApp("10001", "CustomProperty") == null));
        //    #endregion

        //    #region Find by lambda expression
        //    var d = "App2";
        //    var result = Redis.Find<App>(a => a.Name == d && a.Price > 2.2);
        //    PrintDebug("Find App by Name == \"App2\" && Price > 2.2 : Count = " + result.Count);
        //    #endregion

        //    #region Find app By a certain Value Range
        //    List<App> apps = Redis.FindIdsByValueRange<App>("Price", 10.56, 25.88).IdsToValues<App>();
        //    PrintDebug("found app num by 10.56<= Price <=25.88 : " + apps.Count);

        //    apps = Redis.FindIdsByValueRange<App>("CreateDateTime", new DateTime(2011, 11, 23, 1, 1, 1), new DateTime(2011, 12, 1, 1, 1, 1)).IdsToValues<App>();
        //    PrintDebug("found app num by CreateDate : " + apps.Count);
        //    #endregion

        //    #region Fuzzy Find App
        //    apps = Redis.FuzzyFindIdsByCondition<App>("N*e", "App?").IdsToValues<App>();
        //    PrintDebug("found app num by N*e like App? " + apps.Count);
        //    #endregion

        //    #region Complex Find App

        //    List<string> appIdsByValueRange = Redis.FindIdsByValueRange<App>("Price", 10.55, 25.88);
        //    List<string> appIdsByDateValueRange = Redis.FindIdsByValueRange<App>("CreateDateTime", new DateTime(2011, 11, 23, 1, 1, 1), new DateTime(2011, 12, 1, 1, 1, 1));

        //    // Intersect means AND,  Union mean OR
        //    List<App> finalResult = appIdsByValueRange.Intersect<string>(appIdsByDateValueRange).ToList().IdsToValues<App>();

        //    PrintDebug("found app num by complex conditions " + finalResult.Count);
        //    #endregion

        //    #region App Version
        //    AppVersion appVer = new AppVersion();
        //    appVer.Id = "10000";
        //    appVer.FileUrl = "C:\a";
        //    appVer.PublishDateTime = DateTime.Now;
        //    AppStoreSvc.AddAppVersion("10001", appVer);
        //    appVer.FileUrl = "E:\\";
        //    AppStoreSvc.AddAppVersion("10001", appVer);
        //    PrintDebug("added version num : " + AppStoreSvc.GetCurrentVersionForApp("10001").Id);
        //    PrintDebug("app current version : " + Redis.Get<App>("10001").CurrentVer);

        //    var appCopy = CloneHelper.DeepClone<App>(app);
        //    app.Name = "App3";
        //    Redis.UpdateWithRebuildIndex<App>(appCopy, app);
        //    PrintDebug("App name updated to " + Redis.Get<App>("10002").Name);
        //    #endregion

        //    #region Category
        //    Category c1 = new Category();
        //    c1.Id = "1";
        //    c1.Name = "游戏";
        //    c1.ParentId = "0";
        //    c1.status = "1";

        //    Redis.Add<Category>(c1);

        //    Category csub1 = new Category();
        //    csub1.Id = "2";
        //    csub1.Name = "益智游戏";
        //    csub1.ParentId = "1";
        //    csub1.status = "1";

        //    Redis.Add<Category>(csub1);

        //    PrintDebug("Sub Category count for Category[0] : " + AppStoreSvc.GetCategoryList("0").Count);
        //    #endregion

        //    #region App list
        //    AppList applist1 = new AppList();
        //    applist1.Name = "10010";
        //    applist1.Id = "10010";
        //    applist1.CreateDateTime = DateTime.Now.AddSeconds(-3);
        //    applist1.CurrentVersion = 10010;

        //    AppList applist2 = new AppList();
        //    applist2.Name = "10020";
        //    applist2.Id = "10020";
        //    applist2.CreateDateTime = DateTime.Now.AddDays(3);
        //    applist2.CurrentVersion = 10011;

        //    Redis.Add<AppList>(applist1);
        //    Redis.Add<AppList>(applist2);
        //    PrintDebug("two app lists added.");

        //    customProperty.IsQueriable = true;
        //    Redis.AddCustomPropertyFor<AppList, CustomProperty>(applist1.Id, customProperty);
        //    Dictionary<string, string> applistConditions = new Dictionary<string, string>();
        //    applistConditions.Add("CustomProperty", "CustomPropertyValue2");
        //    PrintDebug("found applist by dynamic property : " + Redis.FindIdsByConditions<AppList>(applistConditions).Count);

        //    var applist1Origin = CloneHelper.DeepClone<AppList>(applist1);
        //    applist1.CreateDateTime = DateTime.Now.AddDays(1);
        //    applist1.CurrentVersion++; // make sure bump version number when upate applist
        //    Redis.UpdateWithRebuildIndex<AppList>(applist1Origin, applist1);
        //    PrintDebug("app list updated :" + Redis.Get<AppList>("10010").Name);

        //    AppSettingsForAppList customApp1 = new AppSettingsForAppList();
        //    customApp1.Id = app1.Id;
        //    customApp1.ScoreForSort = 2;
        //    customApp1.CustomProperties = new Dictionary<string, object>();
        //    customApp1.CustomProperties.Add("Name", "NewApp1");
        //    customApp1.CustomProperties.Add("Price", 2.3);
        //    customApp1.CustomProperties.Add("CustomProperty", "newValueFromAppListSettings");
        //    AppSettingsForAppList customApp2 = new AppSettingsForAppList();
        //    customApp2.Id = app.Id;
        //    customApp2.ScoreForSort = 1;
        //    customApp2.CustomProperties = new Dictionary<string, object>();
        //    customApp2.CustomProperties.Add("Name", "NewApp2");
        //    customApp2.CustomProperties.Add("Price", 3.2);

        //    AppStoreSvc.SetAppForAppList<AppList>("10010", customApp1);
        //    AppStoreSvc.SetAppForAppList<AppList>("10010", customApp2);

        //    var appsFromList1 = AppStoreSvc.GetAppsFromAppList<AppList>("10010");
        //    PrintDebug("App count for AppList[10010] : " + appsFromList1.Count);

        //    var app1FromAppList1 = AppStoreSvc.GetAppFromAppList<AppList>("10010", "10001");
        //    PrintDebug("Get App[10001]'s price From AppList[10010] : " + app1FromAppList1.Price);
        //    PrintDebug("Get App[10001]'s CustomProperty From AppList[10010] : " + app1FromAppList1.CustomProperties["CustomProperty"]);

        //    NameValueCollection headers = new NameValueCollection();
        //    headers.Add("Resolution", "240*320");
        //    headers.Add("IsTouchable", "1");
        //    RequestHeadersMock.Setup(s => s.Header).Returns(headers);
        //    MobileParam requestParams = new MobileParam(RequestHeadersMock.Object);
        //    var matchedApp = AppStoreSvc.GetMatchedAppByRequest<AppList>("10010", requestParams);
        //    //PrintDebug("Matched app in applist[10010]: " + matchedApp.Id);

        //    AppSettingsForAppList customApp3 = new AppSettingsForAppList();
        //    customApp3.Id = "10001";
        //    customApp3.CustomProperties = new Dictionary<string, object>();
        //    AppStoreSvc.SetAppForAppList<AppList>("10020", customApp3);

        //    List<string> appIdsUnion = AppStoreSvc.GetAppIdsByUnionAppList<AppList>("10010", "10020");
        //    PrintDebug("Union applist1 and applist2, App count : " + appIdsUnion.Count);

        //    List<string> appIdsDiff = AppStoreSvc.GetAppIdsByDiffAppList<AppList>("10010", "10020");
        //    PrintDebug("Diff applist1 and applist2, App count : " + appIdsDiff.Count);
        //    #endregion

        //    #region Device Model

        //    DeviceModel tyd1 = new DeviceModel()
        //    {
        //        Id = "天奕达HWQP",
        //        ModelName = "天奕达HWQP",
        //        CreateDateTime = DateTime.Now
        //    };
        //    Redis.Add<DeviceModel>(tyd1);
        //    CustomProperty criteria1 = new CustomProperty()
        //    {
        //        Id = "Resolution",
        //        Value = "240*320",
        //        IsQueriable = false
        //    };
        //    CustomProperty criteria2 = new CustomProperty()
        //    {
        //        Id = "IsTouchable",
        //        Value = true,
        //        IsQueriable = false
        //    };
        //    PrintDebug("add device model : " + tyd1.ModelName);
        //    Redis.AddCustomPropertyFor<DeviceModel, CustomProperty>(tyd1.Id, criteria1);
        //    Redis.AddCustomPropertyFor<DeviceModel, CustomProperty>(tyd1.Id, criteria2);
        //    PrintDebug("add search criterias for device model : " + tyd1.ModelName);
        //    tyd1 = Redis.GetModelWithCustomProperties<DeviceModel, CustomProperty>("天奕达HWQP");
        //    PrintDebug("search by DeviceModel[天奕达HWQP]: " + AppStoreSvc.GetAppsForDeviceModel(tyd1).Count);
        //    //PrintDebug("Get Device Model By Request parameters : " + AppStoreSvc.GetDeviceModelByRequest(requestParams).ModelName);

        //    #endregion

        //    #region Clean up
        //    Redis.DeleteWithCustomProperties<App, CustomProperty>("10001");
        //    Redis.DeleteWithCustomProperties<App, CustomProperty>("10002");
        //    Redis.DeleteWithCustomProperties<AppList, CustomProperty>(applist1.Id);
        //    Redis.DeleteWithCustomProperties<AppList, CustomProperty>(applist2.Id);
        //    Redis.Delete<Category>(c1);
        //    Redis.Delete<Category>(csub1);
        //    Redis.Delete<Element>(ele);
        //    Redis.Delete<Element>(ele2);
        //    Redis.Delete<Element>(ele3);
        //    Redis.Delete<Element>(ele4);
        //    Redis.Delete<Element>(ele5);
        //    Redis.Delete<ElementDetail>(eleDetl);
        //    Redis.Delete<ElementDetail>(eleDetl1);
        //    Redis.Delete<ElementDetail>(eleDetl2);
        //    Redis.Delete<DeviceModel>(tyd1);

        //    if (Redis.KeyFuzzyFind("*").Count == 0)
        //    {
        //        PrintDebug("App Count: " + Redis.GetAllCount<App>());
        //        PrintDebug("All cleaned up.");
        //    }
        //    else
        //    {
        //        PrintDebug("Still useless keys left.");
        //    }

        //    #endregion
        //}

        private void PrintDebug(string output)
        {
#if DEBUG
            Console.WriteLine(output);
#endif
        }

    }
}
