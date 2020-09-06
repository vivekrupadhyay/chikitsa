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
    public class UserDAL
    {
        public List<User> Get(QueryBO objQuery)
        {
            SqlDataReader reader = null;
            SqlConnection con = null;
            List<User> lstUsers = null;
            SqlCommand objcmd = null;
            User objUser = null;
            try
            {
                lstUsers = new List<User>();
                using (con = DBconnection.ConnectToDB())
                {
                    objcmd = new SqlCommand();
                    objcmd.Connection = con;
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.CommandText = "STP_SelectUserMaster";
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
                            objUser = new User();

                            objUser.UserId = Convert.ToInt64(reader["UserId"]);
                            objUser.FirstName = Convert.ToString("" + reader["FirstName"]);
                            objUser.LastName = Convert.ToString("" + reader["LastName"]);
                            objUser.FullName = Convert.ToString("" + reader["FullName"]);
                            objUser.Mobile = Convert.ToString("" + reader["Mobile"]);
                            objUser.Email = Convert.ToString("" + reader["Email"]);
                            objUser.Password = Convert.ToString("" + reader["Password"]);
                            objUser.WorkingSince = reader["WorkingSince"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["WorkingSince"];
                            objUser.WorkingSinceWithUs = reader["WorkingSinceWithUs"] == DBNull.Value ? (DateTime?)null : (DateTime)reader["WorkingSinceWithUs"];
                            //Convert.ToDateTime(reader["WorkingSince"]);
                            //objUser.WorkingSinceWithUs = Convert.ToDateTime(reader["WorkingSinceWithUs"]);
                            objUser.Status = Convert.ToInt32("0" + reader["Status"]);
                            if (!string.IsNullOrEmpty(Convert.ToString(reader["IsActiveOnSite"])))
                                objUser.IsActiveOnSite = Convert.ToBoolean(reader["IsActiveOnSite"]);
                            objUser.UserType = Convert.ToInt16("0" + reader["UserType"]);
                            objUser.strStatus = Convert.ToString("" + reader["strStatus"]);
                            objUser.strUserType = Convert.ToString("" + reader["strUserType"]);
                            objUser.ImageUrl = Convert.ToString("" + reader["ImageUrl"]);
                            objUser.CompanyId = Convert.ToInt64("0" + reader["CompanyId"]);
                            lstUsers.Add(objUser);
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
            return lstUsers;
        }

        public int Save(User user, string CRUDAction, out string ErrorCode)
        {
            int result = 0;
            ErrorCode = "-1";
            using (SqlCommand objcmd = new SqlCommand())
            {
                try
                {
                    objcmd.CommandType = CommandType.StoredProcedure;
                    objcmd.CommandText = "STP_InsertUpdateUserMaster";
                    objcmd.Parameters.Add(new SqlParameter("@UserId", user.UserId)).Direction = ParameterDirection.InputOutput;
                    objcmd.Parameters.Add(new SqlParameter("@FirstName", user.FirstName));
                    objcmd.Parameters.Add(new SqlParameter("@LastName", user.LastName));
                    objcmd.Parameters.Add(new SqlParameter("@Mobile", user.Mobile));
                    objcmd.Parameters.Add(new SqlParameter("@Email", user.Email));
                    objcmd.Parameters.Add(new SqlParameter("@Password", user.Password));
                    objcmd.Parameters.Add(new SqlParameter("@ImageUrl", user.ImageUrl));
                    objcmd.Parameters.Add(new SqlParameter("@WorkingSince", user.WorkingSince));
                    objcmd.Parameters.Add(new SqlParameter("@WorkingSinceWithUs", user.WorkingSinceWithUs));
                    objcmd.Parameters.Add(new SqlParameter("@Status", user.Status));
                    objcmd.Parameters.Add(new SqlParameter("@IsActiveOnSite", user.IsActiveOnSite));
                    objcmd.Parameters.Add(new SqlParameter("@UserType", user.UserType));
                    objcmd.Parameters.Add(new SqlParameter("@CompanyID", user.CompanyId));
                    objcmd.Parameters.Add(new SqlParameter("@CreatedBy", user.CreatedBy));
                    objcmd.Parameters.Add(new SqlParameter("@ModifiedBy", user.ModifiedBy));
                    objcmd.Parameters.Add(new SqlParameter("@CRUDAction", CRUDAction));
                    objcmd.Parameters.Add(new SqlParameter("@ErrorCode", SqlDbType.VarChar, 20)).Direction = ParameterDirection.Output;
                    result = DBconnection.ExecuteNonQuery(objcmd);
                    user.UserId = Convert.ToInt64("0" + objcmd.Parameters["@UserId"].Value);
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
