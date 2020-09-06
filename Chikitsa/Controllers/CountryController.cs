using Chikitsa.BusinessLayer;
using Chikitsa.Entities;
using Chikitsa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chikitsa.Controllers
{
    public class CountryController : Controller
    {
        Response objResponse;
        CountryBL objCountryBL = null;
        CountryDetailsVM objDetailsVM = null;
        CountryMasterListVM objListVM = null;


        private CountryMasterListVM GetCountryListVM(CountryMasterFilter objFilter)
        {
            //Thread.Sleep(1000);
            //CountryMasterListVM userListVM = null;
            CommonBL objCommonBL = null;
            try
            {
                objCountryBL = new CountryBL();
                objListVM = new CountryMasterListVM();
                objCommonBL = new CommonBL();
                objFilter.PagingRoute = new PagingRoute() { Action = "Index", Controller = "Country" };
                objListVM.lstCountryMaster = objCountryBL.GetData(objFilter);
                objListVM.lstUserTypes = objCommonBL.GetCodeDetail(new CodeDetailFilter() { CodeTypeId = 1 });
                objListVM.QueryBO = objListVM.objFilter = objFilter;
                return objListVM;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                objListVM = null;
            }

        }
        public ActionResult Index(CountryMasterFilter objFilter, int? pageNumber)
        {
            CountryMasterListVM empVM = null;
            try
            {
                objCountryBL = new CountryBL();
                if (TempData["CountryFilter"] != null)
                {
                    objFilter = (CountryMasterFilter)TempData["CountryFilter"];
                    objFilter.CountryID = 0;
                    if (pageNumber != null)
                    {
                        if (pageNumber != 0)
                            objFilter.PageNumber = pageNumber ?? objFilter.PageNumber;
                        TempData.Keep("CountryFilter");
                    }
                }
                empVM = GetCountryListVM(objFilter);
                return View("Index", empVM);
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                empVM = null;
                objCountryBL = null;
            }
        }
        [HttpPost]
        public ActionResult Filter(CountryMasterFilter objFilter, string submit)
        {
            try
            {
                if (submit.ToLower() == "filter")
                {
                    objFilter.PageNumber = 1;
                    //objFilter.Sort = "";
                    TempData["CountryFilter"] = objFilter;
                }
                else
                {
                    TempData["CountryFilter"] = null;
                    objFilter = new CountryMasterFilter();
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
        [ValidateAntiForgeryToken]
        public ActionResult Action(CountryMasterFilter objFilter, string submit)
        {
            CommonBL objCommonBL = null;
            CountryMaster objCountry = null;
            try
            {
                objDetailsVM = new CountryDetailsVM();
                if (submit.ToLower() == "edit")
                {
                    objCommonBL = new CommonBL();
                    TempData["CountryFilter"] = objFilter;
                    objCountryBL = new CountryBL();
                    objDetailsVM = new CountryDetailsVM();
                    objDetailsVM.CntMstr = objCountryBL.GetData(new CountryMasterFilter() { CountryID = objFilter.CountryID }).FirstOrDefault();
                    // objDetailsVM.lstUserTypes = objCommonBL.GetCodeDetail(new CodeDetailFilter() { CodeTypeId = 1 });
                    return View("Details", objDetailsVM);
                }
                else if (submit.ToLower() == "delete")
                {
                    objCountryBL = new CountryBL();
                    objListVM = new CountryMasterListVM();
                    objCountry = new CountryMaster(objFilter.CountryID);
                    objCountry.ModifiedBy = 1;
                    objCountry.Remark = "Deleted";
                    objResponse = objCountryBL.SaveCountry(objCountry, "D");
                    objFilter.CountryID = 0;
                    objListVM = GetCountryListVM(objFilter);
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
                objCountryBL = null;
                objDetailsVM = null;
                objListVM = null;
                objCommonBL = null;
            }
            return null;
        }


        // GET: User
        public ActionResult Details()
        {
            CommonBL objCommonBL = null;
            try
            {

                objDetailsVM = new CountryDetailsVM();
                objCommonBL = new CommonBL();
                //objDetailsVM.CntMstr.Status = true;
                //objDetailsVM.lstUserTypes = objCommonBL.GetCodeDetail(new CodeDetailFilter() { CodeTypeId = 1 });
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
        [ActionName("Details")]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsPost(CountryDetailsVM objDetailsVM, string Submit, HttpPostedFileBase file)
        {
            CommonBL objCommonBL = null;
            try
            {
                if (Submit == "Save")
                {

                    objCountryBL = new CountryBL();
                    objCommonBL = new CommonBL();

                    objDetailsVM.CntMstr.Status = 1;
                    //if (file != null)
                    //{
                    //    string path = "~/Content/profileimages/" + Guid.NewGuid() + file.FileName;
                    //    file.SaveAs(Server.MapPath(path));
                    //    //objDetailsVM.User.ImageUrl = path;//.Substring(2, path.Length - 2);
                    //}
                    objResponse = objCountryBL.SaveCountry(objDetailsVM.CntMstr, objDetailsVM.CntMstr.CountryID > 0 ? "U" : "C");
                    //  objDetailsVM.lstUserTypes = objCommonBL.GetCodeDetail(new CodeDetailFilter() { CodeTypeId = 1 });
                    objDetailsVM.Toast = WebCommon.SetToast(objResponse, "Country", "Index");
                }
                return View(objDetailsVM);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                objCountryBL = null;
                objDetailsVM = null;
                objCommonBL = null;
            }
        }
    }
}