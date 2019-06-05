using Common;
using Domain.Users;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    /// <summary>
    /// 数据库使用入口
    /// </summary>
    [DbConfigurationType(typeof(MyDbConfiguration))]
    public class DB : BaseDB
    {
        public DB(string conn, bool isReadOnlyDb = false)
            : base(new SqlConnection(conn), false)
        {
            ConnectionString = conn;
            Database.SetInitializer<DB>(null);
            //Database.SetInitializer<DB>(new CreateDatabaseIfNotExists<DB>());
        }

        public static DB Create(bool isReadOnlyDb = false)
        {
            return new DB(Connection.GetConnectionString(isReadOnlyDb), isReadOnlyDb);
        }

        protected string ConnectionString;


        public DbSet<Role> Role { get; set; }
        public DbSet<KjUser> KjUser { get; set; }
        public DbSet<RoleButton> RoleButton { get; set; }
        public DbSet<RoleMenu> RoleMenu { get; set; }
        public DbSet<UserRole> UserRole { get; set; }

        #region ADO 操作

        private SqlConnection _sqlConnection;
        public SqlConnection SqlConnection
        {
            get
            {
                if (_sqlConnection == null)
                {
                    _sqlConnection = new SqlConnection();
                    _sqlConnection.ConnectionString = this.ConnectionString;
                }

                return _sqlConnection;
            }
            set { _sqlConnection = value; }
        }

        /// <summary>
        /// 获取分页数据(注意：该方法不包括在事务中！)
        /// </summary>
        /// <param name="reportName"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public PageData GetPageData(string reportName, SqlParams sqlParams)
        {
            var reportConfig = new ReportConfig(reportName, this);

            return reportConfig.GetPageData(sqlParams);
        }

        /// <summary>
        /// 获取导出数据(注意：该方法不包括在事务中！)
        /// </summary>
        /// <param name="reportName"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public DataTable GetExportData(string reportName, SqlParams sqlParams)
        {
            var reportConfig = new ReportConfig(reportName, this);

            return reportConfig.GetExportData(sqlParams);
        }

        /// <summary>
        /// 通过报表获取全量操作所需要的Ids
        /// </summary>
        /// <param name="reportName"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public List<long> GetFullOperationIds(string reportName, SqlParams sqlParams)
        {
            var reportConfig = new ReportConfig(reportName, this);

            return reportConfig.GetFullOperationIds(sqlParams);
        }

        /// <summary>
        /// 获取统计数据(注意：该方法不包括在事务中！)
        /// </summary>
        /// <param name="reportName"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public DataTable GetSummaryData(string reportName, SqlParams sqlParams)
        {
            var reportConfig = new ReportConfig(reportName, this);

            return reportConfig.GetSummaryData(sqlParams);
        }

        /// <summary>
        /// 执行一条不返回结果的SqlCommand，通过一个已经存在的数据库连接 
        /// 使用参数数组提供参数
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>返回一个数值表示此SqlCommand命令执行后影响的行数</returns>
        public int ExecuteNonQuery(string sql, SqlParameter[] parameters)
        {
            return Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, sql, parameters);
        }

        /// <summary>
        /// 执行一条返回结果的SqlCommand，通过一个已经存在的数据库连接
        /// 使用参数数组提供参数
        /// </summary>
        /// <param name="sql">SQL 语句</param>
        /// <param name="parameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>返回DbRawSqlQuery类型的结果</returns>
        public IEnumerable<T> SqlQuery<T>(string sql, params SqlParameter[] parameters)
        {
            parameters = parameters ?? new SqlParameter[0];
            return Database.SqlQuery<T>(sql, parameters);
        }

        public IEnumerable<T> SqlQuery<T>(string reportName, SqlParams sqlParams)
        {
            var reportConfig = new ReportConfig(reportName, this);

            return reportConfig.SqlQuery<T>(sqlParams);
        }

        /// <summary>
        /// EF SQL 语句返回 DataSet(注意：该方法不包括在事务中！)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql, SqlParameter[] parameters)
        {
            var ds = ExecuteDataSet(sql, parameters);
            if (ds == null || ds.Tables.Count == 0)
                return null;

            return ds.Tables[0];
        }

        /// <summary>
        /// EF SQL 语句返回 DataSet(注意：该方法不包括在事务中！)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sql, SqlParameter[] parameters)
        {
            var cmd = CreateSqlCommand(sql, parameters);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            var ds = new DataSet();
            adapter.Fill(ds);
            cmd.Connection.Close();
            cmd.Dispose();
            return ds;
        }

        /// <summary>
        /// EF SQL 语句返回 DataSet(注意：该方法不包括在事务中！)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, SqlParameter[] parameters)
        {
            var cmd = CreateSqlCommand(sql, parameters);

            return cmd.ExecuteScalar();
        }

        private SqlCommand CreateSqlCommand(string sql, SqlParameter[] parameters)
        {
            if (SqlConnection.State != ConnectionState.Open)
            {
                SqlConnection.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = SqlConnection;
            cmd.CommandText = sql;

            if (parameters.Length > 0)
            {
                foreach (var item in parameters)
                {
                    cmd.Parameters.Add(item);
                }
            }

            return cmd;
        }

        #endregion

        public static SqlParameter[] DictToSqlParameters(Dictionary<string, object> source)
        {
            var result = new List<SqlParameter>();
            foreach (var key in source.Keys)
            {
                if (result.All(p => p.ParameterName != key.ToLower()))
                    result.Add(new SqlParameter(key.ToLower(), source[key]));
            }

            return result.ToArray();
        }
    }

    public class MyDbConfiguration : DbConfiguration
    {
        public MyDbConfiguration()
        {
            SetProviderServices(
                SqlProviderServices.ProviderInvariantName,
                SqlProviderServices.Instance);
        }
    }
}
