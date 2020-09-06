using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chikitsa.Entities
{
    public class TableFilter
    {
        public string TableName { get; set; }
        public string IdColumn { get; set; }
        public string TextColumn { get; set; }
        public string Condition { get; set; }

    }
}
