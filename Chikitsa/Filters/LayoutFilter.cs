
using Chikitsa.BusinessLayer;
using Chikitsa.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chikitsa.Filters
{
    public class LayoutFilter : ActionFilterAttribute, IExceptionFilter
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewResult v = filterContext.Result as ViewResult;
            if (v != null) // v will null when v is not a ViewResult
            {
                Layout bvm = v.Model as Layout;
                if (bvm != null)//bvm will be null when we want a view without Header and footer
                {
                    List<Menu> lstMenu = null;
                    MenuBL objMenuBL = null;
                    List<BreadCrumb> lstBreadCrumb = null;
                    try
                    {
                        objMenuBL = new MenuBL();
                        lstBreadCrumb = new List<BreadCrumb>();
                        lstMenu = objMenuBL.Get(new MenuFilter() { PageSize = null });
                        //string action = filterContext.RouteData.Values["action"].ToString();
                        string action = v.ViewName;
                        if (string.IsNullOrEmpty(action))
                            action = filterContext.ActionDescriptor.ActionName;
                        //string controller = filterContext.RouteData.Values["controller"].ToString();
                        string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

                        foreach (Menu item in lstMenu)
                        {
                            if (item.Controller == controller && item.Action == action)
                            {
                                item.ActiveClass = " active";
                                Menu objParentMenu = null;
                                int parentId = item.ParentId;
                                lstBreadCrumb.Add(new BreadCrumb() { Name = item.Title, Controller = item.Controller, Action = item.Action, IconClass = item.IconClass });
                                if (item.MenuId != 1)
                                {
                                    do
                                    {
                                        objParentMenu = lstMenu.FirstOrDefault(r => r.MenuId == parentId);
                                        lstBreadCrumb.Add(new BreadCrumb() { Name = objParentMenu.Title, Controller = Convert.ToString("" + objParentMenu.Controller), Action = Convert.ToString("" + objParentMenu.Action), IconClass = objParentMenu.IconClass });
                                        parentId = objParentMenu.ParentId;
                                    }
                                    while (parentId != 0);
                                    objParentMenu = lstMenu.FirstOrDefault(r => r.MenuId == 1);
                                    lstBreadCrumb.Add(new BreadCrumb() { Name = objParentMenu.Title, Controller = Convert.ToString("" + objParentMenu.Controller), Action = Convert.ToString("" + objParentMenu.Action), IconClass = objParentMenu.IconClass });
                                }
                                bvm.PageDescription = Convert.ToString("" + item.Description);
                                bvm.PageTitle = Convert.ToString("" + item.MenuHeading);
                                break;
                            }
                        }
                        bvm.lstMenu = lstMenu;
                        bvm.lstBreadCrumb = lstBreadCrumb;
                        bvm.UserName = HttpContext.Current.User.Identity.Name;
                        bvm.CompanyName = "KD";
                        bvm.Year = DateTime.Now.Year.ToString();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                    finally
                    {
                        lstMenu = null;
                        objMenuBL = null;
                        lstBreadCrumb = null;
                    }
                }
            }
        }

        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            if (new HttpException(null, filterContext.Exception).GetHttpCode() != 500)
            {
                return;
            }

            //if (!ExceptionType.IsInstanceOfType(filterContext.Exception))
            //{
            //    return;
            //}

            // if the request is AJAX return JSON else view.
            if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        error = true,
                        message = filterContext.Exception.Message
                    }
                };
            }
            else
            {
                var controllerName = (string)filterContext.RouteData.Values["controller"];
                var actionName = (string)filterContext.RouteData.Values["action"];
                var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

                //filterContext.Result = new ViewResult
                //{
                //    ViewName = filterContext,
                //    MasterName = Master,
                //    ViewData = new ViewDataDictionary(model),
                //    TempData = filterContext.Controller.TempData
                //};
            }

            // log the error by using your own method
            //LogError(filterContext.Exception.Message, filterContext.Exception);
            throw filterContext.Exception;
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }

    public class SessionAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.Session["User"] != null;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //filterContext.Result = new RedirectResult("~/User/Login");
        }
    }
}