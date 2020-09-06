
using Chikitsa.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chikitsa.DataAccessLayer
{
    public class MenuDAL
    {
        public List<Menu> Get(QueryBO objQuery)
        {
            SqlDataReader reader = null;
            SqlConnection con = null;
            List<Menu> lstObject = null;
            SqlCommand objcmd = null;
            Menu obj = null;
            try
            {
                lstObject = new List<Menu>();
                using (con = DBconnection.ConnectToDB())
                {
                    objcmd = new SqlCommand();
                    objcmd.Connection = con;
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.CommandText = "STP_SelectMenuMaster";
                    objcmd.Parameters.Add(new SqlParameter("@PageNumber", objQuery.PageNumber));
                    objcmd.Parameters.Add(new SqlParameter("@PageSize", objQuery.PageSize));
                    objcmd.Parameters.Add(new SqlParameter("@Filter", objQuery.Filter));
                    objcmd.Parameters.Add(new SqlParameter("@Sort", objQuery.Sort));
                    objcmd.Parameters.Add(new SqlParameter("@TotalRecords", SqlDbType.Int)).Direction = ParameterDirection.Output;
                    reader = objcmd.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            obj = new Menu();
                            obj.MenuId = Convert.ToInt32("0" + reader["MenuId"]);
                            obj.Title = Convert.ToString("" + reader["Title"]);
                            obj.MenuHeading = Convert.ToString("" + reader["MenuHeading"]);
                            obj.Description = Convert.ToString("" + reader["Description"]);
                            obj.IconClass = Convert.ToString("" + reader["IconClass"]);
                            obj.Controller = Convert.ToString("" + reader["Controller"]);
                            obj.Action = Convert.ToString("" + reader["Action"]);
                            obj.ParentId = Convert.ToInt32("0" + reader["ParentId"]);
                            obj.Order = Convert.ToInt32("0" + reader["Order"]);
                            obj.Status = Convert.ToInt32("0" + reader["Status"]);
                            obj.IsParent = reader["IsParent"] == DBNull.Value ? false : (Boolean)reader["IsParent"];
                            lstObject.Add(obj);
                        }
                    }
                }
                objQuery.TotalRecords = Convert.ToInt32(objcmd.Parameters["@TotalRecords"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                reader.Dispose();
                objcmd = null;
                con = null;
            }
            return lstObject;
        }


    }
}
