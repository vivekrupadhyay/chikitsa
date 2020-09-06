using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chikitsa.Entities
{
    public class StateMaster
    {
        public StateMaster()
        {
        }
        public StateMaster(Int64 StateID)
        {
            this.StateID = StateID;
            StateName = string.Empty;
        }
        public long StateID { get; set; }
        [Required]
        public string StateName { get; set; }
        [Required]
        public int CountryId { get; set; }
        public short? Status { get; set; }
        public string Remark { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string strStatus { get; set; }
        
    }
    public class StateDetailsVM : Layout
    {

        public StateDetailsVM()
        {
            StateMstr = new StateMaster();
            CntMst = new List<CountryMaster>();
           

        }
        public StateMaster StateMstr { get; set; }
        public List<CountryMaster> CntMst { get; set; }

    }
    public class StateMasterListVM : Layout
    {
        public List<StateMaster> lstStateMaster { get; set; }
        public StateMasterFilter objFilter { get; set; }
        public List<CountryMaster> CntMst { get; set; }
    }
    public class StateMasterFilter : QueryBO
    {
        public Int64 StateID { get; set; }
        public string StateName { get; set; }
        public Int64 CountryID { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }

    }
}
