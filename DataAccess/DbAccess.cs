using Common;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    /// <summary>
    /// 继承该类才允许使用DB
    /// </summary>
    public abstract class DbAccess
    {
        public DbPool DbPool = IocContainer.Resolve<IDbPool>() as DbPool;
        public DB Db { get { return DbPool.GetDb(); } }

        public DB GetReportConfigDb(string reportName)
        {
            var reportConfig = new ReportConfig(reportName);
            return GetReportConfigDb(reportConfig);
        }

        public DB GetReportConfigDb(ReportConfig reportConfig)
        {
            return DbPool.GetReportConfigDb(reportConfig);
        }
    }
}
