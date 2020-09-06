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
   public class CityDAL
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

        public static List<StateMaster> GetState(int countryID)
        {
            string res = "";
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
                    objcmd.CommandText = "STP_GetStateDtls";
                    reader = objcmd.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            objState = new StateMaster();
                            countryID = Convert.ToInt32(reader["countryID"]);
                            objState.StateID = Convert.ToInt64(reader["StateID"]);
                            objState.StateName = Convert.ToString("" + reader["StateName"]);
                            lstState.Add(objState);
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
            return lstState;
        }
        public List<CityMaster> Get(QueryBO objQuery)
        {
            SqlDataReader reader = null;
            SqlConnection con = null;
            List<CityMaster> lstCity = null;
            SqlCommand objcmd = null;
            CityMaster objCity = null;
            try
            {
                lstCity = new List<CityMaster>();
                using (con = DBconnection.ConnectToDB())
                {
                    objcmd = new SqlCommand();
                    objcmd.Connection = con;
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.CommandText = "STP_SelectCityMaster";
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
                            objCity = new CityMaster();
                            objCity.CityID= Convert.ToInt64(reader["CityID"]);
                            objCity.StateID = Convert.ToInt32(reader["StateID"]);
                            objCity.CityName = Convert.ToString("" + reader["CityName"]);
                            objCity.CountryID = Convert.ToInt32(reader["CountryID"]);
                            objCity.Status = Convert.ToInt16("" + reader["Status"]);
                            objCity.Remark = Convert.ToString("" + reader["Remark"]);
                           objCity.strStatus = Convert.ToString("" + reader["strStatus"]);

                            lstCity.Add(objCity);
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
            return lstCity;
        }
        public int Save(CityMaster city, string CRUDAction, out string ErrorCode)
        {
            int result = 0;
            ErrorCode = "-1";
            using (SqlCommand objcmd = new SqlCommand())
            {
                try
                {
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.CommandText = "STP_InsertUpdateCityMaster";
                    objcmd.Parameters.Add(new SqlParameter("@CityID", city.CityID)).Direction = ParameterDirection.InputOutput;
                    objcmd.Parameters.Add(new SqlParameter("@CityName", city.CityName));
                    objcmd.Parameters.Add(new SqlParameter("@CountryID", city.CountryID));
                    objcmd.Parameters.Add(new SqlParameter("@StateID", city.StateID));
                    objcmd.Parameters.Add(new SqlParameter("@Status", city.Status));
                    objcmd.Parameters.Add(new SqlParameter("@Remark", city.Remark));
                    objcmd.Parameters.Add(new SqlParameter("@CreatedBy", city.CreatedBy));
                    objcmd.Parameters.Add(new SqlParameter("@ModifiedBy", city.ModifiedBy));
                    objcmd.Parameters.Add(new SqlParameter("@CRUDAction", CRUDAction));
                    objcmd.Parameters.Add(new SqlParameter("@ErrorCode", SqlDbType.VarChar, 20)).Direction = ParameterDirection.Output;
                    result = DBconnection.ExecuteNonQuery(objcmd);
                    city.CityID = Convert.ToInt64("0" + objcmd.Parameters["@CityID"].Value);
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
