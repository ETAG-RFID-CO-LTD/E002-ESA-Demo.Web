using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EagleServicesWebApp.Models;
using System.Configuration;
using EagleServicesWebApp.Models;

namespace EagleServicesWebApp.Controllers
{
    public class PartialController : Controller
    {
        string sLogFilePath = ConfigurationManager.AppSettings.Get("LogFilePath");

        public ActionResult MainMenu()
        {
            string pas = System.Web.HttpContext.Current.Session["Password"] as String;
            ////Extract With Filter
            string RoleName = Convert.ToString(System.Web.HttpContext.Current.Session["RoleName"]);
            bool isAdminUser = Convert.ToBoolean(System.Web.HttpContext.Current.Session["isAdmin"]);

            MenuModel oClass = new MenuModel();
            var vResult = oClass.GetMainMenuIDRoleID(RoleName).ToList();

            if (!string.IsNullOrEmpty(pas))
            {
                ViewData["ListMenuParent"] = vResult.FindAll(f => f.MenuID.Length == 2).OrderBy(x => x.Ordering);
            }
            else
            {
                ViewData["ListMenuParent"] = vResult.FindAll(f => f.MenuID.Length == 1).OrderBy(x => x.Ordering);
            }
            return PartialView("Partials/MainMenu");
        }

        public static List<Menu_REC> ListMenuChild(string MenuID)
        {
            string pas = System.Web.HttpContext.Current.Session["Password"] as String;
            //Extract With Filter
            string RoleName = Convert.ToString(System.Web.HttpContext.Current.Session["RoleName"]);
            MenuModel oClass = new MenuModel();
            var vResult = oClass.GetMainMenuIDRoleID(RoleName).ToList();
            return vResult.FindAll(f => f.MenuID.Contains(MenuID) && (f.MenuID.Length == 4));
        }

        public static List<Department_REC> GetLookUpGetDepartmentListing()
        {
            //string UserID = System.Web.HttpContext.Current.Session["UserID"] as String;
            bool isAdminUser = Convert.ToBoolean(System.Web.HttpContext.Current.Session["isAdmin"]);
            string UserID = "";

            Department_REC department_REC = new Department_REC();
            DepartmentModel oClass = new DepartmentModel();
            var vResultdata = oClass.GetDepListByUserID(UserID).ToList();
            return vResultdata;
        }
        public static List<Department_REC> GetLookUpGetUpdateDepartmentListing(string Data)
        {
            Department_REC department_REC = new Department_REC();
            DepartmentModel oClass = new DepartmentModel();
            var vResultdata = oClass.GetDepListByUserID(Data).ToList();
            return vResultdata;
        }
        //

        public static List<Department_REC> GetSelectedDepListByUserID(string Data)
        {
            Department_REC department_REC = new Department_REC();
            DepartmentModel oClass = new DepartmentModel();
            var vResultdata = oClass.GetSelectedDepListByUserID(Data).ToList();
            return vResultdata;
        }


        public static List<Menu_REC> MenuChild()
        {
            //Extract With Filter
            string pas = System.Web.HttpContext.Current.Session["Password"] as String;
            string RoleName = Convert.ToString(System.Web.HttpContext.Current.Session["RoleName"]);
            MenuModel oClass = new MenuModel();
            var vResult = oClass.GetMainMenuIDRoleID(RoleName).ToList();
            return vResult;
        }

      public static List<Menu_REC> MenuChild1(string MenuID)
      {
         //Extract With Filter
         string pas = System.Web.HttpContext.Current.Session["Password"] as String;
         string RoleName = Convert.ToString(System.Web.HttpContext.Current.Session["RoleName"]);
         MenuModel oClass = new MenuModel();
         var vResult = oClass.GetMainMenuIDRoleID(RoleName).ToList();
         if (!string.IsNullOrEmpty(pas))
         {
            return vResult.FindAll(f => f.MenuID.Contains(MenuID));
         }
         else
         {
            return vResult.FindAll(f => f.MenuID.Length == 1);

         }
      }


      public static List<Menu_REC> MenuAllChild(long _RoleID)
        {
            string pas = System.Web.HttpContext.Current.Session["Password"] as String;
            MenuModel oClass = new MenuModel();
            var vResult = oClass.GetList().ToList();
            return vResult.FindAll(f => f.Status == true);
        }

    }
}
