using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface ILazyLoad
    {
        void InitLazyLoad();
    }

    public static class DataSetExtensions
    {
        /// <summary>
        /// 标识为支持延迟加载功能的对象
        /// </summary>
        public static T LazyLoad<T>(this T obj) where T : class, ILazyLoad
        {
            if (obj == null)
                return null;

            obj.InitLazyLoad();

            return obj;
        }

        /// <summary>
        /// 标识为支持延迟加载功能的对象
        /// </summary>
        public static List<T> LazyLoad<T>(this List<T> objs) where T : class, ILazyLoad
        {
            if (objs == null)
                return null;

            foreach (var obj in objs)
            {
                obj.InitLazyLoad();
            }

            return objs;
        }

    }
}
