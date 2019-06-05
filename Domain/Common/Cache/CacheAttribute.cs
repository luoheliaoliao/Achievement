using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ArxOne.MrAdvice.Advice;
using Infrastructure.Extends;

namespace Domain.Common.Cache
{
    [Serializable]
    public class CacheAttribute : Attribute, IMethodAdvice
    {
        public CacheAttribute(CacheItem item, int expireSecond = 0, CacheGroup groupType = CacheGroup.Default)
        {
            CacheItem = item;
            ExpireSecond = expireSecond;
            CacheGroup = groupType;
        }

        public CacheItem CacheItem { get; set; }
        public CacheGroup CacheGroup { get; set; }
        public int ExpireSecond { get; set; }

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="args"></param>
        public void OnEntry(MethodAdviceContext args)
        {
            var cacheKey = GetCacheKey(args);
            var cache = CacheManager.Get(CacheGroup);
            var cacheData = cache.Get(CacheItem, cacheKey, args.TargetMethod as MethodInfo);

            if (cacheData != null)
            {
                args.ReturnValue = cacheData;
                //args.FlowBehavior = FlowBehavior.Return;
            }
        }

        /// <summary>
        /// 添加Cache
        /// </summary>
        /// <param name="args"></param>
        public void OnSuccess(MethodAdviceContext args)
        {
            var cacheKey = GetCacheKey(args);
            //if (string.IsNullOrEmpty(cacheKey))
            //    return;

            var cache = CacheManager.Get(CacheGroup);

            var cacheData = args.ReturnValue;
            cache.Set(CacheItem, cacheKey, cacheData, ExpireSecond);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static string GetCacheKey(MethodAdviceContext args)
        {
            var sb = new StringBuilder();
            int i = 0;
            foreach (var parameter in args.TargetMethod.GetParameters())
            {
                if (parameter.GetCustomAttributes(typeof(CacheKeyAttribute), false).Count() > 0)
                {
                    if (args.Arguments.Count < i)
                        throw new Exception("CacheKey生成失败，请排查！");

                    var argument = args.Arguments[i];
                    if (argument == null)
                        continue;
                    var type = args.Arguments[i].GetType();
                    if (type.IsValueType || type == typeof(string))
                        sb.Append("|" + args.Arguments[i]);
                    else
                        sb.Append("|" + args.Arguments[i].ToJson());
                }
                i++;
            }
            return sb.ToString().TrimStart('|');
        }

        public void Advise(MethodAdviceContext context)
        {
            OnEntry(context);
            if (context.ReturnValue != null)
                return;

            context.Proceed();
            OnSuccess(context);
        }
    }
}
