using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chikitsa.Entities
{
    public class CityMaster
    {
        public CityMaster()
        {
        }
        public CityMaster(Int64 CityID)
        {
            this.CityID = CityID;
            CityName = string.Empty;
        }
        public Int64 CityID { get; set; }
        [Required]
        public int CountryID { get; set; }
        [Required]
        public int StateID { get; set; }
        [Required]
        public string CityName { get; set; }

        public short? Status { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string Remark { get; set; }
        public string strStatus { get; set; }

    }
    public class CityDetailsVM : Layout
    {

        public CityDetailsVM()
        {
            CityMstr = new CityMaster();
            StateMst = new List<StateMaster>();
            CntMst = new List<CountryMaster>();
        }
        public CityMaster CityMstr { get; set; }
        public List<CountryMaster> CntMst { get; set; }
        public List<StateMaster> StateMst { get; set; }

    }
    public class CityMasterListVM : Layout
    {
        public List<CityMaster> lstCityMaster { get; set; }
        public CityMasterFilter objFilter { get; set; }
       // public string CityName { get; set; }

    }
    public class CityMasterFilter : QueryBO
    {
        public Int64 CityID { get; set; }
        public int CountryID { get; set; }
        public int StateID { get; set; }
        public string CityName { get; set; }
        public int Status { get; set; }
        //public string Remark { get; set; }

    }

}
