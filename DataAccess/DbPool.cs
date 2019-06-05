using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccess
{
    /// <summary>
    /// 数据库连接对象池，每个请求会拥有一个DB实例
    /// </summary>
    public class DbPool : IDbPool
    {
        private static readonly Dictionary<string, DB> DbPoolDict = new Dictionary<string, DB>();
        private static readonly object PadLock = new object();
        public const string ReadOnlyDbPrefix = "ReadOnly-";
        public DB GetDb()
        {
            var key = GetCurrentKey();
            return GetDb(key);
        }

        public DB GetReportConfigDb(ReportConfig reportConfig)
        {
            return GetDb();
        }

        public DB GetReadOnlyDb()
        {
            var key = GetCurrentKey(ReadOnlyDbPrefix);
            return GetDb(key, true);
        }

        public DB GetDb(string key, bool isReadOnly = false)
        {
            lock (PadLock)
            {
                if (DbPoolDict.ContainsKey(key))
                    return DbPoolDict[key];

                DbPoolDict.Add(key, DB.Create(isReadOnly));
                return DbPoolDict[key];
            }
        }

        IDB IDbPool.GetDb(string key)
        {
            return GetDb(key);
        }

        public void ChangeKey(string oldKey, string newKey)
        {
            var db = GetDb(oldKey);
            lock (PadLock)
            {
                DbPoolDict.Remove(oldKey);
                DbPoolDict.Add(newKey, db);
            }
        }

        public void DisposeDb(string key = null)
        {
            if (key == null)
                key = GetCurrentKey();
            lock (PadLock)
            {
                if (!DbPoolDict.ContainsKey(key))
                    return;
                var db = DbPoolDict[key];
                db.Database.Connection.Close();
                db.SqlConnection.Close();
                db.Dispose();
                DbPoolDict.Remove(key);
            }
        }

        public string GetCurrentKey(string prefix = "")
        {
            //if (HttpContext.Current == null || HttpContext.Current.Session == null)
            return prefix + Thread.CurrentThread.ManagedThreadId.ToString();

            //var sessionId = HttpContext.Current.Session.SessionId;
            //var hashCode = HttpContext.Current.Request.GetHashCode();

            //return sessionId + hashCode;
        }
    }
}
