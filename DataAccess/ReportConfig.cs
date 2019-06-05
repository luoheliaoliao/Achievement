using Domain.Common.Cache;
using Infrastructure;
using Infrastructure.Extends;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataAccess
{
    /// <summary>
    /// ReportConfig管理类
    /// </summary>
    public class ReportConfig
    {
        public ReportConfig(string reportName, DB db = null)
        {
            this.ReportName = reportName;
            this.Db = db;
            this.XmlConfig = AllReportConfig.Elements("Rep").FirstOrDefault(q => q.Attribute("key").Value == ReportName);

            if (XmlConfig == null)
            {
                throw new Exception("xml没有相关配置");
            }
        }

        /// <summary>
        /// 获取sql语句xml
        /// </summary>
        /// <returns></returns>
        [Cache(CacheItem.ReportConfig_AllReportConfig, int.MaxValue)]
        public static XElement AllReportConfig
        {
            get
            {
                var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\ReportConfig\\", "*.ReportConfig.xml");
                var obj = new XElement("Reps");
                foreach (var file in files)
                {
                    obj.Add(XElement.Load(file).Elements("Rep"));
                }
                return obj;
            }
        }

        public string ReportName { get; set; }

        private XElement XmlConfig { get; set; }

        public bool IsQueryReadOnlyDb
        {
            get
            {
                var attribute = XmlConfig.Attribute("IsQueryReadOnlyDb");
                var result = attribute == null ? true : attribute.Value.ToBool();
                return result;
            }
        }

        public DB Db { get; set; }

        public PageData GetPageData(SqlParams sqlParams)
        {
            var result = new PageData();
            var reportsql = GetReportSql();
            var prepareSql = GetPrepareSql();
            var totalSql = GetTotalSql();
            var orderSql = GetOrderSql(sqlParams.Order);

            var parameters = ProcessDynamicParams(sqlParams, ref reportsql, ref totalSql, ref prepareSql);
            totalSql = prepareSql + totalSql;

            int start = (sqlParams.PageIndex - 1) * sqlParams.PageSize + 1;
            int end = sqlParams.PageIndex * sqlParams.PageSize + 1;
            reportsql = @" select * from (select *,ROW_NUMBER() over({0}) 
                                as rows from (" + reportsql + @")as t1) as t2
                                where t2.rows>={1} and t2.rows<{2} ";
            var sql = prepareSql + string.Format(reportsql, orderSql, start, end);
            result.Data = Db.ExecuteDataTable(sql, DB.DictToSqlParameters(parameters));
            result.Total = (int)Db.ExecuteScalar(totalSql, DB.DictToSqlParameters(parameters));
            //if (footerSql != "")
            //    result.Footer = Db.ExecuteDataTable(footerSql, DB.DictToSqlParameters(parameters));

            return result;
        }

        public DataTable GetExportData(SqlParams sqlParams)
        {
            var reportsql = GetReportSql();
            var prepareSql = GetPrepareSql();

            var orderSql = GetOrderSql(sqlParams.Order);

            var parameters = ProcessDynamicParams(sqlParams, ref reportsql, ref prepareSql);

            var sql = prepareSql + reportsql + orderSql;
            return Db.ExecuteDataTable(sql, DB.DictToSqlParameters(parameters));
        }

        public List<long> GetFullOperationIds(SqlParams sqlParams)
        {
            var prepareSql = GetPrepareSql();
            var reportsql = GetReportSql();

            var parameters = ProcessDynamicParams(sqlParams, ref reportsql, ref prepareSql);

            var sql = prepareSql + string.Format(@" SELECT Id FROM ({0}) AS FullOperationTemp", reportsql);
            return Db.SqlQuery<long>(sql, DB.DictToSqlParameters(parameters)).ToList();
        }

        public DataTable GetSummaryData(SqlParams sqlParams)
        {
            var reportsql = GetReportSql();
            var prepareSql = GetPrepareSql();

            var parameters = ProcessDynamicParams(sqlParams, ref reportsql, ref prepareSql);
            var sql = prepareSql + reportsql;

            return Db.ExecuteDataTable(sql, DB.DictToSqlParameters(parameters));
        }

        public IEnumerable<T> SqlQuery<T>(SqlParams sqlParams)
        {
            var reportsql = GetReportSql();
            var prepareSql = "";
            var parameters = ProcessDynamicParams(sqlParams, ref reportsql, ref prepareSql);

            return Db.SqlQuery<T>(reportsql, DB.DictToSqlParameters(parameters));
        }

        private List<ReportWhereEntity> FillReportWhereEntity(XElement root)
        {
            List<ReportWhereEntity> list = new List<ReportWhereEntity>();
            foreach (var dyItem in root.Elements("c"))
            {
                ReportWhereEntity result = new ReportWhereEntity();
                result.Data = dyItem.Value;
                result.Prepend = dyItem.Attribute("prepend").Value;
                result.Type = dyItem.Attribute("type").Value;
                result.Property = dyItem.Attribute("property").Value;
                list.Add(result);
            }
            return list;
        }

        private string GetDynamicStr(List<ReportWhereEntity> list, Dictionary<string, object> condition, out Dictionary<string, object> tag)
        {
            var sb = new StringBuilder();
            tag = new Dictionary<string, object>();
            Dictionary<string, object> conditionDict = new Dictionary<string, object>();
            foreach (var item in condition)
            {
                conditionDict.Add(item.Key.ToLower(), item.Value);
            }
            bool isSet = false;
            foreach (var item in list)
            {
                item.Property = item.Property.ToLower();
                item.Data = item.Data.ToLower();
                if (item.Type.Trim().ToLower() == "staticcondition")
                {
                    if (sb.Length != 0)
                    {
                        sb.Append(item.Prepend);
                    }
                    if (item.Prepend.Trim() == ",")
                    {
                        isSet = true;
                        sb.Append(item.Data);
                    }
                    else
                    {
                        sb.Append(" ( ").Append(item.Data).Append(" ) ");
                    }
                    continue;
                }
                if (!conditionDict.Keys.Contains(item.Property))
                {
                    continue;
                }
                if (tag.Keys.Contains(item.Property))
                {
                    continue;
                }
                if (conditionDict[item.Property] == null || (conditionDict[item.Property] is string && string.IsNullOrEmpty(conditionDict[item.Property].ToString())))
                {
                    continue;
                }
                if (item.Type.Trim() == string.Empty)
                {
                    if (sb.Length != 0)
                    {
                        sb.Append(item.Prepend);
                    }
                    if (item.Prepend.Trim() == ",")
                    {
                        isSet = true;
                        sb.Append(item.Data);
                    }
                    else
                    {
                        sb.Append(" ( ").Append(item.Data).Append(" ) ");
                    }
                    tag.Add(item.Property, conditionDict[item.Property]);
                }
                else if (item.Type.Trim().ToLower() == "in")
                {
                    var value = conditionDict[item.Property].ToString().Trim(',');

                    if (string.IsNullOrEmpty(value))
                    {
                        continue;
                    }
                    string str = " (";
                    for (int i = 0; i < value.Split(',').Count(); i++)
                    {
                        string keyStr = item.Property + "__" + i.ToString();
                        str = str + "@" + keyStr + ",";
                        tag.Add(keyStr, value.Split(',')[i]);
                    }
                    str = str.Trim(',') + ") ";
                    if (sb.Length != 0)
                    {
                        sb.Append(item.Prepend);
                    }
                    sb.Append(" ( ").Append(item.Data.Replace("@" + item.Property, str)).Append(" ) ");
                }
                else if (item.Type.Trim().ToLower() == "like")
                {
                    if (sb.Length != 0)
                    {
                        sb.Append(item.Prepend);
                    }
                    sb.Append(" ( ").Append(item.Data).Append(" ) ");
                    tag.Add(item.Property, "%" + conditionDict[item.Property] + "%");
                }
                else if (item.Type.Trim().ToLower() == "leftlike")
                {
                    if (sb.Length != 0)
                    {
                        sb.Append(item.Prepend);
                    }
                    sb.Append(" ( ").Append(item.Data).Append(" ) ");
                    tag.Add(item.Property, conditionDict[item.Property] + "%");
                }
                else if (item.Type.Trim().ToLower() == "rightlike")
                {
                    if (sb.Length != 0)
                    {
                        sb.Append(item.Prepend);
                    }
                    sb.Append(" ( ").Append(item.Data).Append(" ) ");
                    tag.Add(item.Property, "%" + conditionDict[item.Property]);
                }
            }
            if (!isSet && sb.Length == 0)
            {
                sb.Append("(1=1)");
            }
            string result = sb.ToString();
            return result;
        }

        private Dictionary<string, object> ProcessDynamicParams(SqlParams sqlParams, ref string reportsql, ref string prepareSql)
        {
            var totalSql = "";
            return ProcessDynamicParams(sqlParams, ref reportsql, ref prepareSql, ref totalSql);
        }

        private Dictionary<string, object> ProcessDynamicParams(SqlParams sqlParams, ref string reportsql, ref string prepareSql, ref string totalSql)
        {
            var parameters = new Dictionary<string, object>();
            var elements = XmlConfig.Elements("Dynamic");
            if (XmlConfig.Attribute("BaseType") != null)
            {
                var baseConfig = AllReportConfig.Elements("Rep").FirstOrDefault(q => q.Attribute("key").Value == XmlConfig.Attribute("BaseType").Value);
                if (baseConfig != null)
                {
                    foreach (var item in baseConfig.Elements("Dynamic"))
                    {
                        var property = elements.FirstOrDefault(p => p.Attribute("property").Value == item.Attribute("property").Value);
                        if (property != null)
                        {
                            property.Add(item.Elements());
                        }
                    }
                }
            }

            foreach (var itemEle in elements)
            {
                Dictionary<string, object> dataRes;
                List<ReportWhereEntity> list = FillReportWhereEntity(itemEle);
                string dynamicStr = GetDynamicStr(list, sqlParams.Where, out dataRes);
                string dyProperty = itemEle.Attribute("property").Value;

                if (!string.IsNullOrEmpty(reportsql))
                    reportsql = reportsql.Replace(dyProperty, dynamicStr);
                //if (!string.IsNullOrEmpty(footerSql))
                //    footerSql = footerSql.Replace(dyProperty, dynamicStr);
                if (!string.IsNullOrEmpty(prepareSql))
                    prepareSql = prepareSql.Replace(dyProperty, dynamicStr);
                if (!string.IsNullOrEmpty(totalSql))
                    totalSql = totalSql.Replace(dyProperty, dynamicStr);

                if (dataRes != null)
                {
                    foreach (var item in dataRes)
                    {
                        if (parameters.All(p => p.Key != item.Key.ToLower()))
                        {
                            parameters.Add(item.Key.ToLower(), item.Value);
                        }
                    }
                }
            }

            foreach (var item in sqlParams.Where)
            {
                if (parameters.All(p => p.Key != item.Key.ToLower()))
                {
                    parameters.Add(item.Key.ToLower(), item.Value);
                }
            }
            return parameters;
        }

        private string GetPrepareSql()
        {
            var result = XmlConfig.Element("PrepareSql") == null ? "" : XmlConfig.Element("PrepareSql").Value;
            return result;
        }

        private string GetReportSql()
        {
            var result = XmlConfig.Element("ReportSql") == null ? "" : XmlConfig.Element("ReportSql").Value;
            if (string.IsNullOrEmpty(result))
                throw new Exception(string.Format("请为报表{0}配置ReportSql", ReportName));

            return result;
        }

        private string GetTotalSql()
        {
            var result = XmlConfig.Element("TotalSql") == null ? "" : XmlConfig.Element("TotalSql").Value;
            if (string.IsNullOrEmpty(result))
                throw new Exception(string.Format("请为报表{0}配置TotalSql", ReportName));

            return result;
        }

        private string GetOrderSql(string order)
        {
            if (!string.IsNullOrEmpty(order))
                return "ORDER BY " + order;
            var result = XmlConfig.Element("OrderSql") == null ? "" : XmlConfig.Element("OrderSql").Value;
            if (!string.IsNullOrEmpty(result))
                return result;

            return "ORDER BY Id DESC";
        }

        #region 内部类

        class ReportWhereEntity
        {
            public string Property { get; set; }
            public string Prepend { get; set; }
            public string Type { get; set; }
            public string Data { get; set; }
        }

        #endregion

    }
}
