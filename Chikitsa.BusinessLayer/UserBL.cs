using Chikitsa.DataAccessLayer;
using Chikitsa.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chikitsa.BusinessLayer
{
    public class UserBL
    {
        public Response SaveUser(User user, string CRUDAction)
        {
            UserDAL objUserDAL = null;
            Response objResponse;
            string ErrorCode = "";
            try
            {
                objUserDAL = new UserDAL();
                objUserDAL.Save(user, CRUDAction, out ErrorCode);
                objResponse.ErrorCode = ErrorCode;
                objResponse.Description = "";
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                objUserDAL = null;
            }
            return objResponse;
        }

        public List<User> GetData(UserFilter objFilter, bool prevFilter = false)
        {
            UserDAL objUserDAL = null;
            try
            {
                objUserDAL = new UserDAL();
                SetUserFilter(objFilter, prevFilter);
                return objUserDAL.Get(objFilter);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                objUserDAL = null;
            }
        }

        public void SetUserFilter(UserFilter objFilter, bool prevFilter)
        {
            if (!prevFilter)
                objFilter.Filter = "";
            if (objFilter.UserId > 0)
                objFilter.Filter += " and UM.UserId ='" + objFilter.UserId + "'";
            if (!string.IsNullOrEmpty(objFilter.Name))
                objFilter.Filter += " and ( UM.FirstName like '%" + objFilter.Name + "%' or UM.LastName like '%" + objFilter.Name + "%')";
            if (!string.IsNullOrEmpty(objFilter.Email))
                objFilter.Filter += " and UM.Email like '%" + objFilter.Email + "%'";
            if (!string.IsNullOrEmpty(objFilter.Mobile))
                objFilter.Filter += " and UM.Mobile like '%" + objFilter.Mobile + "%'";
            if (!string.IsNullOrEmpty(objFilter.UserType))
                objFilter.Filter += " and UM.UserType =" + objFilter.UserType + "";
            if (objFilter.Status != 0)
                objFilter.Filter += " and UM.Status ='" + objFilter.Status + "'";
        }

    }
}
