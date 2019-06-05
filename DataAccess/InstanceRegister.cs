using Common;
using DataAccess.Repository.Common.SystemMenu;
using Domain.Common.SystemMenu;
using Domain.Users.Repository;
using Infrastructure;
using Microsoft.Practices.Unity;

namespace DataAccess
{
    public class InstanceRegister
    {
        public static void Regist()
        {
            IocContainer.RegisterType<IDbPool, DbPool>(new ContainerControlledLifetimeManager());
            IocContainer.RegisterType<IKjUserRepository, KjUserRepository>(new ContainerControlledLifetimeManager());
            IocContainer.RegisterType< IRolerRepository, RolerRepository> (new ContainerControlledLifetimeManager());
            IocContainer.RegisterType<ISystemMenuRepository, SystemMenuRepository>(new ContainerControlledLifetimeManager());
        }
    }
}
