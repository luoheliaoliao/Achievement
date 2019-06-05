using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Connection
    {
        /// <summary>
        /// 连接串
        /// </summary>
        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["SqlConnString"].ConnectionString; }
        }

        public static string GetConnectionString(bool isReadOnly = false)
        {
            //if (isReadOnly)
            //    return ReadOnlyConnectionString;

            return ConnectionString;
        }
    }
}
