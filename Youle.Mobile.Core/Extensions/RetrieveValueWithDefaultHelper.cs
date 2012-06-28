using System;

namespace Youle.Mobile.Core
{
    public class RetrieveValueWithDefaultHelper
    {
        public static T TryGet<T>(Func<T> tryGet, Func<T> defalutValueFunc)
        {
            var value = default(T);

            try
            {
                value = tryGet();
            }
            catch
            {
                //LogHelper.Error(string.Format("DefaultValueRetriving account error: {0}", ex.Message));
            }
            if (value == null && defalutValueFunc != null)
            {
                value = defalutValueFunc();
            }

            return value;
        }
    }
}
