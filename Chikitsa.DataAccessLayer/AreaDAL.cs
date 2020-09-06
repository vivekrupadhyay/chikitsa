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
    public class AreaDAL
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

        public static List<CityMaster> GetCity(int countryID, int StateID)
        {
            string res = "";
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
                    objcmd.CommandText = "STP_GetCityDtls";
                    reader = objcmd.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            objCity = new CityMaster();
                            countryID = Convert.ToInt32(reader["countryID"]);
                            StateID = Convert.ToInt32(reader["StateID"]);
                            objCity.CityID = Convert.ToInt64(reader["CityID"]);
                            objCity.CityName = Convert.ToString("" + reader["StateName"]);
                            lstCity.Add(objCity);
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
            return lstCity;
        }
        public List<AreaMaster> Get(QueryBO objQuery)
        {
            SqlDataReader reader = null;
            SqlConnection con = null;
            List<AreaMaster> lstArea = null;
            SqlCommand objcmd = null;
            AreaMaster objArea = null;
            try
            {
                lstArea = new List<AreaMaster>();
                using (con = DBconnection.ConnectToDB())
                {
                    objcmd = new SqlCommand();
                    objcmd.Connection = con;
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.CommandText = "STP_SelectAreaMaster";
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
                            objArea = new AreaMaster();
                            objArea.AreaID = Convert.ToInt64(reader["AreaID"]);
                            objArea.CityID = Convert.ToInt32(reader["CityID"]);
                            objArea.StateID = Convert.ToInt32(reader["StateID"]);
                            objArea.AreaName = Convert.ToString("" + reader["AreaName"]);
                            objArea.CountryID = Convert.ToInt32(reader["CountryID"]);
                            objArea.Status = Convert.ToInt32("" + reader["Status"]);
                            objArea.Remarks = Convert.ToString("" + reader["Remarks"]);
                            objArea.strStatus = Convert.ToString("" + reader["strStatus"]);
                            lstArea.Add(objArea);
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
            return lstArea;
        }
        public int Save(AreaMaster area, string CRUDAction, out string ErrorCode)
        {
            int result = 0;
            ErrorCode = "-1";
            using (SqlCommand objcmd = new SqlCommand())
            {
                try
                {
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.CommandText = "STP_InsertUpdateAreaMaster";
                    objcmd.Parameters.Add(new SqlParameter("@AreaID", area.AreaID)).Direction = ParameterDirection.InputOutput;
                    objcmd.Parameters.Add(new SqlParameter("@AreaName", area.AreaName));
                    objcmd.Parameters.Add(new SqlParameter("@CityID", area.CityID));
                    objcmd.Parameters.Add(new SqlParameter("@CountryID", area.CountryID));
                    objcmd.Parameters.Add(new SqlParameter("@StateID", area.StateID));
                    objcmd.Parameters.Add(new SqlParameter("@Status", area.Status));
                    objcmd.Parameters.Add(new SqlParameter("@Remarks", area.Remarks));
                    objcmd.Parameters.Add(new SqlParameter("@CreatedBy", area.CreatedBy));
                    objcmd.Parameters.Add(new SqlParameter("@ModifiedBy", area.ModifiedBy));
                    objcmd.Parameters.Add(new SqlParameter("@CRUDAction", CRUDAction));
                    objcmd.Parameters.Add(new SqlParameter("@ErrorCode", SqlDbType.VarChar, 20)).Direction = ParameterDirection.Output;
                    result = DBconnection.ExecuteNonQuery(objcmd);
                    area.AreaID = Convert.ToInt64("0" + objcmd.Parameters["@AreaID"].Value);
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
