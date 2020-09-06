using Chikitsa.Filters;
using System.Web.Mvc;

namespace Chikitsa
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new SessionAuthorizeAttribute());
            filters.Add(new AuthorizeAttribute());
            filters.Add(new LayoutFilter());
        }
    }
}