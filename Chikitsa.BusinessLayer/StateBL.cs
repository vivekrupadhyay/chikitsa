using Chikitsa.DataAccessLayer;
using Chikitsa.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chikitsa.BusinessLayer
{
    public class StateBL
    {
        public Response SaveState(StateMaster state, string CRUDAction)
        {
            StateDAL objStateDAL = null;
            Response objResponse;
            string ErrorCode = "";
            try
            {
                objStateDAL = new StateDAL();
                objStateDAL.Save(state, CRUDAction, out ErrorCode);
                objResponse.ErrorCode = ErrorCode;
                objResponse.Description = "";
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                objStateDAL = null;
            }
            return objResponse;
        }

        public List<StateMaster> GetData(StateMasterFilter objFilter, bool prevFilter = false)
        {
            StateDAL objStateDAL = null;
            try
            {
                objStateDAL = new StateDAL();
                SetStateFilter(objFilter, prevFilter);
                return objStateDAL.Get(objFilter);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                objStateDAL = null;
            }
        }

        public void SetStateFilter(StateMasterFilter objFilter, bool prevFilter)
        {
            if (!prevFilter)
                objFilter.Filter = "";
            if (objFilter.StateID > 0)
                objFilter.Filter += " and SM.StateID ='" + objFilter.StateID + "'";
            if (!string.IsNullOrEmpty(objFilter.StateName))
                objFilter.Filter += " and ( SM.StateName like '%" + objFilter.StateName + "%')";
            if (objFilter.Status != 0)
                objFilter.Filter += " and SM.Status ='" + objFilter.Status + "'";
        }
    }
}
