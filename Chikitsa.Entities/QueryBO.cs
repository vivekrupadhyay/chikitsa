using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chikitsa.Entities
{
    public class QueryBO
    {
        public int PageNumber { get; set; } = 1;
        public Nullable<int> PageSize { get; set; } = 10;
        public string Filter { get; set; } = "";
        public string Sort { get; set; } = "";
        public Int64 TotalRecords { get; set; }
        public PagingRoute PagingRoute { get; set; }
    }

    public struct PagingRoute
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ParameterName { get; set; }
    }

    public struct Response
    {
        public string ErrorCode { get; set; }
        public string Description { get; set; }
    }
}
