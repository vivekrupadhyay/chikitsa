using Chikitsa.DataAccessLayer;
using Chikitsa.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chikitsa.BusinessLayer
{
   public class AreaBL
    {
        public Response SaveArea(AreaMaster area, string CRUDAction)
        {
            AreaDAL objAreaDAL = null;
            Response objResponse;
            string ErrorCode = "";
            try
            {
                objAreaDAL = new AreaDAL();
                //area.CreatedBy = 1;
                //area.ModifiedBy = 1;
                objAreaDAL.Save(area, CRUDAction, out ErrorCode);
                objResponse.ErrorCode = ErrorCode;
                objResponse.Description = "";
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                objAreaDAL = null;
            }
            return objResponse;
        }

        public List<AreaMaster> GetData(AreaMasterFilter objFilter, bool prevFilter = false)
        {
            AreaDAL objAreaDAL = null;
            try
            {
                objAreaDAL = new AreaDAL();
                SetStateFilter(objFilter, prevFilter);
                return objAreaDAL.Get(objFilter);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                objAreaDAL = null;
            }
        }

        public void SetStateFilter(AreaMasterFilter objFilter, bool prevFilter)
        {
            if (!prevFilter)
                objFilter.Filter = "";
            if (objFilter.AreaID > 0)
                objFilter.Filter += " and am.AreaID ='" + objFilter.AreaID + "'";
            if (!string.IsNullOrEmpty(objFilter.AreaName))
                objFilter.Filter += " and ( am.AreaName like '%" + objFilter.AreaName + "%')";
            if (objFilter.Status != 0)
                objFilter.Filter += " and am.Status ='" + objFilter.Status + "'";
        }
    }
}
