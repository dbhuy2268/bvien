using ConnectOracleWithoutClient;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Tutorial.SqlConn
{
    class DBUtils
    {
        public static OracleConnection GetDBConnection()
        {
            string host = "localhost";
            int port = 1521;
            string sid = "orcl";
            string user = Globals.curr_session_id;
            string password = Globals.curr_session_pw;

            return DBOracleUtils.GetDBConnection(host, port, sid, user, password);
        }

        public OracleConnection conn = GetDBConnection();

        public DataTable ExecuteQuery(string query, List<OracleParameter> parameters = null)
        {
            DataTable dt = new DataTable();

            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.CommandType = CommandType.Text;
                if (parameters != null)
                {
                    foreach (OracleParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("## Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public bool ExecuteNonQuery(String query, List<OracleParameter> parameters = null)
        {
            bool flag = true;
            try
            {
                this.conn.Open();
                OracleCommand cmd = new OracleCommand(query, this.conn);
                cmd.CommandType = CommandType.Text;
                if (parameters != null)
                {
                    foreach (OracleParameter param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                }
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                flag = false;
                throw new Exception("Error execute non query: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return flag;
        }
    }
}