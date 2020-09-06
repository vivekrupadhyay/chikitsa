using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chikitsa.Entities
{
    public class CountryMaster
    {
        public CountryMaster()
        {
        }
        public CountryMaster(Int64 CountryID)
        {
            this.CountryID = CountryID;
            CountryName = string.Empty;
        }
        public long CountryID { get; set; }
        [Required]
        public string CountryName { get; set; }
        public short? Status { get; set; }
        public string Remark { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string strStatus { get; set; }
    }
    public class CountryDetailsVM : Layout
    {

        public CountryDetailsVM() { CntMstr = new CountryMaster(); }
        public CountryMaster CntMstr { get; set; }

    }
    public class CountryMasterListVM : Layout
    {
        public List<CountryMaster> lstCountryMaster { get; set; }
        public CountryMasterFilter objFilter { get; set; }
        public List<CodeDetail> lstUserTypes { get; set; }
    }
    public class CountryMasterFilter : QueryBO
    {
        public Int64 CountryID { get; set; }
        public string CountryName { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public string UserType { get; set; }
    }
}
