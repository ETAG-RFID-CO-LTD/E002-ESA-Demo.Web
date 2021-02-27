using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using EagleServicesWebApp.Components;
using EagleServicesWebApp.Models.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using EagleServicesWebApp.Models;
using System.IO;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

using System.Globalization;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Data.OleDb;
using System.Data;

namespace EagleServicesWebApp.Controllers
{
    public class MainController : Controller
    {
        // GET: Main
        public ActionResult Module(int? engine)
        {
            System.Web.HttpContext.Current.Session["engineNo"] = engine != null ? engine : 1;
            MainModel model = new MainModel();
            System.Web.HttpContext.Current.Session["TitleName"] = model.GetEngineList().ToList().SingleOrDefault(s=>s.EngineID== int.Parse(System.Web.HttpContext.Current.Session["engineNo"].ToString())).EngineName.Trim();
            return View();
        }
        public ActionResult Engine_Read([DataSourceRequest] DataSourceRequest poRequest)
        {

            MainModel oClass = new MainModel();
            System.Collections.Generic.List<Module_Rec> vResult = null;
            try
            {
                int _actualPSize = poRequest.PageSize;
                int _actualPageNo = poRequest.Page;
                int engineNo = 0;
                if (System.Web.HttpContext.Current.Session["engineNo"] != null)
                {
                    engineNo = int.Parse(System.Web.HttpContext.Current.Session["engineNo"].ToString());
                }
                vResult = oClass.GetModuleData(engineNo).ToList();
            }
            catch (Exception ex)
            {
                GlobalFunction.SendErrorToText(ex);
            }
            return new JsonResult() { Data = vResult.ToDataSourceResult(poRequest), JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ResetData(int engID)
        {
            if (engID != 0 )
            {
                MainModel oClass = new MainModel();
                bool vResultdata = oClass.RestEnigne(engID);
                return Json(new { result = vResultdata });
            }
            return Json(new { result = false });
        }
        public ActionResult Part(int moduleID)
        {
            System.Web.HttpContext.Current.Session["moduleID"] = moduleID;
            MainModel model = new MainModel();
            string moduleName = model.GetModuleList().ToList().SingleOrDefault(d => d.ModuleID == moduleID).ModuleName.Trim();
            System.Web.HttpContext.Current.Session["TitleName"] = System.Web.HttpContext.Current.Session["TitleName"]!=null ? System.Web.HttpContext.Current.Session["TitleName"].ToString() + 
                                                                " > "+ moduleName : moduleName;
            return View();
        }
        public ActionResult Part_Read([DataSourceRequest] DataSourceRequest poRequest)
        {

            MainModel oClass = new MainModel();
            System.Collections.Generic.List<Part_Rec> vResult = null;
            try
            {
                int _actualPSize = poRequest.PageSize;
                int _actualPageNo = poRequest.Page;
                int module = 0;
                if (System.Web.HttpContext.Current.Session["moduleID"] != null)
                {
                    module = int.Parse(System.Web.HttpContext.Current.Session["moduleID"].ToString());
                }
                vResult = oClass.GetPartData(module).ToList();
            }
            catch (Exception ex)
            {
                GlobalFunction.SendErrorToText(ex);
            }
            return new JsonResult() { Data = vResult.ToDataSourceResult(poRequest), JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
    }
}