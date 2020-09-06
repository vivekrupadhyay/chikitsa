using Chikitsa.BusinessLayer;
using Chikitsa.DataAccessLayer;
using Chikitsa.Entities;
using Chikitsa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chikitsa.Controllers
{
    public class CityController : Controller
    {
        Response objResponse;
        CityBL objCityBL = null;
        CityDetailsVM objDetailsVM = null;
        CityMasterListVM objListVM = null;
        // GET: City
        public ActionResult Index(CityMasterFilter objFilter, int? pageNumber)
        {
            CityMasterListVM empVM = null;
            try
            {
                objCityBL = new CityBL();
                if (TempData["CityFilter"] != null)
                {
                    objFilter = (CityMasterFilter)TempData["CityFilter"];
                    objFilter.CountryID = 0;
                    if (pageNumber != null)
                    {
                        if (pageNumber != 0)
                            objFilter.PageNumber = pageNumber ?? objFilter.PageNumber;
                        TempData.Keep("CityFilter");
                    }
                }
                empVM = GetCityListVM(objFilter);
                return View("Index", empVM);
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                empVM = null;
                objCityBL = null;
            }
        }
        private CityMasterListVM GetCityListVM(CityMasterFilter objFilter)
        {
            //Thread.Sleep(1000);
            //CountryMasterListVM userListVM = null;
            CommonBL objCommonBL = null;
            try
            {
                objCityBL = new CityBL();
                objListVM = new CityMasterListVM();
                objCommonBL = new CommonBL();
                objFilter.PagingRoute = new PagingRoute() { Action = "Index", Controller = "State" };
                objListVM.lstCityMaster = objCityBL.GetData(objFilter);
                // objListVM.lstUserTypes = objCommonBL.GetCodeDetail(new CodeDetailFilter() { CodeTypeId = 1 });
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

        public JsonResult GetStateOnCountryId(Int64 CountryId)
        {
            CommonBL objCommonBL = null;
            try
            {
                objDetailsVM = new CityDetailsVM();
                objCommonBL = new CommonBL();
                objDetailsVM.StateMst = objCommonBL.GetTable<StateMaster>(new TableFilter() { Condition = " and Status = 1 and CountryId = " + CountryId, IdColumn = "StateID", TextColumn = "StateName", TableName = "StateMaster" });
                return Json(new SelectList(objDetailsVM.StateMst, "StateID", "StateName"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetCityByState(Int64 StateId)
        {
            CommonBL objCommonBL = null;
            List<CityMaster> lstCity = null;
            try
            {
                objCommonBL = new CommonBL();
                lstCity = objCommonBL.GetTable<CityMaster>(new TableFilter() { Condition = " and Status = 1 and StateId = " + StateId, IdColumn = "CityID", TextColumn = "Name", TableName = "CityM" });
                return Json(new SelectList(lstCity, "CityID", "CityName"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult Filter(StateMasterFilter objFilter, string submit)
        {
            try
            {
                if (submit.ToLower() == "filter")
                {
                    objFilter.PageNumber = 1;
                    //objFilter.Sort = "";
                    TempData["StateFilter"] = objFilter;
                }
                else
                {
                    TempData["StateFilter"] = null;
                    objFilter = new StateMasterFilter();
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
        public ActionResult Action(CityMasterFilter objFilter, string submit)
        {
            CommonBL objCommonBL = null;
            CityMaster objCity = null;
            try
            {
                objDetailsVM = new CityDetailsVM();
                if (submit.ToLower() == "edit")
                {
                    objCommonBL = new CommonBL();
                    TempData["CountryFilter"] = objFilter;
                    objCityBL = new CityBL();
                    objDetailsVM = new CityDetailsVM();
                    objDetailsVM.CityMstr = objCityBL.GetData(new CityMasterFilter() { CityID = objFilter.StateID }).FirstOrDefault();
                    objDetailsVM.CntMst = Chikitsa.DataAccessLayer.StateDAL.GetCountry();
                    // objDetailsVM.lstUserTypes = objCommonBL.GetCodeDetail(new CodeDetailFilter() { CodeTypeId = 1 });
                    return View("Details", objDetailsVM);
                }
                else if (submit.ToLower() == "delete")
                {
                    objCityBL = new CityBL();
                    objListVM = new CityMasterListVM();
                    objCity = new CityMaster(objFilter.StateID);
                    objCity.ModifiedBy = 1;
                    objCity.Remark = "Deleted";
                    objResponse = objCityBL.SaveState(objCity, "D");
                    objFilter.CountryID = 0;
                    objListVM = GetCityListVM(objFilter);
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
                objCityBL = null;
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

                objDetailsVM = new CityDetailsVM();
                objCommonBL = new CommonBL();

                CityDAL obj = new CityDAL();
                //var query = CityDAL.GetCountry();
                objDetailsVM.CntMst = Chikitsa.DataAccessLayer.StateDAL.GetCountry();

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
        public ActionResult DetailsPost(CityDetailsVM objDetailsVM, string Submit)
        {
            CommonBL objCommonBL = null;
            try
            {
                if (Submit == "Save")
                {

                    objCityBL = new CityBL();
                    objCommonBL = new CommonBL();

                    objDetailsVM.CityMstr.Status = 1;

                    objResponse = objCityBL.SaveState(objDetailsVM.CityMstr, objDetailsVM.CityMstr.CityID > 0 ? "U" : "C");
                    objDetailsVM.CntMst = Chikitsa.DataAccessLayer.StateDAL.GetCountry();
                    //  objDetailsVM.lstUserTypes = objCommonBL.GetCodeDetail(new CodeDetailFilter() { CodeTypeId = 1 });
                    objDetailsVM.Toast = WebCommon.SetToast(objResponse, "City", "Index");
                }
                return View(objDetailsVM);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                objCityBL = null;
                objDetailsVM = null;
                objCommonBL = null;
            }
        }
    }
}