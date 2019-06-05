using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using Infrastructure;

namespace Domain.Common.Cache
{
    public class LocalCache : BaseCache
    {
        protected static object Padlock = new object();

        public LocalCache()
        {
            CacheGroup = CacheGroup.Default;
            CachePool = MemoryCache.Default;
            DefaultExpireSecond = 30 * 60;
            IsSlidingExpiration = false;
        }

        MemoryCache CachePool { get; set; }

        /// <summary>
        /// false：到时间强制到期
        /// true：时间段内未访问自动到期
        /// </summary>
        public static bool IsSlidingExpiration { get; set; }

        public override CacheType CacheType { get { return CacheType.Local; } }

        public override void Set(CacheItem item, string key, object data, int second = 0)
        {
            if (data == null)
                return;
            var cacheKey = GenerateCacheKey(item, key);
            if (CachePool.Contains(cacheKey))
            {
                CachePool[cacheKey] = data;
            }
            else
            {
                CachePool.Add(cacheKey, data, GetPolicy(second));
            }
        }

        public override object Get(CacheItem item, string key, MethodInfo methodInfo)
        {
            var cacheKey = GenerateCacheKey(item, key);
            if (CachePool.Contains(cacheKey))
            {
                return (object)CachePool[cacheKey];
            }
            return null;
        }

        public override void Remove(CacheItem item, string key, bool isMainSlbServer = true)
        {
            var cacheKey = GenerateCacheKey(item, key);
            if (CachePool.Contains(cacheKey))
            {
                CachePool.Remove(cacheKey);
            }

        }

        public override void ClearItem(CacheItem item, bool isMainSlbServer = true)
        {
            lock (Padlock)
            {
                foreach (var cacheItem in CachePool.Where(p => p.Key.StartsWith(item.ToString())))
                {
                    CachePool.Remove(cacheItem.Key);
                }

            }
        }

        public override void ClearAll()
        {
            lock (Padlock)
            {
                foreach (var cacheItem in CachePool)
                {
                    CachePool.Remove(cacheItem.Key);
                }
            }
        }

        private CacheItemPolicy GetPolicy(int second)
        {
            if (second <= 0)
                second = DefaultExpireSecond;

            if (IsSlidingExpiration)
            {
                return new CacheItemPolicy
                {
                    SlidingExpiration = new TimeSpan(0, 0, second),
                };
            }
            return new CacheItemPolicy
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddSeconds(second)),
                };
        }

    }
}
