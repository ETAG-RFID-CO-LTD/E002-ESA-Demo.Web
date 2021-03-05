using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System.Threading.Tasks;

using EagleServicesWebApp.Models.RollRoyceSystem;
using System.Configuration;
using EagleServicesWebApp.Components;
using EagleServicesWebApp.Models.Enquiry;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Threading;
using EagleServicesWebApp.Models.Main;

namespace EagleServicesWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private static Mutex mutex = new Mutex();

        #region Error Page
        public ActionResult ExceptionError()
        {
            return View();
        }
        #endregion
        public ActionResult Home()
        {
            //#region Session
            //System.Web.HttpContext.Current.Session["UserID"] = "admin";
            //System.Web.HttpContext.Current.Session["Password"] = "12345678";
            //System.Web.HttpContext.Current.Session["isAdmin"] = false;
            //System.Web.HttpContext.Current.Session["RoleName"] = "Administrator";

            //System.Web.HttpContext.Current.Session["PagerSettingSize"] = 30;
            //#endregion
            MainModel model = new MainModel();
            ViewData["EngineData"] = model.GetEngineList().ToList();
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Outstanding_Read(int engine)
        {
            MainModel oClass = new MainModel();
            var result = oClass.GetDashboardData(engine).ToList();
            return Json(result);
        }
    }
}
