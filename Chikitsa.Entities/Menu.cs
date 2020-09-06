using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chikitsa.Entities
{
    public class Menu
    {
        public int MenuId { get; set; }
        public string Title { get; set; }
        public string MenuHeading { get; set; }
        public string Description { get; set; }
        public string IconClass { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        //public string Url { get; set; }
        public int ParentId { get; set; }
        public int Order { get; set; }
        public int Status { get; set; }
        public bool IsParent { get; set; }
        public string ActiveClass { get; set; }
    }

    public class MenuFilter : QueryBO
    {

    }
}
