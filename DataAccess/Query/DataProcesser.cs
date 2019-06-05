using Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Query
{
    /// <summary>
    /// 数据二次加工
    /// </summary>
    public class DataProcesser : DbAccess
    {
        public PageData Process(string reportName, SqlParams pars, bool isExport, bool isShowFooter = false)
        {
            PageData result;
            DataTable data;

            var db = GetReportConfigDb(reportName);
            //Sql执行前加工
            switch (reportName)
            {
                //case "ConsumerCardTimes_GetListForLdy":
                //    Process_ConsumerCardTimes_GetListForLdy(data);
                //    break;
            }
            if (isExport)
            {
                data = db.GetExportData(reportName, pars);
                result = new PageData(data, 0);
            }
            else
            {
                result = db.GetPageData(reportName, pars);
                data = result.Data;
            }

            //Sql执行后加工
            switch (reportName)
            {
                //case "ConsumerCardTimes_GetListForLdy":
                //    Process_ConsumerCardTimes_GetListForLdy(data);
                //    break;
            }
            return result;
        }
        private void Promotion_ExportPromotionActivityList_Export(DataTable data)
        {

            //if (data.Rows.Count > 0)
            //{
            //    for (int i = data.Rows.Count - 1; i >= 0; i--)
            //    {
            //        string ShopRetailPrice = data.Rows[i]["ShopRetailPrice"].ToString();
            //        if (ShopRetailPrice == "0.00")
            //        {
            //            data.Rows[i]["ShopRetailPrice"] = "";

            //        }
            //        if (ShopRetailPrice.Contains("~"))
            //        {
            //            string[] SectionPrice = ShopRetailPrice.Split('~');
            //            if (SectionPrice[0] == SectionPrice[1])
            //            {
            //                data.Rows[i]["ShopRetailPrice"] = SectionPrice[1];
            //            }
            //        }
            //        var rows = data.AsEnumerable()
            //            .Where(p => p.Field<long>("Id") == data.Rows[i]["Id"].AsLong()
            //                        && p.Field<string>("ActivityName") == data.Rows[i]["ActivityName"].ToString()).ToList();
            //        if (rows.Count == 1)
            //        {
            //            continue;
            //        }
            //        if (rows.Count > 1)
            //        {
            //            if (data.Rows[i]["ShopRetailPrice"] == "" && data.Rows[i]["ProductName"] == "")
            //            {
            //                data.Rows[i].Delete();
            //                data.AcceptChanges();//通过此函数来提交删除

            //            }
            //        }

            //    }

            //}
        }

    }
}
