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
    public class StateDAL
    {
        public static List<CountryMaster> GetCountry()
        {
            string res = "";
            SqlDataReader reader = null;
            SqlConnection con = null;
            List<CountryMaster> lstCnt = null;
            SqlCommand objcmd = null;
            CountryMaster objCountry = null;
            try
            {
                lstCnt = new List<CountryMaster>();
                using (con = DBconnection.ConnectToDB())
                {
                    objcmd = new SqlCommand();
                    objcmd.Connection = con;
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.CommandText = "STP_GetCountryDtls";
                    reader = objcmd.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            objCountry = new CountryMaster();
                            objCountry.CountryID = Convert.ToInt64(reader["CountryID"]);
                            objCountry.CountryName = Convert.ToString("" + reader["CountryName"]);
                            lstCnt.Add(objCountry);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                reader.Dispose();
                objcmd = null;
                con = null;
            }
            return lstCnt;
        }
        public List<StateMaster> Get(QueryBO objQuery)
        {
            SqlDataReader reader = null;
            SqlConnection con = null;
            List<StateMaster> lstState = null;
            SqlCommand objcmd = null;
            StateMaster objState = null;
            try
            {
                lstState = new List<StateMaster>();
                using (con = DBconnection.ConnectToDB())
                {
                    objcmd = new SqlCommand();
                    objcmd.Connection = con;
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.CommandText = "STP_SelectStateMaster";
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
                            objState = new StateMaster();

                            objState.StateID = Convert.ToInt64(reader["StateID"]);
                            objState.StateName = Convert.ToString("" + reader["StateName"]);
                            objState.CountryId = Convert.ToInt32(reader["CountryID"]);
                            objState.Status = Convert.ToInt16("" + reader["Status"]);
                            objState.Remark = Convert.ToString("" + reader["Remark"]);
                            objState.strStatus = Convert.ToString("" + reader["strStatus"]);
                            lstState.Add(objState);
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
            return lstState;
        }
        public int Save(StateMaster state, string CRUDAction, out string ErrorCode)
        {
            int result = 0;
            ErrorCode = "-1";
            using (SqlCommand objcmd = new SqlCommand())
            {
                try
                {
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.CommandText = "STP_InsertUpdateStateMaster";
                    objcmd.Parameters.Add(new SqlParameter("@StateID", state.StateID)).Direction = ParameterDirection.InputOutput;
                    objcmd.Parameters.Add(new SqlParameter("@StateName", state.StateName));
                    objcmd.Parameters.Add(new SqlParameter("@CountryID", state.CountryId));
                    objcmd.Parameters.Add(new SqlParameter("@Status", state.Status));
                    objcmd.Parameters.Add(new SqlParameter("@remark", state.Remark));
                    objcmd.Parameters.Add(new SqlParameter("@CreatedBy", state.CreatedBy));//
                    objcmd.Parameters.Add(new SqlParameter("@ModifiedBy", state.ModifiedBy));
                    objcmd.Parameters.Add(new SqlParameter("@CRUDAction", CRUDAction));
                    objcmd.Parameters.Add(new SqlParameter("@ErrorCode", SqlDbType.VarChar, 20)).Direction = ParameterDirection.Output;
                    result = DBconnection.ExecuteNonQuery(objcmd);
                    state.StateID = Convert.ToInt64("0" + objcmd.Parameters["@StateID"].Value);
                    ErrorCode = Convert.ToString(objcmd.Parameters["@ErrorCode"].Value);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    objcmd.Dispose();
                }
            }
            return result;
        }
    }
}
