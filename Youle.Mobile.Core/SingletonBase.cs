
namespace Youle.Mobile.Core
{
    public abstract class SingletonBase<T>
        where T : new()
    {
        #region Static Field
        private static T s_Instance = new T();
        #endregion

        #region Static
        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get
            {
                return s_Instance;
            }
        }


        /// <summary>
        /// Initializes the specified customer cache.
        /// </summary>
        /// <param name="customerCache">The customer cache.</param>
        public static void Initialize(T instance)
        {
            s_Instance = instance;
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        public static void Reset()
        {
            s_Instance = default(T);
        }
        #endregion
    }
}
