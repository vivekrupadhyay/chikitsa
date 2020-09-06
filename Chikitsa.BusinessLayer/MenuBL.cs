using Chikitsa.DataAccessLayer;
using Chikitsa.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chikitsa.BusinessLayer
{
    public class MenuBL
    {
        public List<Menu> Get(MenuFilter objFilter)
        {
            MenuDAL objDAL = null;
            try
            {
                objDAL = new MenuDAL();
                //SetAttendenceFilter(objFilter);
                return objDAL.Get(objFilter);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                objDAL = null;
            }
        }

    }
}
