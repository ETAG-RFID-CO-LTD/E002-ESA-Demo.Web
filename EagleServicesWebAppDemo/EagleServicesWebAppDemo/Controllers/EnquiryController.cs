using EagleServicesWebApp.Components;
using EagleServicesWebApp.Models;
using EagleServicesWebApp.Models.Main;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EagleServicesWebApp.Controllers
{
    public class EnquiryController : Controller
    {
        // GET: Enquiry
        public ActionResult Index()
        {
            #region Users Message From XML
            ErrorMessageModel Model = new ErrorMessageModel();
            ViewData["LogInMsgSelect"] = Model.LogInMsgSelect();
            #endregion
            return View();
        }
        public ActionResult GetEngineList()
        {
            MainModel oClass = new MainModel();
            var vResultdata = oClass.GetEngineList().ToList();

            return Json(vResultdata, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetModuleList(int engineID)
        {
            MainModel oClass = new MainModel();
            var vResultdata = oClass.GetModuleList().Where(e => e.EngineID == engineID).ToList();

            return Json(vResultdata, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetInspectionList()
        {
            MainModel oClass = new MainModel();
            var vResultdata = oClass.GetInspectionStatusList().ToList();

            return Json(vResultdata, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Part_Read([DataSourceRequest] DataSourceRequest poRequest, int? enginID, int? moduleID, int? inspectionStatus, string partName)
        {
            string condition = ""; string joinStr;
            if (enginID != null)
                condition += " where engineID=" + enginID.Value;
            if (moduleID != null)
            {
                joinStr = enginID != null ? " and " : " where ";
                condition += joinStr
                              + " moduleID=" + moduleID.Value;
            }
            if (inspectionStatus != null)
            {
                joinStr = (enginID != null || moduleID != null) ? " and " : " where ";
                condition += joinStr
                              + " inspectionStatusID=" + inspectionStatus.Value;
            }
            if (!string.IsNullOrEmpty(partName))
            {
                joinStr = (enginID != null || moduleID != null || inspectionStatus != null) ? " and " : " where ";
                condition += joinStr
                              + " PartName like '%" + partName + "%'";
            }
            MainModel oClass = new MainModel();
            System.Collections.Generic.List<Part_Enquiry> vResult = null;
            try
            {
                int _actualPSize = poRequest.PageSize;
                int _actualPageNo = poRequest.Page;
                if (moduleID != 0)
                    vResult = oClass.GetPartDataByCondition(condition).ToList();
            }
            catch (Exception ex)
            {
                GlobalFunction.SendErrorToText(ex);
            }
            return new JsonResult() { Data = vResult.ToDataSourceResult(poRequest), JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Part_Update([DataSourceRequest] DataSourceRequest poRequest, Part_Enquiry poRecord)
        {
            if ((poRecord != null))
            {
                #region Update External Vendor Info
                MainModel oClass = new MainModel();

                if (!oClass.UpdatePart(poRecord))
                    ModelState.AddModelError("Error", "Error updating.");
                #endregion
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).ToList();

            return Json(new[] { poRecord }.ToDataSourceResult(poRequest, ModelState));
        }
    }
}