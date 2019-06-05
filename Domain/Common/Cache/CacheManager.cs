using System.Configuration;
using Infrastructure;
using Infrastructure.Extends;

namespace Domain.Common.Cache
{
    public class CacheManager
    {
        static CacheManager()
        {
            Default = IocContainer.Resolve<ICache>(CacheType.Local.ToString());
        }
        /// <summary>
        /// 默认内存缓存
        /// </summary>
        public static ICache Default { get; private set; }

        /// <summary>
        /// 根据枚举CacheGroup来切换缓存
        /// </summary>
        /// <param name="groupType"></param>
        /// <returns></returns>
        public static ICache Get(CacheGroup groupType = CacheGroup.Default)
        {
            if (!ConfigurationManager.AppSettings["IsEnableCache"].ToBool())
                return IocContainer.Resolve<ICache>(CacheType.Empty.ToString());

            CacheType cacheType;
            switch (groupType)
            {
                case CacheGroup.Default:
                default:
                    cacheType = CacheType.Local;
                    break;
            }
            return IocContainer.Resolve<ICache>(cacheType.ToString());
        }

        /// <summary>
        /// 清除所有缓存(如果内存缓存和Redis缓存共存，就会把这俩种缓存都清除)
        /// </summary>
        public static void ClearAll()
        {
            var cacheList = IocContainer.ResolveAll(typeof(ICache));
            if (cacheList != null)
            {
                foreach (var item in cacheList)
                {
                    if (item == null)
                        continue;
                    ((ICache)item).ClearAll();
                }
            }
        }
    }
}
