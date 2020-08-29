using Oracle.DataAccess.Client;
using System;

namespace Tutorial.SqlConn
{
    class DBOracleUtils
    {

        public static OracleConnection GetDBConnection(string host, int port, String sid, String user, String password)
        {
            // 'Connection String' kết nối trực tiếp tới Oracle.
            //string connString = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = "
            //     + host + ")(PORT = " + port + "))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME = "
            //     + sid + ")));Password=" + password + ";User ID=" + user;
            string connString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST="
                + host + ")(PORT=1521)))(CONNECT_DATA=(SID=" + sid + ")));User ID=" + user + ";Password=" + password;

            OracleConnection conn = new OracleConnection();

            conn.ConnectionString = connString;

            return conn;
        }

    }
}