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
    public class CommonDAL
    {
        SqlDataReader reader = null;
        SqlConnection con = null;
        List<CodeDetail> lstObject = null;
        SqlCommand objcmd = null;
        CodeDetail obj = null;
        public List<CodeDetail> GetCodeDetail(QueryBO objQuery)
        {

            try
            {
                lstObject = new List<CodeDetail>();
                using (con = DBconnection.ConnectToDB())
                {
                    objcmd = new SqlCommand();
                    objcmd.Connection = con;
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.CommandText = "STP_SelectCodeDetail";
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
                            obj = new CodeDetail();
                            obj.CodeDetailId = Convert.ToInt64("0" + reader["CodeDetailId"]);
                            obj.CodeTypeId = Convert.ToInt32("0" + reader["CodeTypeId"]);
                            obj.DetailLongDesc = Convert.ToString("" + reader["DetailLongDesc"]);
                            obj.DetailShortDesc = Convert.ToString("" + reader["DetailShortDesc"]);
                            obj.ParentId = Convert.ToInt64("0" + reader["ParentId"]);
                            obj.Status = Convert.ToBoolean(reader["Status"]);
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

        public List<T> GetTable<T>(TableFilter objFilter)
        {
            List<object> lstObject = null;
            try
            {
                lstObject = new List<object>();
                using (con = DBconnection.ConnectToDB())
                {
                    objcmd = new SqlCommand();
                    objcmd.Connection = con;
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.CommandText = "STP_Select_BindDropdown";
                    objcmd.Parameters.Add(new SqlParameter("@Table", objFilter.TableName));
                    objcmd.Parameters.Add(new SqlParameter("@IDColumn", objFilter.IdColumn));
                    objcmd.Parameters.Add(new SqlParameter("@TextColumn", objFilter.TextColumn));
                    objcmd.Parameters.Add(new SqlParameter("@Condition", objFilter.Condition));
                    reader = objcmd.ExecuteReader();
                    if (reader != null && reader.HasRows)
                    {
                        switch (typeof(T).Name.ToLower())
                        {
                            case "countrymaster":
                                {
                                    while (reader.Read())
                                    {
                                        CountryMaster obj = new CountryMaster();
                                        obj.CountryID = Convert.ToInt64("0" + reader["CountryID"]);
                                        obj.CountryName = Convert.ToString("" + reader["CountryName"]);
                                        lstObject.Add(obj);
                                    }
                                    break;
                                }
                            case "statemaster":
                                {
                                    while (reader.Read())
                                    {
                                        StateMaster obj = new StateMaster();
                                        //obj.CountryId = Convert.ToInt32("0" + reader["CountryID"]);
                                        obj.StateID = Convert.ToInt64("0" + reader["StateId"]);
                                        obj.StateName = Convert.ToString("" + reader["StateName"]);
                                        lstObject.Add(obj);
                                    }
                                    break;
                                }
                            case "citymaster":
                                {
                                    while (reader.Read())
                                    {
                                        CityMaster obj = new CityMaster();
                                        obj.CityID = Convert.ToInt64("0" + reader["CityID"]);
                                        obj.CityName = Convert.ToString("" + reader["CityName"]);
                                        lstObject.Add(obj);
                                    }
                                    break;
                                }
                            case "areamaster":
                                {
                                    while (reader.Read())
                                    {
                                        AreaMaster obj = new AreaMaster();
                                        obj.AreaID = Convert.ToInt64("0" + reader["Area_ID"]);
                                        obj.AreaName = Convert.ToString("" + reader["Area_Name"]);
                                        lstObject.Add(obj);
                                    }
                                    break;
                                }
                        }

                    }
                }
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
            return lstObject.OfType<T>().ToList();
        }
        public List<CountryMaster> GetCountryList(TableFilter objQuery)
        {

            List<CountryMaster> lstObject = null;
            CountryMaster obj = null;
            try
            {
                lstObject = new List<CountryMaster>();
                using (con = DBconnection.ConnectToDB())
                {
                    objcmd = new SqlCommand();
                    objcmd.Connection = con;
                    reader = sdr(objcmd, objQuery);
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            obj = new CountryMaster();
                            obj.CountryID = Convert.ToInt64("0" + reader["CountryID"]);
                            obj.CountryName = Convert.ToString("" + reader["CountryName"]);
                            lstObject.Add(obj);
                        }
                    }
                }
                // objQuery.TotalRecords = Convert.ToInt32(objcmd.Parameters["@TotalRecords"].Value);
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
        public List<StateMaster> GetStateList(TableFilter objQuery)
        {
            List<StateMaster> lstObject = null;
            StateMaster obj = null;
            try
            {
                lstObject = new List<StateMaster>();
                using (con = DBconnection.ConnectToDB())
                {
                    objcmd = new SqlCommand();
                    objcmd.Connection = con;
                    reader = sdr(objcmd, objQuery);
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            obj = new StateMaster();
                            obj.StateID = Convert.ToInt64("0" + reader["StateID"]);
                            obj.StateName = Convert.ToString("" + reader["StateName"]);
                            lstObject.Add(obj);
                        }
                    }
                }
                // objQuery.TotalRecords = Convert.ToInt32(objcmd.Parameters["@TotalRecords"].Value);
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
        public List<CityMaster> GetCityList(TableFilter objQuery)
        {
            List<CityMaster> lstObject = null;
            CityMaster obj = null;
            try
            {
                lstObject = new List<CityMaster>();
                using (con = DBconnection.ConnectToDB())
                {
                    objcmd = new SqlCommand();
                    objcmd.Connection = con;
                    reader = sdr(objcmd, objQuery);
                    if (reader != null && reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            obj = new CityMaster();
                            obj.CityID = Convert.ToInt64("0" + reader["CityID"]);
                            obj.CityName = Convert.ToString("" + reader["CityName"]);
                            lstObject.Add(obj);
                        }
                    }
                }
                // objQuery.TotalRecords = Convert.ToInt32(objcmd.Parameters["@TotalRecords"].Value);
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
        public SqlDataReader sdr(SqlCommand objcmd, TableFilter objFilter)
        {
            objcmd.CommandType = CommandType.StoredProcedure;
            objcmd.CommandText = "STP_Select_BindDropdown";
            objcmd.Parameters.Add(new SqlParameter("@Table", objFilter.TableName));
            objcmd.Parameters.Add(new SqlParameter("@IDColumn", objFilter.IdColumn));
            objcmd.Parameters.Add(new SqlParameter("@TextColumn", objFilter.TextColumn));
            objcmd.Parameters.Add(new SqlParameter("@Condition", objFilter.Condition));
            SqlDataReader reader = objcmd.ExecuteReader();
            return reader;
        }
    }
}
