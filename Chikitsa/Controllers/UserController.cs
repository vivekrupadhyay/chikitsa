using Chikitsa.BusinessLayer;
using Chikitsa.Entities;
using Chikitsa.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace Chikitsa.Controllers
{
    public class UserController : Controller
    {
        Response objResponse;
        UserBL objUserBL = null;
        UserDetailsVM objDetailsVM = null;
        UserListViewModel objListVM = null;

        private UserListViewModel GetUserListVM(UserFilter objFilter)
        {
            //Thread.Sleep(1000);
            UserListViewModel userListVM = null;
            CommonBL objCommonBL = null;
            try
            {
                objUserBL = new UserBL();
                userListVM = new UserListViewModel();
                objCommonBL = new CommonBL();
                objFilter.PagingRoute = new PagingRoute() { Action = "Index", Controller = "User" };
                userListVM.lstUsers = objUserBL.GetData(objFilter);
                userListVM.lstUserTypes = objCommonBL.GetCodeDetail(new CodeDetailFilter() { CodeTypeId = 1 });
                userListVM.QueryBO = userListVM.objFilter = objFilter;
                return userListVM;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                userListVM = null;
            }

        }

        public ActionResult Index(UserFilter objFilter, int? pageNumber)
        {

            UserListViewModel empVM = null;
            try
            {
                objUserBL = new UserBL();
                if (TempData["UserFilter"] != null)
                {
                    objFilter = (UserFilter)TempData["UserFilter"];
                    objFilter.UserId = 0;
                    if (pageNumber != null)
                    {
                        if (pageNumber != 0)
                            objFilter.PageNumber = pageNumber ?? objFilter.PageNumber;
                        TempData.Keep("UserFilter");
                    }
                }
                empVM = GetUserListVM(objFilter);
                return View("Index", empVM);
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                empVM = null;
                objUserBL = null;
            }
        }

        // GET: User
        public ActionResult Details()
        {
            CommonBL objCommonBL = null;
            try
            {

                objDetailsVM = new UserDetailsVM();
                objCommonBL = new CommonBL();
                objDetailsVM.User.IsActiveOnSite = true;
                objDetailsVM.lstUserTypes = objCommonBL.GetCodeDetail(new CodeDetailFilter() { CodeTypeId = 1 });
                return View(objDetailsVM);
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                objCommonBL = null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Action(UserFilter objFilter, string submit)
        {
            CommonBL objCommonBL = null;
            User objUser = null;
            try
            {
                objDetailsVM = new UserDetailsVM();
                if (submit.ToLower() == "edit")
                {
                    objCommonBL = new CommonBL();
                    TempData["UserFilter"] = objFilter;
                    objUserBL = new UserBL();
                    objDetailsVM = new UserDetailsVM();
                    objDetailsVM.User = objUserBL.GetData(new UserFilter() { UserId = objFilter.UserId }).FirstOrDefault();
                    objDetailsVM.lstUserTypes = objCommonBL.GetCodeDetail(new CodeDetailFilter() { CodeTypeId = 1 });
                    return View("Details", objDetailsVM);
                }
                else if (submit.ToLower() == "delete")
                {
                    objUserBL = new UserBL();
                    objListVM = new UserListViewModel();
                    objUser = new User(objFilter.UserId);
                    objUser.ModifiedBy = 1;
                    objResponse = objUserBL.SaveUser(objUser, "D");
                    objFilter.UserId = 0;
                    objListVM = GetUserListVM(objFilter);
                    objListVM.Toast = WebCommon.SetToast(objResponse);
                    return View("Index", objListVM);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                objUserBL = null;
                objDetailsVM = null;
                objListVM = null;
                objCommonBL = null;
            }
            return null;
        }

        [HttpPost]
        [ActionName("Details")]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsPost(UserDetailsVM objDetailsVM, string Submit, HttpPostedFileBase file)
        {
            CommonBL objCommonBL = null;
            try
            {
                if (Submit == "Save")
                {
                    objUserBL = new UserBL();
                    objCommonBL = new CommonBL();

                    objDetailsVM.User.Status = 1;
                    if (file != null)
                    {
                        string path = "~/Content/profileimages/" + Guid.NewGuid() + file.FileName;
                        file.SaveAs(Server.MapPath(path));
                        objDetailsVM.User.ImageUrl = path;//.Substring(2, path.Length - 2);
                    }
                    objResponse = objUserBL.SaveUser(objDetailsVM.User, objDetailsVM.User.UserId > 0 ? "U" : "C");
                    objDetailsVM.lstUserTypes = objCommonBL.GetCodeDetail(new CodeDetailFilter() { CodeTypeId = 1 });
                    objDetailsVM.Toast = WebCommon.SetToast(objResponse, "User", "Index");
                }
                return View(objDetailsVM);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                objUserBL = null;
                objDetailsVM = null;
                objCommonBL = null;
            }
        }

        [HttpPost]
        public ActionResult Filter(UserFilter objFilter, string submit)
        {
            try
            {
                if (submit.ToLower() == "filter")
                {
                    objFilter.PageNumber = 1;
                    //objFilter.Sort = "";
                    TempData["UserFilter"] = objFilter;
                }
                else
                {
                    TempData["UserFilter"] = null;
                    objFilter = new UserFilter();
                }
                return RedirectToAction("Index", new { pageNumber = objFilter.PageNumber });
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
            }
        }

        [HttpPost]
        public ActionResult ListFilter(UserFilter objFilter, string submit = "")
        {
            try
            {

                if (submit.ToLower() == "filter")
                {
                    objFilter.PageNumber = 1;
                    //objFilter.Sort = "";
                    TempData["UserFilter"] = objFilter;
                }
                else if (submit.ToLower() == "clear")
                {
                    TempData["UserFilter"] = null;
                    objFilter = new UserFilter();
                }
                return View("_ListUser", GetUserListVM(objFilter));
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
            }
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Login")]
        [AllowAnonymous]
        public ActionResult LoginPost(string userName, string passWord, string ReturnUrl, bool remember = false)
        {
            User objUser = null;
            try
            {
                ViewBag.ReturnUrl = ReturnUrl;
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(passWord))
                {
                    objUserBL = new UserBL();
                    objUser = objUserBL.GetData(new UserFilter() { Filter = " and (Email = '" + userName + "' or  Mobile = '" + userName + "') and Password = '" + passWord + "'" }, true).FirstOrDefault();
                    if (objUser != null)
                    {
                        Session["User"] = objUser;
                        Session["UserId"] = objUser.UserId;
                        //Session["FullName"] = objUser.FullName;
                        //Session["Email"] = objUser.Email;
                        //Session["Mobile"] = objUser.Mobile;
                        FormsAuthentication.SetAuthCookie(objUser.Email, remember);
                        ReturnUrl = Server.UrlDecode(ReturnUrl);
                        if (Url.IsLocalUrl(ReturnUrl) && ReturnUrl.Length > 1 && ReturnUrl.StartsWith("/")
                    && !ReturnUrl.StartsWith("//") && !ReturnUrl.StartsWith("/\\"))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                        ViewBag.LoginError = "Invalid Email/Mobile or Password";
                }
                else
                    ViewBag.LoginError = "Please enter Email/Mobile and Password";
                return View();
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                objUserBL = null;
                objUser = null;
            }

        }

        public ActionResult Logout()
        {
            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
            TempData.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }

        [AllowAnonymous]
        public ActionResult LoginExternalCallBack(string acToken)
        {
            User objUser = null;
            try
            {
                if (!string.IsNullOrEmpty(acToken))
                {
                    string userInfo = "";

                    userInfo = "https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + acToken;
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(userInfo);
                    WebResponse response = (HttpWebResponse)webRequest.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    userInfo = reader.ReadToEnd();
                    reader.Close();
                    GoogleUser objGoogleUser = new JavaScriptSerializer().Deserialize<GoogleUser>(userInfo);
                    if (objGoogleUser != null)
                    {
                        objUserBL = new UserBL();
                        objUser = objUserBL.GetData(new UserFilter() { Filter = " and Email = '" + objGoogleUser.email + "'" }, true).FirstOrDefault();
                        if (objUser != null)
                        {
                            Session["User"] = objUser;
                            Session["UserId"] = objUser.UserId;
                            FormsAuthentication.SetAuthCookie(objUser.Email, true);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ViewBag.LoginError = "Something went wrong please try again";
                            return View("Login");
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                objUserBL = null;
                objUser = null;
            }

        }
    }
}