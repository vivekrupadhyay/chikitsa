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
    public class AreaController : Controller
    {
        Response objResponse;
        AreaBL objAreaBL = null;
        AreaDetailsVM objDetailsVM = null;
        AreaMasterListVM objListVM = null;
        // GET: City
        public ActionResult Index(AreaMasterFilter objFilter, int? pageNumber)
        {
            AreaMasterListVM empVM = null;
            try
            {
                objAreaBL = new AreaBL();
                if (TempData["CityFilter"] != null)
                {
                    objFilter = (AreaMasterFilter)TempData["CityFilter"];
                    objFilter.CountryID = 0;
                    if (pageNumber != null)
                    {
                        if (pageNumber != 0)
                            objFilter.PageNumber = pageNumber ?? objFilter.PageNumber;
                        TempData.Keep("CityFilter");
                    }
                }
                empVM = GetAreaListVM(objFilter);
                return View("Index", empVM);
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                empVM = null;
                objAreaBL = null;
            }
        }
        private AreaMasterListVM GetAreaListVM(AreaMasterFilter objFilter)
        {
            //Thread.Sleep(1000);
            //CountryMasterListVM userListVM = null;
            CommonBL objCommonBL = null;
            try
            {
                objAreaBL = new AreaBL();
                objListVM = new AreaMasterListVM();
                objCommonBL = new CommonBL();
                objFilter.PagingRoute = new PagingRoute() { Action = "Index", Controller = "State" };
                objListVM.lstAreaMaster = objAreaBL.GetData(objFilter);
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

        public JsonResult GetStateOnCountryId(Int64 CountryId)
        {
            CommonBL objCommonBL = null;
            try
            {
                objDetailsVM = new AreaDetailsVM();
                objCommonBL = new CommonBL();
                objDetailsVM.StateMst = objCommonBL.GetTable<StateMaster>(new TableFilter() { Condition = " and Status = 1 and CountryId = " + CountryId, IdColumn = "StateId", TextColumn = "StateName", TableName = "StateMaster" });
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
                lstCity = objCommonBL.GetTable<CityMaster>(new TableFilter() { Condition = " and Status = 1 and StateId = " + StateId, IdColumn = "CityID", TextColumn = "CityName", TableName = "CityMaster" });
                return Json(new SelectList(lstCity, "CityID", "CityName"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Action(AreaMasterFilter objFilter, string submit)
        {
            CommonBL objCommonBL = null;
            AreaMaster objCity = null;
            try
            {
                objDetailsVM = new AreaDetailsVM();
                if (submit.ToLower() == "edit")
                {
                    objCommonBL = new CommonBL();
                    TempData["CountryFilter"] = objFilter;
                    objAreaBL = new AreaBL();
                    objDetailsVM = new AreaDetailsVM();
                    objDetailsVM.AreaMstr = objAreaBL.GetData(new AreaMasterFilter() { AreaID = objFilter.AreaID }).FirstOrDefault();
                    objDetailsVM.CntMst = Chikitsa.DataAccessLayer.StateDAL.GetCountry();
                    // objDetailsVM.lstUserTypes = objCommonBL.GetCodeDetail(new CodeDetailFilter() { CodeTypeId = 1 });
                    return View("Details", objDetailsVM);
                }
                else if (submit.ToLower() == "delete")
                {
                    objAreaBL = new AreaBL();
                    objListVM = new AreaMasterListVM();
                    objCity = new AreaMaster(objFilter.StateID);
                    objCity.ModifiedBy = 1;
                    objCity.Remarks = "Deleted";
                    objResponse = objAreaBL.SaveArea(objCity, "D");
                    objFilter.CountryID = 0;
                    objListVM = GetAreaListVM(objFilter);
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
                objAreaBL = null;
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

                objDetailsVM = new AreaDetailsVM();
                objCommonBL = new CommonBL();
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
        public ActionResult DetailsPost(AreaDetailsVM objDetailsVM, string Submit)
        {
            CommonBL objCommonBL = null;
            try
            {
                if (Submit == "Save")
                {

                    objAreaBL = new AreaBL();
                    objCommonBL = new CommonBL();

                    objDetailsVM.AreaMstr.Status = 1;

                    objResponse = objAreaBL.SaveArea(objDetailsVM.AreaMstr, objDetailsVM.AreaMstr.AreaID > 0 ? "U" : "C");
                    objDetailsVM.CntMst = Chikitsa.DataAccessLayer.StateDAL.GetCountry();
                    //  objDetailsVM.lstUserTypes = objCommonBL.GetCodeDetail(new CodeDetailFilter() { CodeTypeId = 1 });
                    objDetailsVM.Toast = WebCommon.SetToast(objResponse, "Area", "Index");
                }
                return View(objDetailsVM);
            }
            catch (Exception ex)
            {


                throw ex;
            }
            finally
            {
                objAreaBL = null;
                objDetailsVM = null;
                objCommonBL = null;
            }
        }
    }
}