using DataAccess.Query;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Common
{
    public class CommonReportService : BaseService
    {
        public CommonDataQuery CommonDataQuery = IocContainer.Resolve<CommonDataQuery>();

        public PageData GetPageData(string reportName, SqlParams pars, bool isShowFooter = false)
        {

            var result = CommonDataQuery.GetPageData(reportName, pars, isShowFooter);
            return result;
        }

        public DataTable GetExportData(string reportName, SqlParams pars)
        {
            return CommonDataQuery.GetExportData(reportName, pars);
        }

        public List<long> GetFullOperationIds(string reportName, SqlParams pars)
        {
            return CommonDataQuery.GetFullOperationIds(reportName, pars);
        }

        public DataTable GetSummaryData(string reportName, SqlParams pars)
        {
            return CommonDataQuery.GetSummaryData(reportName, pars);
        }

        public DataTable SuperQueryExecute(string query, SqlParameter[] parameters)
        {
            return CommonDataQuery.SuperQueryExecute(query, parameters);
        }
    }
}
