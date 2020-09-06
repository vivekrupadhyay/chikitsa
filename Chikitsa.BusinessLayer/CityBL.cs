using Chikitsa.DataAccessLayer;
using Chikitsa.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chikitsa.BusinessLayer
{
  public class CityBL
    {
        public Response SaveState(CityMaster city, string CRUDAction)
        {
            CityDAL objCityDAL = null;
            Response objResponse;
            string ErrorCode = "";
            try
            {
                objCityDAL = new CityDAL();
                objCityDAL.Save(city, CRUDAction, out ErrorCode);
                objResponse.ErrorCode = ErrorCode;
                objResponse.Description = "";
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                objCityDAL = null;
            }
            return objResponse;
        }

        public List<CityMaster> GetData(CityMasterFilter objFilter, bool prevFilter = false)
        {
            CityDAL objCityDAL = null;
            try
            {
                objCityDAL = new CityDAL();
                SetStateFilter(objFilter, prevFilter);
                return objCityDAL.Get(objFilter);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                objCityDAL = null;
            }
        }

        public void SetStateFilter(CityMasterFilter objFilter, bool prevFilter)
        {
            if (!prevFilter)
                objFilter.Filter = "";
            if (objFilter.StateID > 0)
                objFilter.Filter += " and CM.CityID ='" + objFilter.CityID + "'";
            if (!string.IsNullOrEmpty(objFilter.CityName))
                objFilter.Filter += " and ( CM.CityName like '%" + objFilter.CityName + "%')";
            if (objFilter.Status != 0)
                objFilter.Filter += " and CM.Status ='" + objFilter.Status + "'";
        }
    }
}
