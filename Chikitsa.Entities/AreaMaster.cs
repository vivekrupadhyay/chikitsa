using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chikitsa.Entities
{
    public class AreaMaster
    {
        public AreaMaster()
        {
        }
        public AreaMaster(Int64 AreaID)
        {
            this.AreaID = AreaID;
            AreaName = string.Empty;
            CountryName = string.Empty;
            StateName = string.Empty;
            CityName = string.Empty;
        }
        public Int64 AreaID { get; set; }
        [Required]
        public string AreaName { get; set; }
        [Required]
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        [Required]
        public int StateID { get; set; }
        [Required]
        public int CityID { get; set; }

        public int Status { get; set; }

        public string Remarks { get; set; }
        public string strStatus { get; set; }
        public int? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

    }
    public class AreaDetailsVM : Layout
    {

        public AreaDetailsVM()
        {
            AreaMstr = new AreaMaster();
            CntMst = new List<CountryMaster>();
            StateMst = new List<StateMaster>();
            CityMstr = new List<CityMaster>();
        }
        public AreaMaster AreaMstr { get; set; }
        public List<CountryMaster> CntMst { get; set; }
        public List<StateMaster> StateMst { get; set; }
        public List<CityMaster> CityMstr { get; set; }
    }
    public class AreaMasterListVM : Layout
    {
        public List<AreaMaster> lstAreaMaster { get; set; }
        public AreaMasterFilter objFilter { get; set; }

    }
    public class AreaMasterFilter : QueryBO
    {
        public Int64 AreaID { get; set; }

        public int CountryID { get; set; }
        public int StateID { get; set; }
        public string AreaName { get; set; }
        public int CityID { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }

    }
}
