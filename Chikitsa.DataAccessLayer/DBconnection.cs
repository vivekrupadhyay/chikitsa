using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Chikitsa.DataAccessLayer
{
    public class DBconnection
    {
        public static SqlConnection ConnectToDB()
        {
            SqlConnection objConn = new SqlConnection(ConfigurationManager.ConnectionStrings["ChikitsaConString"].ConnectionString);
            if (objConn.State != ConnectionState.Open)
                objConn.Open();
            else
                throw new Exception("Database is already Open.");
            return objConn;

        }

        private static void CloseDB(ref SqlConnection objConn)
        {
            if (objConn.State == ConnectionState.Open)
                objConn.Close();
            else
                throw new Exception("Needs an open connection");
            objConn = null;
        }

        internal static void CloseDB(SqlConnection objConn)
        {
            if (objConn.State == ConnectionState.Open)
                objConn.Close();
            else
                throw new Exception("Needs an open connection");
            objConn = null;
        }

        public static void Insert_Trans(SqlCommand sqlcmd)
        {
            SqlConnection objConn = ConnectToDB();
            sqlcmd.Connection = objConn;
            sqlcmd.ExecuteNonQuery();
            CloseDB(ref objConn);
        }

        public static DataSet GetAsDataSet(SqlCommand objCmd, string RecordsetName)
        {
            DataSet objDs = new DataSet();
            SqlConnection objConn = ConnectToDB();
            objCmd.Connection = objConn;
            SqlDataAdapter objDA = new SqlDataAdapter(objCmd);
            objDA.Fill(objDs, RecordsetName);
            CloseDB(ref objConn);
            objDA.Dispose();
            return objDs;
        }

        public static DataSet GetAsDataSet(string qry, string RecordsetName)
        {
            SqlConnection objConn = ConnectToDB();
            SqlCommand objCmd = new SqlCommand(qry);
            objCmd.Connection = objConn;
            SqlDataAdapter objDA = new SqlDataAdapter(objCmd);
            DataSet objDs = new DataSet();
            objDA.Fill(objDs, RecordsetName);
            CloseDB(ref objConn);
            objDA.Dispose();
            return objDs;
        }

        public static int ExecuteNonQuery(SqlCommand sqlcmd)
        {
            SqlConnection objConn = null;
            try
            {
                int inst;
                objConn = ConnectToDB();
                SqlCommand cmd = sqlcmd;
                cmd.Connection = objConn;
                inst = cmd.ExecuteNonQuery();
                return inst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CloseDB(objConn);
            }
        }

        public static object ExecuteScalar(SqlCommand sqlcmd)
        {
            SqlConnection objConn = ConnectToDB();
            sqlcmd.Connection = objConn;
            object firstColumn = new object();
            firstColumn = sqlcmd.ExecuteScalar();
            return firstColumn;
        }

        public static DataSet GetAsDataSet(SqlCommand objCmd)
        {
            SqlConnection objConn = ConnectToDB();
            objCmd.Connection = objConn;
            SqlDataAdapter objDA = new SqlDataAdapter(objCmd);
            DataSet objDs = new DataSet();
            objDA.Fill(objDs);
            CloseDB(ref objConn);
            objDA.Dispose();
            return objDs;
        }

        internal static int ExecuteQueryforupdate(SqlCommand nQuery)
        {
            int retID = -1;
            SqlConnection objConn = ConnectToDB();
            SqlCommand ObjCmd = nQuery;
            ObjCmd.Connection = objConn;
            retID = ObjCmd.ExecuteNonQuery();
            ObjCmd.Dispose();
            CloseDB(ref objConn);
            return retID;

        }

        internal static int ExecuteQuery(string nQuery)
        {
            int retID;
            SqlConnection objConn = ConnectToDB();
            SqlCommand ObjCmd = new SqlCommand(nQuery, objConn);
            SqlDataReader objDr;
            objDr = ObjCmd.ExecuteReader();
            if (objDr.Read())
                retID = 1;
            else
                retID = 0;
            objDr.Close();
            ObjCmd.Dispose();
            CloseDB(ref objConn);
            return retID;
        }

        internal static void ExecuteQuery(SqlCommand nQuery)
        {
            SqlConnection objConn = ConnectToDB();
            nQuery.Connection = objConn;
            nQuery.ExecuteNonQuery();
            CloseDB(ref objConn);
        }

        public static SqlDataReader ExecuteReader(SqlCommand sqlcmd)
        {
            SqlDataReader dr = null;
            using (SqlConnection objConn = ConnectToDB())
            {
                try
                {
                    sqlcmd.Connection = objConn;
                    dr = sqlcmd.ExecuteReader();
                }
                catch (Exception er)
                {
                    throw er;
                }
                finally
                {
                    CloseDB(objConn);
                }
            }
            return dr;
        }
    }
}
