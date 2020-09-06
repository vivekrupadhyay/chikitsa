using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chikitsa.Entities
{
    public class CodeDetail
    {
        public Int64 CodeDetailId { get; set; }

        public int CodeTypeId { get; set; }

        public string DetailShortDesc { get; set; }

        public string DetailLongDesc { get; set; }

        public Int64? ParentId { get; set; }

        public bool Status { get; set; }
    }

    public class CodeDetailFilter : QueryBO
    {
        public int CodeTypeId { get; set; }
        public string CodeTypeIds { get; set; }
    }
}
