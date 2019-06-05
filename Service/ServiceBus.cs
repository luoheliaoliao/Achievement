using Common;
using DataAccess;
using Domain.Common.Cache;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceBus
    {
        public static void Init()
        {
            //   RuntimeHelper.Init(true);
            InitContainer();
            //   TimerManager.Init();
            CacheManager.ClearAll();
        }

        /// <summary>
        /// IOC容器初始化
        /// </summary>
        private static void InitContainer()
        {
            InstanceRegister.Regist();
            //自动化验收用，不允许用于正式环境
            //AutoTestInstanceRegister.Regist();
        }

        /// <summary>
        /// 消息队列初始化
        /// </summary>

        /// <summary>
        /// DisposeDb
        /// </summary>
        public static void DisposeDb()
        {
            IocContainer.Resolve<IDbPool>().DisposeDb();
        }
    }
}
