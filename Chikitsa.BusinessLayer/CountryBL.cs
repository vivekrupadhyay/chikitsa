using Chikitsa.DataAccessLayer;
using Chikitsa.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chikitsa.BusinessLayer
{
    public class CountryBL
    {
        public Response SaveCountry(CountryMaster country, string CRUDAction)
        {
            CountryDAL objCntDAL = null;
            Response objResponse;
            string ErrorCode = "";
            try
            {
                objCntDAL = new CountryDAL();
                objCntDAL.Save(country, CRUDAction, out ErrorCode);
                objResponse.ErrorCode = ErrorCode;
                objResponse.Description = "";
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                objCntDAL = null;
            }
            return objResponse;
        }

        public List<CountryMaster> GetData(CountryMasterFilter objFilter, bool prevFilter = false)
        {
            CountryDAL objCntDAL = null;
            try
            {
                objCntDAL = new CountryDAL();
                SetCountryFilter(objFilter, prevFilter);
                return objCntDAL.Get(objFilter);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                objCntDAL = null;
            }
        }

        public void SetCountryFilter(CountryMasterFilter objFilter, bool prevFilter)
        {
            if (!prevFilter)
                objFilter.Filter = "";
            if (objFilter.CountryID > 0)
                objFilter.Filter += " and CM.CountryID ='" + objFilter.CountryID + "'";
            if (!string.IsNullOrEmpty(objFilter.CountryName))
                objFilter.Filter += " and ( CM.CountryName like '%" + objFilter.CountryName + "%')";
            if (objFilter.Status != 0)
                objFilter.Filter += " and CM.Status ='" + objFilter.Status + "'";
        }
    }
}
