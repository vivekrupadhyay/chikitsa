using Chikitsa.DataAccessLayer;
using Chikitsa.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chikitsa.BusinessLayer
{
    public class CommonBL
    {
        public List<CodeDetail> GetCodeDetail(CodeDetailFilter objFilter)
        {
            CommonDAL objRep = null;
            try
            {
                objRep = new CommonDAL();
                SetDetailFilter(objFilter);
                return objRep.GetCodeDetail(objFilter);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                objRep = null;
            }
        }

        public void SetDetailFilter(CodeDetailFilter objFilter)
        {
            objFilter.Filter = "";
            if (objFilter.CodeTypeId > 0)
                objFilter.Filter += " and CT.CodeTypeId ='" + objFilter.CodeTypeId + "'";
            else if (!string.IsNullOrEmpty(objFilter.CodeTypeIds))
                objFilter.Filter += " and CT.CodeTypeId in (" + objFilter.CodeTypeIds + ")";

        }
        public List<T> GetTable<T>(TableFilter objTableFilter)
        {
            CommonDAL objRep = null;
            try
            {
                objRep = new CommonDAL();
                return objRep.GetTable<T>(objTableFilter);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                objRep = null;
            }
        }
        public List<CountryMaster> GetCountry(TableFilter objQueryBO)
        {
            CommonDAL objRep = null;
            try
            {
                objRep = new CommonDAL();
                return objRep.GetCountryList(objQueryBO);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                objRep = null;
            }
        }
        public List<StateMaster> GetState(TableFilter objQueryBO)
        {
            CommonDAL objRep = null;
            try
            {
                objRep = new CommonDAL();
                return objRep.GetStateList(objQueryBO);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                objRep = null;
            }
        }
    }

}
