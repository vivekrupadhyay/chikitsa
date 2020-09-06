using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chikitsa.Entities;

namespace Chikitsa.DataAccessLayer
{
    public class CountryDAL
    {
        public List<CountryMaster> Get(QueryBO objQuery)
        {
            SqlDataReader reader = null;
            SqlConnection con = null;
            List<CountryMaster> lstCountry = null;
            SqlCommand objcmd = null;
            CountryMaster objCountry = null;
            try
            {
                lstCountry = new List<CountryMaster>();
                using (con = DBconnection.ConnectToDB())
                {
                    objcmd = new SqlCommand();
                    objcmd.Connection = con;
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.CommandText = "STP_SelectCountryMaster";
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
                            objCountry = new CountryMaster();

                            objCountry.CountryID = Convert.ToInt64(reader["CountryID"]);
                            objCountry.CountryName = Convert.ToString("" + reader["CountryName"]);
                            objCountry.Status = Convert.ToInt16("" + reader["Status"]);
                            objCountry.strStatus = Convert.ToString("" + reader["strStatus"]);
                            objCountry.Remark = Convert.ToString("" + reader["Remark"]);
                            objCountry.strStatus = Convert.ToString("" + reader["strStatus"]);
                            lstCountry.Add(objCountry);
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
            return lstCountry;
        }
        public int Save(CountryMaster country, string CRUDAction, out string ErrorCode)
        {
            int result = 0;
            ErrorCode = "-1";
            using (SqlCommand objcmd = new SqlCommand())
            {
                try
                {
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.CommandText = "STP_InsertUpdateCountryMaster";
                    objcmd.Parameters.Add(new SqlParameter("@CountryID", country.CountryID)).Direction = ParameterDirection.InputOutput;
                    objcmd.Parameters.Add(new SqlParameter("@CountryName", country.CountryName));
                    objcmd.Parameters.Add(new SqlParameter("@Status", country.Status));
                    objcmd.Parameters.Add(new SqlParameter("@remark", country.Remark));
                    objcmd.Parameters.Add(new SqlParameter("@CreatedBy", country.CreatedBy));
                    objcmd.Parameters.Add(new SqlParameter("@ModifiedBy", country.ModifiedBy));
                    objcmd.Parameters.Add(new SqlParameter("@CRUDAction", CRUDAction));
                    objcmd.Parameters.Add(new SqlParameter("@ErrorCode", SqlDbType.VarChar, 20)).Direction = ParameterDirection.Output;
                    result = DBconnection.ExecuteNonQuery(objcmd);
                    country.CountryID = Convert.ToInt64("0" + objcmd.Parameters["@CountryID"].Value);
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
