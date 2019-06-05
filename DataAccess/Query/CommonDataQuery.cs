using Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Query
{
    /// <summary>
    /// 通用查询方法
    /// </summary>
    public class CommonDataQuery : DbAccess
    {
        public DataProcesser DataProcesser = IocContainer.Resolve<DataProcesser>();

        public PageData GetPageData(string reportName, SqlParams pars, bool isShowFooter = false)
        {
            return DataProcesser.Process(reportName, pars, false, isShowFooter);
        }

        public DataTable GetExportData(string reportName, SqlParams pars)
        {
            return DataProcesser.Process(reportName, pars, true).Data;
        }

        public List<long> GetFullOperationIds(string reportName, SqlParams pars)
        {
            return GetReportConfigDb(reportName).GetFullOperationIds(reportName, pars);
        }

        public DataTable GetSummaryData(string reportName, SqlParams pars)
        {
            return GetReportConfigDb(reportName).GetSummaryData(reportName, pars);
        }

        public DataTable SuperQueryExecute(string query, SqlParameter[] parameters)
        {
            return Db.ExecuteDataTable(query, parameters);
        }
    }
}
