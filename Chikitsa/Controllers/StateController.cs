using Chikitsa.BusinessLayer;
using Chikitsa.Entities;
using Chikitsa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Chikitsa.DataAccessLayer;

namespace Chikitsa.Controllers
{
    public class StateController : Controller
    {
        Response objResponse;
        StateBL objStateBL = null;
        StateDetailsVM objDetailsVM = null;
        StateMasterListVM objListVM = null;
        private StateMasterListVM GetStateListVM(StateMasterFilter objFilter)
        {
            //Thread.Sleep(1000);
            //CountryMasterListVM userListVM = null;
            CommonBL objCommonBL = null;
            try
            {
                objStateBL = new StateBL();
                objListVM = new StateMasterListVM();
                objCommonBL = new CommonBL();
                objFilter.PagingRoute = new PagingRoute() { Action = "Index", Controller = "State" };
                objListVM.lstStateMaster = objStateBL.GetData(objFilter);
                //objDetailsVM.CntMst = Chikitsa.DataAccessLayer.StateDAL.GetCountry(); //objCommonBL.GetCodeDetail(new CodeDetailFilter() { CodeTypeId = 1 });
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
        public ActionResult Index(StateMasterFilter objFilter, int? pageNumber)
        {
            StateMasterListVM empVM = null;
            try
            {
                objStateBL = new StateBL();
                if (TempData["StateFilter"] != null)
                {
                    objFilter = (StateMasterFilter)TempData["StateFilter"];
                    objFilter.CountryID = 0;
                    if (pageNumber != null)
                    {
                        if (pageNumber != 0)
                            objFilter.PageNumber = pageNumber ?? objFilter.PageNumber;
                        TempData.Keep("StateFilter");
                    }
                }
                empVM = GetStateListVM(objFilter);
                return View("Index", empVM);
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                empVM = null;
                objStateBL = null;
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
        public ActionResult Action(StateMasterFilter objFilter, string submit)
        {
            CommonBL objCommonBL = null;
            StateMaster objState = null;
            try
            {
                objDetailsVM = new StateDetailsVM();
                if (submit.ToLower() == "edit")
                {
                    objCommonBL = new CommonBL();
                    TempData["CountryFilter"] = objFilter;
                    objStateBL = new StateBL();
                    objDetailsVM = new StateDetailsVM();
                    objDetailsVM.StateMstr = objStateBL.GetData(new StateMasterFilter() { StateID = objFilter.StateID }).FirstOrDefault();
                    objDetailsVM.CntMst = Chikitsa.DataAccessLayer.StateDAL.GetCountry();
                    // objDetailsVM.lstUserTypes = objCommonBL.GetCodeDetail(new CodeDetailFilter() { CodeTypeId = 1 });
                    return View("Details", objDetailsVM);
                }
                else if (submit.ToLower() == "delete")
                {
                    objStateBL = new StateBL();
                    objListVM = new StateMasterListVM();
                    objState = new StateMaster(objFilter.StateID);
                    objState.ModifiedBy = 1;
                    objState.Remark = "Deleted";
                    objResponse = objStateBL.SaveState(objState, "D");
                    objFilter.CountryID = 0;
                    objListVM = GetStateListVM(objFilter);
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
                objStateBL = null;
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

                objDetailsVM = new StateDetailsVM();
                objCommonBL = new CommonBL();
                objDetailsVM.CntMst = Chikitsa.DataAccessLayer.StateDAL.GetCountry();
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
        public ActionResult DetailsPost(StateDetailsVM objDetailsVM, string Submit)
        {
            CommonBL objCommonBL = null;
            try
            {
                if (Submit == "Save")
                {
                    objStateBL = new StateBL();
                    objCommonBL = new CommonBL();
                    objDetailsVM.StateMstr.Status = 1;
                    objResponse = objStateBL.SaveState(objDetailsVM.StateMstr, objDetailsVM.StateMstr.StateID > 0 ? "U" : "C");
                    objDetailsVM.CntMst = Chikitsa.DataAccessLayer.StateDAL.GetCountry();
                    // objDetailsVM.lstUserTypes = objCommonBL.GetCodeDetail(new CodeDetailFilter() { CodeTypeId = 1 });
                    objDetailsVM.Toast = WebCommon.SetToast(objResponse, "State", "Index");
                }
                return View(objDetailsVM);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                objStateBL = null;
                objDetailsVM = null;
                objCommonBL = null;
            }
        }
    }
}