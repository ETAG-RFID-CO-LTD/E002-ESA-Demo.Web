using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EagleServicesWebApp.Models;
using EagleServicesWebApp.Components;

namespace EagleServicesWebApp.Controllers
{
    public class ErrorController : Controller
    {


        public ActionResult NotFound()
        {
            #region Read From XML 
            ViewBag.App_Permission_Error_Desp_text= GlobalFunction.GetStatus_MsgByKey("App_Permission_Error_Desp_text");
            ViewBag.App_Permission_Error_text = GlobalFunction.GetStatus_MsgByKey("App_Permission_Error_text");
            
            #endregion

            return View();
        }
        public ActionResult NoPermission()
        {
            #region Read From XML 
            ViewBag.App_Permission_Error_Desp_text = GlobalFunction.GetStatus_MsgByKey("App_Permission_Error_Desp_text");
            ViewBag.App_Permission_Error_text = GlobalFunction.GetStatus_MsgByKey("App_Permission_Error_text");

            #endregion

            return View();
        }
    }
}