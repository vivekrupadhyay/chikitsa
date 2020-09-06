using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chikitsa.Entities
{
    public class Layout
    {
        public List<Menu> lstMenu { get; set; }
        public string UserName { get; set; }
        public string CompanyName { get; set; }
        public string Year { get; set; }
        public List<BreadCrumb> lstBreadCrumb { get; set; }
        public string PageDescription = "";
        public string PageTitle = "";
        public ToastNotification Toast { get; set; }
        public QueryBO QueryBO { get; set; }
    }

    public class BreadCrumb
    {
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string IconClass { get; set; }
    }

    public class ToastNotification
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public ToastType toastType { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public int RedirectDuration { get; set; } = 3000;
    }

    public enum ToastType
    {
        success, warning, info, error
    }

    public class HomeViewModel : Layout
    {

    }
}
