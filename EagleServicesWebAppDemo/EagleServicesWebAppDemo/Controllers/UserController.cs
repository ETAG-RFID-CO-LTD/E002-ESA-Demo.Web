using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using EagleServicesWebApp.Components;
using EagleServicesWebApp.Models.Enquiry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using EagleServicesWebApp.Models.RollRoyceSystem;
using EagleServicesWebApp.Models.GeneralTableModel;
using EagleServicesWebApp.Models;

namespace EagleServicesWebApp.Controllers
{
    public class UserController : Controller
    {
        #region Read From XML
        string sLogFilePath = ConfigurationManager.AppSettings.Get("LogFilePath");
        string errorcode_text = GlobalFunction.GetStatus_MsgByKey("errorcode_text");
        string title_text = GlobalFunction.GetStatus_MsgByKey("title_text");
        string msg_text = GlobalFunction.GetStatus_MsgByKey("msg_text");
        string no_errorcode = GlobalFunction.GetStatus_MsgByKey("no_errorcode");
        string error_code = GlobalFunction.GetStatus_MsgByKey("error_code");
        string Warning_Noti = GlobalFunction.GetStatus_MsgByKey("Warning_Noti");
        string Error_text = GlobalFunction.GetStatus_MsgByKey("Error_text");
        string success_title = GlobalFunction.GetStatus_MsgByKey("success_title");
        string success_Noti = GlobalFunction.GetStatus_MsgByKey("success_Noti");
        string info_Noti = GlobalFunction.GetStatus_MsgByKey("info_Noti");
        string CardIDdigit = GlobalFunction.GetStatus_MsgByKey("CardIDdigit");
        string UserID_Dup = GlobalFunction.GetStatus_MsgByKey("UserID_Dup");
        #endregion

        #region "Users..."
        public ActionResult Users()
        {
            try
            {
                string password = System.Web.HttpContext.Current.Session["Password"] as String;
                if (!string.IsNullOrEmpty(password))
                {
                    #region Button Access Right
                    string RoleName = System.Web.HttpContext.Current.Session["RoleName"] as String;
                    RolePermissionModel rolePermission = new RolePermissionModel();
                    var accesibleRoles = rolePermission.GetMenuPermissionByMenuName(RoleName, "Users").ToList().SingleOrDefault();
                    if (accesibleRoles == null)
                    {
                        accesibleRoles.IsAllowAdd = false;
                        accesibleRoles.IsAllowEdit = false;
                        accesibleRoles.IsAllowDelete = false;
                    }

                    System.Web.HttpContext.Current.Session["Roles"] = accesibleRoles;
                    #endregion

                    #region Users Message From XML
                    ErrorMessageModel Model = new ErrorMessageModel();
                    ViewData["LogInMsgSelect"] = Model.LogInMsgSelect();
                    #endregion

                    return View();
                }
                else
                    return RedirectToAction("Index", "Login");
            }
            catch (Exception exp)
            {
                GlobalFunction.SendErrorToText(exp);
                return RedirectToAction("ExceptionError", "Dashboard");
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Users_Create([DataSourceRequest]DataSourceRequest poRequest, User_REC poRecord)
        {
            #region Validation

            if ((poRecord.FirstName == null) && (poRecord.LastName == null))
            {
                poRecord.FirstName = "-";
                poRecord.LastName = "-";
            }
            else if (poRecord.FirstName == null)
                poRecord.FirstName = "-";
            else if (poRecord.LastName == null)
                poRecord.LastName = "-";
            else
            {
                poRecord.FirstName = poRecord.FirstName.Trim().ToString();
                poRecord.LastName = poRecord.LastName.Trim().ToString();
            }

            if (poRecord.Status == "Active")
                poRecord.isActive = true;
            else
                poRecord.isActive = false;
            if (poRecord.RoleName == "Super Administrator")
                poRecord.isAdmin = true;
            else
                poRecord.isAdmin = false;
            #endregion           

            if ((poRecord != null))
            {
                string UserID = System.Web.HttpContext.Current.Session["UserID"].ToString();
                poRecord.CreatedBy = UserID;
                UserModel oClass = new UserModel();

                #region Create New Role
                poRecord.Password = "Password!23";
                poRecord.ConfirmPassowrd = "Password!23";


                if (poRecord.Password == poRecord.ConfirmPassowrd)
                {
                    if (!oClass.Insert(poRecord))
                    {
                        if (oClass.ErrorMessage.Contains("Violation of PRIMARY KEY constraint"))
                        {

                            ModelState.AddModelError(Error_text, UserID_Dup);
                        }
                        else
                            ModelState.AddModelError(Error_text, oClass.ErrorMessage);
                    }
                }
                #endregion
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).ToList();

            if (errors.Count > 2)
            {
                return Json(new[] { poRecord }.ToDataSourceResult(poRequest, ModelState));
            }
            else
            {
                return Json(new[] { poRecord }.ToDataSourceResult(poRequest, ModelState));

            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Users_Read([DataSourceRequest]DataSourceRequest poRequest)
        {
            UserModel oClass = new UserModel();
            var vResult = oClass.GetListingExceptSystemAdmin().ToList();
            return Json(vResult.ToDataSourceResult(poRequest));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Users_Update([DataSourceRequest]DataSourceRequest poRequest, User_REC poRecord)
        {
            #region Validation

            if ((poRecord.FirstName == null) && (poRecord.LastName == null))
            {
                poRecord.FirstName = "-";
                poRecord.LastName = "-";
            }
            else if (poRecord.FirstName == null)
                poRecord.FirstName = "-";
            else if (poRecord.LastName == null)
                poRecord.LastName = "-";
            else
            {
                poRecord.FirstName = poRecord.FirstName.Trim().ToString();
                poRecord.LastName = poRecord.LastName.Trim().ToString();
            }

            if (poRecord.Status == "Active")
                poRecord.isActive = true;
            else
                poRecord.isActive = false;
            if (poRecord.RoleName == "Super Administrator")
                poRecord.isAdmin = true;
            else
                poRecord.isAdmin = false;
            #endregion

            if ((poRecord != null) && (ModelState.IsValid))
            {
                string UserID = System.Web.HttpContext.Current.Session["UserID"].ToString();
                poRecord.UpdatedBy = UserID;
                UserModel oClass = new UserModel();

                #region Update Users Info
                poRecord.Password = "Password!23";
                poRecord.ConfirmPassowrd = "Password!23";
                if (!oClass.UpdateInfo(poRecord))
                {
                    if (oClass.ErrorMessage.Contains("Violation of PRIMARY KEY constraint"))
                    {
                        string UserID_Dup = GlobalFunction.GetStatus_MsgByKey("UserID_Dup");
                        ModelState.AddModelError(Error_text, UserID_Dup);
                    }
                    else
                        ModelState.AddModelError(Error_text, oClass.ErrorMessage);
                }

                #endregion

            }

            return Json(new[] { poRecord }.ToDataSourceResult(poRequest, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Users_UpdatePwd([DataSourceRequest]DataSourceRequest poRequest, User_REC poRecord)
        {
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            if ((poRecord != null))
            {
                string UserID = System.Web.HttpContext.Current.Session["UserID"].ToString();
                poRecord.UpdatedBy = UserID;
                UserModel oClass = new UserModel();
                poRecord.Password = "Password!23";
                poRecord.ConfirmPassowrd = "Password!23";
                poRecord.LogQuery = "Update password to tblUser Table==>Change Password=" + poRecord.ConfirmPassowrd + " By UserID=" + poRecord.UserID + ",RoleName=" + poRecord.RoleName + ",isActive=" + poRecord.isActive + ",isadmin=" + poRecord.isAdmin + ",Status=" + poRecord.Status;

                if (!oClass.UpdatePwd(poRecord))
                {
                    //ModelState.AddModelError(Error_text, oClass.ErrorMessage);
                    vsLogMessage.Add(errorcode_text, error_code);
                    vsLogMessage.Add(title_text, Warning_Noti);
                    vsLogMessage.Add(msg_text, oClass.ErrorMessage);
                }
                else
                {
                    string DefaultPwd_resetInfo = GlobalFunction.GetStatus_MsgByKey("DefaultPwd_resetInfo");
                    vsLogMessage.Add(errorcode_text, no_errorcode);
                    vsLogMessage.Add(title_text, success_Noti);
                    vsLogMessage.Add(msg_text, DefaultPwd_resetInfo);
                }
            }
            return Json(vsLogMessage, JsonRequestBehavior.DenyGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Users_Destroy([DataSourceRequest]DataSourceRequest poRequest, User_REC poRecord)
        {
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            if (poRecord != null)
            {
                string UserID_Inused = GlobalFunction.GetStatus_MsgByKey("UserID_Inused");
                string UserID = System.Web.HttpContext.Current.Session["UserID"].ToString();
                if (UserID != poRecord.UserID)
                {
                    UserModel oClass = new UserModel();
                    if (!oClass.Delete(poRecord, UserID))
                    {
                        vsLogMessage.Add(errorcode_text, error_code);
                        vsLogMessage.Add(title_text, Warning_Noti);
                        vsLogMessage.Add(msg_text, oClass.ErrorMessage);
                    }
                    else
                    {
                        vsLogMessage.Add(errorcode_text, no_errorcode);
                        vsLogMessage.Add(title_text, success_Noti);
                    }
                }
                else
                {
                    //ModelState.AddModelError(info_Noti, UserID_Inused);
                    vsLogMessage.Add(errorcode_text, error_code);
                    vsLogMessage.Add(title_text, Warning_Noti);
                    vsLogMessage.Add(msg_text, UserID_Inused);
                }
            }
            //return Json(new[] { poRecord }.ToDataSourceResult(poRequest, ModelState));
            return Json(vsLogMessage, JsonRequestBehavior.DenyGet);
        }

        public ActionResult GetLookUpRoleList()
        {
            UserModel oClass = new UserModel();
            var vResultdata = oClass.GetListForRoleSearch().ToList();
            //var vResult = (from e in vResultdata
            //               where e.RoleName.ToLower().StartsWith(text.ToLower())
            //               select new { e.RoleName, e.RoleID });
            return Json(vResultdata, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLookUpStatus()
        {
            List<User_REC> _Status = new List<User_REC>();
            User_REC _item1 = new User_REC();
            _item1.Status = "Active";
            _Status.Add(_item1);

            User_REC _item2 = new User_REC();
            _item2.Status = "Inactive";
            _Status.Add(_item2);
            var vResult = _Status.ToList();

            return Json(vResult, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region "Users Access Right..."
        public ActionResult UserAccessRight()
        {
            try
            {
                string password = System.Web.HttpContext.Current.Session["Password"] as String;
                if (!string.IsNullOrEmpty(password))
                {
                    #region Button Access Right
                    string RoleName = System.Web.HttpContext.Current.Session["RoleName"] as String;
                    RolePermissionModel rolePermission = new RolePermissionModel();
                    var accesibleRoles = rolePermission.GetMenuPermissionByMenuName(RoleName, "UserAccessRight").ToList().SingleOrDefault();
                    if (accesibleRoles == null)
                    {
                        accesibleRoles.IsAllowAdd = false;
                        accesibleRoles.IsAllowEdit = false;
                        accesibleRoles.IsAllowDelete = false;
                    }

                    System.Web.HttpContext.Current.Session["Roles"] = accesibleRoles;
                    #endregion

                    return View();
                }
                else
                    return RedirectToAction("Index", "Login");
            }
            catch (Exception exp)
            {
                GlobalFunction.SendErrorToText(exp);
                return RedirectToAction("ExceptionError", "Dashboard");
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Role_Read([DataSourceRequest]DataSourceRequest poRequest)
        {
            UserModel oClass = new UserModel();
            var vResult = oClass.GetRoleListingExceptIsAdmin().ToList();
            return Json(vResult.ToDataSourceResult(poRequest));
        }

        public ActionResult AddNew()
        {
            string pas = System.Web.HttpContext.Current.Session["Password"] as String;

            if (!string.IsNullOrEmpty(pas))
            {
                #region User Access Right Message From XML
                ErrorMessageModel Model = new ErrorMessageModel();
                ViewData["UserAccessRightMsgSelect"] = Model.UserAccessRightMsgSelect();
                #endregion

                return View("UserAccessControl/Form");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult UpdateInfo(string id)
        {
            string pas = System.Web.HttpContext.Current.Session["Password"] as String;

            if (!string.IsNullOrEmpty(pas))
            {
                #region User Access Right Message From XML
                ErrorMessageModel Model = new ErrorMessageModel();
                ViewData["UserAccessRightMsgSelect"] = Model.UserAccessRightMsgSelect();

                //Get data from Role Permission
                string RoleName = "";
                string MenuData = "";
                string MenuSelectedData = "";
                string MobileData = "0";
                UserModel userModel = new UserModel();
                var UserPermissionData = userModel.GetRolePermissionData(id).ToList();
                foreach (RolePermission_REC rolepermission in UserPermissionData)
                {
                    RoleName = rolepermission.RoleName;
                    if (rolepermission.MenuID != "M4")
                    {
                        MenuData += rolepermission.MenuID + ",";

                        if (rolepermission.MenuID == "M1P1")
                            MenuSelectedData += rolepermission.MenuID + ",";
                        if (rolepermission.MenuID == "M1P2")
                            MenuSelectedData += rolepermission.MenuID + ",";
                        if (rolepermission.MenuID == "M1P3")
                            MenuSelectedData += rolepermission.MenuID + ",";
                        if (rolepermission.MenuID == "M1P4")
                            MenuSelectedData += rolepermission.MenuID + ",";
                        if (rolepermission.MenuID == "M1P5")
                            MenuSelectedData += rolepermission.MenuID + ",";
                    }
                    if (rolepermission.IsListing != "")
                    {
                        MenuData += rolepermission.IsListing + ",";
                        MenuSelectedData += rolepermission.IsListing + ",";
                    }
                    if (rolepermission.IsAdd != "")
                    {
                        MenuData += rolepermission.IsAdd + ",";
                        MenuSelectedData += rolepermission.IsAdd + ",";
                    }
                    if (rolepermission.IsUpdate != "")
                    {
                        MenuData += rolepermission.IsUpdate + ",";
                        MenuSelectedData += rolepermission.IsUpdate + ",";
                    }
                    if (rolepermission.IsDelete != "")
                    {
                        MenuData += rolepermission.IsDelete + ",";
                        MenuSelectedData += rolepermission.IsDelete + ",";
                    }

                    //mobile
                    if (rolepermission.MenuID == "M4")
                    {
                        if (MobileData == "0")
                            MobileData = rolepermission.MenuID;
                    }
                }
                ViewData["MobileData"] = MobileData;
                ViewData["MenuData"] = MenuData;
                ViewData["MenuSelectedData"] = MenuSelectedData;
                ViewData["RoleName"] = RoleName;
                #endregion

                return View("UserAccessControl/FormEdit");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserAccessRight_Create([DataSourceRequest]DataSourceRequest poRequest, RolePermission_REC poRecord)
        {
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            string _ResultMsg = string.Empty;
            bool RoleFunctionMap = true;

            if (poRecord != null)
            {
                try
                {
                    UserModel oClass = new UserModel();

                    Role_REC objRole = new Role_REC();
                    objRole.RoleName = poRecord.RoleName;
                    objRole.Description = poRecord.RoleName;
                    objRole.CreatedBy = System.Web.HttpContext.Current.Session["UserID"].ToString();
                    objRole.CreatedDate = DateTime.Now;

                    List<FindDataCount> vResult = new List<FindDataCount>();
                    vResult = oClass.CheckUserRoleName(poRecord.RoleName).ToList();

                    if (vResult[0].Rcount > 0)
                    {
                        _ResultMsg = GlobalFunction.GetStatus_MsgByKey("UserRole_Dup_Noti");
                        vsLogMessage.Add(errorcode_text, error_code);
                        vsLogMessage.Add(title_text, Warning_Noti);
                        vsLogMessage.Add(msg_text, _ResultMsg);
                    }
                    else
                    {
                        #region  Add New Data

                        if (oClass.Insert(objRole))
                        {
                            #region After Successfully Create User Role Name ,create Role Permission 

                            if (poRecord.MenuID.ToString().Contains("All") && poRecord.MobileID != "0")
                            {
                                //Insert One Record For All
                                RolePermission_REC RolePermissionRecord = new RolePermission_REC();

                                #region Insert All
                                RolePermissionRecord.CreatedBy = System.Web.HttpContext.Current.Session["UserID"].ToString();
                                RolePermissionRecord.RoleName = poRecord.RoleName;
                                if (!oClass.AllInfoInsertRolePermission(RolePermissionRecord))
                                {
                                    RoleFunctionMap = false;
                                }
                                #endregion
                            }
                            else if (poRecord.MenuID.ToString().Contains("All") && poRecord.MobileID == "0")
                            {
                                //Insert One Record For All except mobile
                                RolePermission_REC RolePermissionRecord = new RolePermission_REC();

                                #region Insert All except Mobile
                                RolePermissionRecord.CreatedBy = System.Web.HttpContext.Current.Session["UserID"].ToString();
                                RolePermissionRecord.RoleName = poRecord.RoleName;
                                if (!oClass.AllInfoInsertRolePermissionExceptMobile(RolePermissionRecord))
                                {
                                    RoleFunctionMap = false;
                                }
                                #endregion
                            }
                            else
                            {
                                ////Split Chosen Menu String    
                                //Get Menu data
                                MenuModel menuModel = new MenuModel();
                                var vMenuResult = menuModel.GetList().ToList();

                                List<RolePermission_REC> PermissionList = new List<RolePermission_REC>();
                                RolePermission_REC ObjPermission;

                                var MenuArray = poRecord.MenuID.Split(',');

                                foreach (Menu_REC menu in vMenuResult)
                                {
                                    if (MenuArray.Any(item => item == menu.MenuID))
                                    {
                                        //check data in object listing
                                        var checkduplicate = PermissionList.Where(Permission => Permission.MenuID == menu.MenuID).ToList();
                                        if (checkduplicate.Count == 0)
                                        {
                                            ObjPermission = new RolePermission_REC();
                                            ObjPermission.RoleName = poRecord.RoleName;
                                            ObjPermission.MenuID = menu.MenuID;
                                            ObjPermission.IsAllowAdd = false;
                                            ObjPermission.IsAllowEdit = false;
                                            ObjPermission.IsAllowDelete = false;
                                            if (menu.MenuID == "M0" || menu.MenuID == "M1" || menu.MenuID == "M2" || menu.MenuID == "M3" || menu.MenuID == "M1P2" || menu.MenuID == "M1P5")
                                            {
                                                ObjPermission.IsAllowAdd = true;
                                                ObjPermission.IsAllowEdit = true;
                                                ObjPermission.IsAllowDelete = true;
                                            }
                                            else
                                            {
                                                string[] child;
                                                child = MenuArray.Where(b => b.ToString().Contains(menu.MenuName)).ToArray<string>();
                                                if (Array.Exists(child, element => element.Contains("Insert")))
                                                    ObjPermission.IsAllowAdd = true;
                                                if (Array.Exists(child, element => element.Contains("Update")))
                                                    ObjPermission.IsAllowEdit = true;
                                                if (Array.Exists(child, element => element.Contains("Delete")))
                                                    ObjPermission.IsAllowDelete = true;
                                            }
                                            PermissionList.Add(ObjPermission);
                                        }
                                    }

                                    if (poRecord.MobileID.Trim() != "0")
                                    {
                                        if (poRecord.MobileID == menu.MenuID)
                                        {
                                            ObjPermission = new RolePermission_REC();
                                            ObjPermission.RoleName = poRecord.RoleName;
                                            ObjPermission.MenuID = menu.MenuID;
                                            ObjPermission.IsAllowAdd = false;
                                            ObjPermission.IsAllowEdit = false;
                                            ObjPermission.IsAllowDelete = false;
                                            PermissionList.Add(ObjPermission);
                                        }
                                    }
                                }



                                if (!oClass.InsertRolePermission(PermissionList))
                                {
                                    RoleFunctionMap = false;
                                }
                            }

                            if (RoleFunctionMap)
                            {
                                _ResultMsg = GlobalFunction.GetStatus_MsgByKey("Data_Add_Success");
                                vsLogMessage.Add(errorcode_text, no_errorcode);
                                vsLogMessage.Add(title_text, success_title);
                                vsLogMessage.Add(msg_text, _ResultMsg);
                            }
                            else
                            {
                                // delete UserRole
                                oClass.DeleteOnlyUserRole(objRole);
                                _ResultMsg = GlobalFunction.GetStatus_MsgByKey("Data_Add_Fail");
                                vsLogMessage.Add(errorcode_text, error_code);
                                vsLogMessage.Add(title_text, Warning_Noti);
                                vsLogMessage.Add(msg_text, _ResultMsg);
                            }
                            #endregion
                        }
                        else
                        {
                            vsLogMessage.Add(errorcode_text, error_code);
                            vsLogMessage.Add(title_text, Warning_Noti);
                            vsLogMessage.Add(msg_text, oClass.ErrorMessage);
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    GlobalFunction.SendErrorToText(ex);
                    vsLogMessage.Add(errorcode_text, error_code);
                    vsLogMessage.Add(title_text, Warning_Noti);
                    vsLogMessage.Add(msg_text, ex.Message);
                }
            }
            return Json(vsLogMessage, JsonRequestBehavior.DenyGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserAccessRight_Update([DataSourceRequest]DataSourceRequest poRequest, RolePermission_REC poRecord)
        {
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            string _ResultMsg = string.Empty;
            bool RoleFunctionMap = true;

            if (poRecord != null)
            {
                try
                {
                    UserModel oClass = new UserModel();

                    Role_REC objRole = new Role_REC();
                    objRole.RoleName = poRecord.RoleName;
                    objRole.Description = poRecord.RoleName;
                    objRole.CreatedBy = System.Web.HttpContext.Current.Session["UserID"].ToString();
                    objRole.CreatedDate = DateTime.Now;

                    if (!oClass.DeleteOnlyRolePermission(objRole))
                    {
                        vsLogMessage.Add(errorcode_text, error_code);
                        vsLogMessage.Add(title_text, Warning_Noti);
                        vsLogMessage.Add(msg_text, oClass.ErrorMessage);
                    }
                    else
                    {
                        if (poRecord.MenuID.ToString().Contains("All") && poRecord.MobileID != "0")
                        {
                            //Insert One Record For All
                            RolePermission_REC RolePermissionRecord = new RolePermission_REC();

                            #region Insert All
                            RolePermissionRecord.CreatedBy = System.Web.HttpContext.Current.Session["UserID"].ToString();
                            RolePermissionRecord.RoleName = poRecord.RoleName;
                            if (!oClass.AllInfoInsertRolePermission(RolePermissionRecord))
                            {
                                RoleFunctionMap = false;
                            }
                            #endregion
                        }
                        else if (poRecord.MenuID.ToString().Contains("All") && (poRecord.MobileID == "0" || poRecord.MobileID == null))
                        {
                            //Insert One Record For All except mobile
                            RolePermission_REC RolePermissionRecord = new RolePermission_REC();

                            #region Insert All except Mobile
                            RolePermissionRecord.CreatedBy = System.Web.HttpContext.Current.Session["UserID"].ToString();
                            RolePermissionRecord.RoleName = poRecord.RoleName;
                            if (!oClass.AllInfoInsertRolePermissionExceptMobile(RolePermissionRecord))
                            {
                                RoleFunctionMap = false;
                            }
                            #endregion
                        }
                        else
                        {
                            ////Split Chosen Menu String    
                            //Get Menu data
                            MenuModel menuModel = new MenuModel();
                            var vMenuResult = menuModel.GetList().ToList();

                            List<RolePermission_REC> PermissionList = new List<RolePermission_REC>();
                            RolePermission_REC ObjPermission;

                            var MenuArray = poRecord.MenuID.Split(',');

                            foreach (Menu_REC menu in vMenuResult)
                            {
                                if (MenuArray.Any(item => item == menu.MenuID))
                                {
                                    //check data in object listing
                                    var checkduplicate = PermissionList.Where(Permission => Permission.MenuID == menu.MenuID).ToList();
                                    if (checkduplicate.Count == 0)
                                    {
                                        ObjPermission = new RolePermission_REC();
                                        ObjPermission.RoleName = poRecord.RoleName;
                                        ObjPermission.MenuID = menu.MenuID;
                                        ObjPermission.IsAllowAdd = false;
                                        ObjPermission.IsAllowEdit = false;
                                        ObjPermission.IsAllowDelete = false;
                                        if (menu.MenuID == "M0" || menu.MenuID == "M1" || menu.MenuID == "M2" || menu.MenuID == "M3" || menu.MenuID == "M1P2" || menu.MenuID == "M1P5")
                                        {
                                            ObjPermission.IsAllowAdd = true;
                                            ObjPermission.IsAllowEdit = true;
                                            ObjPermission.IsAllowDelete = true;
                                        }
                                        else
                                        {
                                            string[] child;
                                            child = MenuArray.Where(b => b.ToString().Contains(menu.MenuName)).ToArray<string>();
                                            if (Array.Exists(child, element => element.Contains("Insert")))
                                                ObjPermission.IsAllowAdd = true;
                                            if (Array.Exists(child, element => element.Contains("Update")))
                                                ObjPermission.IsAllowEdit = true;
                                            if (Array.Exists(child, element => element.Contains("Delete")))
                                                ObjPermission.IsAllowDelete = true;
                                        }
                                        PermissionList.Add(ObjPermission);
                                    }
                                }

                                if ((poRecord.MobileID.Trim() != "0") || (poRecord.MobileID.Trim() != null))
                                {
                                    if (poRecord.MobileID == menu.MenuID)
                                    {
                                        ObjPermission = new RolePermission_REC();
                                        ObjPermission.RoleName = poRecord.RoleName;
                                        ObjPermission.MenuID = menu.MenuID;
                                        ObjPermission.IsAllowAdd = false;
                                        ObjPermission.IsAllowEdit = false;
                                        ObjPermission.IsAllowDelete = false;
                                        PermissionList.Add(ObjPermission);
                                    }
                                }
                            }



                            if (!oClass.InsertRolePermission(PermissionList))
                            {
                                RoleFunctionMap = false;
                            }
                        }

                        if (RoleFunctionMap)
                        {
                            _ResultMsg = GlobalFunction.GetStatus_MsgByKey("Data_Update_Success");
                            vsLogMessage.Add(errorcode_text, no_errorcode);
                            vsLogMessage.Add(title_text, success_title);
                            vsLogMessage.Add(msg_text, _ResultMsg);
                        }
                        else
                        {
                            // delete UserRole
                            _ResultMsg = GlobalFunction.GetStatus_MsgByKey("Data_Update_Fail");
                            vsLogMessage.Add(errorcode_text, error_code);
                            vsLogMessage.Add(title_text, Warning_Noti);
                            vsLogMessage.Add(msg_text, _ResultMsg);
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobalFunction.SendErrorToText(ex);
                    vsLogMessage.Add(errorcode_text, error_code);
                    vsLogMessage.Add(title_text, Warning_Noti);
                    vsLogMessage.Add(msg_text, ex.Message);

                }

            }
            return Json(vsLogMessage, JsonRequestBehavior.DenyGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserAccessRight_Destroy([DataSourceRequest]DataSourceRequest poRequest, Role_REC poRecord)
        {
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            string _ResultMsg = string.Empty;

            if (poRecord != null)
            {
                UserModel userModel = new UserModel();
                List<FindDataCount> vResult = new List<FindDataCount>();
                vResult = userModel.CheckRoleNameCurrentInUse(poRecord.RoleName).ToList();

                if (vResult[0].Rcount > 0)
                {
                    _ResultMsg = GlobalFunction.GetStatus_MsgByKey("UserAcces_Delete_Inused");
                    vsLogMessage.Add(errorcode_text, error_code);
                    vsLogMessage.Add(title_text, Warning_Noti);
                    vsLogMessage.Add(msg_text, _ResultMsg);
                }
                else
                {
                    UserRoleModel oClass = new UserRoleModel();
                    if (!oClass.DeleteAllUserRole(poRecord))
                    {
                        vsLogMessage.Add(errorcode_text, error_code);
                        vsLogMessage.Add(title_text, Warning_Noti);
                        vsLogMessage.Add(msg_text, oClass.ErrorMessage);
                    }
                    else
                    {
                        _ResultMsg = GlobalFunction.GetStatus_MsgByKey("Data_Delete_Success");
                        vsLogMessage.Add(errorcode_text, no_errorcode);
                        vsLogMessage.Add(title_text, success_title);
                        vsLogMessage.Add(msg_text, _ResultMsg);
                    }
                }
            }
            return Json(vsLogMessage, JsonRequestBehavior.DenyGet);
        }
        #endregion
    }
}