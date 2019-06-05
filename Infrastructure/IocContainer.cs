using System;
using Microsoft.Practices.Unity;
using Infrastructure.Extends;
using System.Collections.Generic;

namespace Infrastructure
{
    public class IocContainer
    {
        private static UnityContainer _myContainer = new UnityContainer();

        public static UnityContainer MyContainer
        {
            get { return _myContainer; }
        }

        public static T Resolve<T>(string name = "") where T : class
        {
            try
            {
                var result = name.IsNullOrEmpty() ? MyContainer.Resolve<T>() : MyContainer.Resolve<T>(name);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static IEnumerable<object> ResolveAll(Type t, params ResolverOverride[] resolverOverrides)
        {
            return MyContainer.ResolveAll(t, resolverOverrides);
        }

        public static object Resolve(Type t, string name = "", params ResolverOverride[] resolverOverrides)
        {
            var result = name.IsNullOrEmpty()
                ? MyContainer.Resolve(t, resolverOverrides)
                : MyContainer.Resolve(t, name, resolverOverrides);
            return result;
        }

        public static IUnityContainer RegisterType<TFrom, TTo>(LifetimeManager lm, string name = "",
            params InjectionMember[] im)
            where TTo : TFrom
        {
            var iUnityContainer = name.IsNullOrEmpty()
                ? MyContainer.RegisterType<TFrom, TTo>(lm, im)
                : MyContainer.RegisterType<TFrom, TTo>(name, lm, im);
            return iUnityContainer;
        }
    }
}
