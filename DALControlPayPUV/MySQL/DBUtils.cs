using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALControlPayPUV.MySQL
{
    class DBUtils
    {
        public static MySql.Data.MySqlClient.MySqlConnection GetDBConnection()
        {
            string host = "10.38.23.15";
            int port = 3306;
            string database = "sedtfbankxml";
            string username = "root";
            string password = "12345678";

            return DBMySQLUtils.GetDBConnection(host, port, database, username, password);
        }

    }
}
