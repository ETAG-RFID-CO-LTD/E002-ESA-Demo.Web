using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using EagleServicesWebApp.Models.RollRoyceSystem;
using EagleServicesWebApp.Components;
using System.Configuration;
using EagleServicesWebApp.Models;
using EagleServicesWebApp.Models;

namespace EagleServicesWebApp.Controllers
{
    public class LoginController : Controller
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
        string LogIn_Failed = GlobalFunction.GetStatus_MsgByKey("LogIn_Failed");
        string success_Noti = GlobalFunction.GetStatus_MsgByKey("success_Noti");
        string LogIn_Success = GlobalFunction.GetStatus_MsgByKey("LogIn_Success");
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Home()
        {
            try
            {
                string pas = System.Web.HttpContext.Current.Session["Password"] as String;
                string Defaultpas = System.Web.HttpContext.Current.Session["Defaultpwd"] as String;

                if (!string.IsNullOrEmpty(pas))
                {
                    if (pas == Defaultpas)
                    {
                        //Browse to Change Password Alert Page

                        return RedirectToAction("Prompt", "Login");
                    }
                    else
                        return RedirectToAction("Home", "Dashboard");
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

        #region LogIn Section
        public ActionResult Auth(string kdpUserId, string kdpPassword)
        {
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            string _ResultMsg = string.Empty;
            kdpUserId = kdpUserId.ToLower().Trim();
            kdpPassword = kdpPassword.ToLower().Trim();
            if ((kdpUserId == string.Empty) && (kdpPassword == string.Empty))
            {
                _ResultMsg = GlobalFunction.GetStatus_MsgByKey("LogIn_Empty");

                vsLogMessage.Add(errorcode_text, error_code);
                vsLogMessage.Add(title_text, LogIn_Failed);
                vsLogMessage.Add(msg_text, _ResultMsg);
            }
            else if ((kdpUserId == string.Empty))
            {
                _ResultMsg = GlobalFunction.GetStatus_MsgByKey("LogIn_EmptyId");
                vsLogMessage.Add(errorcode_text, error_code);
                vsLogMessage.Add(title_text, LogIn_Failed);
                vsLogMessage.Add(msg_text, _ResultMsg);
            }
            else if ((kdpPassword == string.Empty))
            {
                _ResultMsg = GlobalFunction.GetStatus_MsgByKey("LogIn_EmptyPwd");

                vsLogMessage.Add(errorcode_text, error_code);
                vsLogMessage.Add(title_text, LogIn_Failed);
                vsLogMessage.Add(msg_text, _ResultMsg);
            }
            else
            {
                #region Check With DB For LogIn
                try
                {
                    //User ID and Password field will be empty. 
                    string Password = GlobalFunction.CreateMD5Hash(kdpPassword);
                    User_REC Exist1 = getUser(kdpUserId, Password);
                    if (Exist1 != null)
                    {
                        if (!Exist1.isActive)
                        {
                            _ResultMsg = GlobalFunction.GetStatus_MsgByKey("LogIn_InActive");
                            vsLogMessage.Add(errorcode_text, error_code);
                            vsLogMessage.Add(title_text, LogIn_Failed);
                            vsLogMessage.Add(msg_text, _ResultMsg);
                        }
                        else
                        {
                            #region Create Default encrypted DefaultPassword
                            string _pwd = "Password!23";
                            string _Defaultpwd = _pwd.ToLower();
                            string Default = GlobalFunction.CreateMD5Hash(kdpUserId.ToLower() + _Defaultpwd);
                            #endregion


                            #region Session Storage

                            //System.Web.HttpContext.Current.Session["ID"] = Exist1.ID;
                            System.Web.HttpContext.Current.Session["UserID"] = Exist1.UserID;
                            System.Web.HttpContext.Current.Session["FirstName"] = Exist1.FirstName;
                            System.Web.HttpContext.Current.Session["LastName"] = Exist1.LastName;
                            System.Web.HttpContext.Current.Session["Password"] = Exist1.Password;
                            System.Web.HttpContext.Current.Session["Defaultpwd"] = Default;
                            System.Web.HttpContext.Current.Session["isAdmin"] = Exist1.isAdmin;
                            System.Web.HttpContext.Current.Session["RoleName"] = Exist1.RoleName;

                            System.Web.HttpContext.Current.Session["PagerSettingSize"] = 30;

                            _ResultMsg = GlobalFunction.GetStatus_MsgByKey("LogIn_redirect");

                            vsLogMessage.Add(errorcode_text, no_errorcode);
                            vsLogMessage.Add(title_text, LogIn_Success);
                            vsLogMessage.Add(msg_text, _ResultMsg);
                            #endregion
                        }
                    }
                    else
                    {
                        _ResultMsg = GlobalFunction.GetStatus_MsgByKey("LogIn_Invalid");
                        vsLogMessage.Add(errorcode_text, error_code);
                        vsLogMessage.Add(title_text, LogIn_Failed);
                        vsLogMessage.Add(msg_text, _ResultMsg);
                    }
                }
                catch (Exception exc)
                {
                    GlobalFunction.SendErrorToText(exc);

                    vsLogMessage.Add(errorcode_text, error_code);
                    vsLogMessage.Add(title_text, Error_text);
                    vsLogMessage.Add(msg_text, "Message : " + exc.Message + " Source : " + exc.Source);
                }
                #endregion
            }


            return Json(vsLogMessage, JsonRequestBehavior.DenyGet);
        }
        #endregion

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdatePwd([DataSourceRequest] DataSourceRequest poRequest, User_REC poRecord)
        {
            string pas = System.Web.HttpContext.Current.Session["Password"] as String;
            string _ResultMsg = string.Empty;

            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            if ((poRecord != null))
            {
                poRecord.UpdatedBy = poRecord.UserID;
                UserModel oClass = new UserModel();
                poRecord.Password = poRecord.Password;
                poRecord.ConfirmPassowrd = poRecord.ConfirmPassowrd;
                string _Defpwd = "Password!23";
                poRecord.LogQuery = poRecord.LogQuery = "Update password to tblUser Table==>Change Password="
                    + poRecord.ConfirmPassowrd + " By UserID=" + poRecord.UserID
                    + ",RoleName=" + poRecord.RoleName + ",isActive=" + poRecord.isActive
                    + ",isadmin=" + poRecord.isAdmin + ",Status=" + poRecord.Status;

                if (poRecord.Password == _Defpwd)
                {
                    _ResultMsg = GlobalFunction.GetStatus_MsgByKey("Equal_DefPwd");
                    vsLogMessage.Add(errorcode_text, error_code);
                    vsLogMessage.Add(title_text, Warning_Noti);
                    vsLogMessage.Add(msg_text, _ResultMsg);
                }
                else
                {
                    #region Change Default pwd
                    if (!oClass.UpdatePwd(poRecord))
                    {
                        vsLogMessage.Add(errorcode_text, error_code);
                        vsLogMessage.Add(title_text, Warning_Noti);
                        vsLogMessage.Add(msg_text, oClass.ErrorMessage);
                    }
                    else
                    {
                        System.Web.HttpContext.Current.Session["Password"] = poRecord.Password;
                        _ResultMsg = GlobalFunction.GetStatus_MsgByKey("Pwd_UpdateInfo");
                        vsLogMessage.Add(errorcode_text, no_errorcode);
                        vsLogMessage.Add(title_text, success_Noti);
                        vsLogMessage.Add(msg_text, _ResultMsg);
                    }
                }
            }
            return Json(vsLogMessage, JsonRequestBehavior.DenyGet);
        }
        #endregion

        #region Change Default password page
        public ActionResult Prompt()
        {
            try
            {
                string password = System.Web.HttpContext.Current.Session["Password"] as String;
                if (!string.IsNullOrEmpty(password))
                {
                    #region LogIn Message From XML
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

        #endregion

        #region Logout Section
        public ActionResult Clear()
        {
            #region Clear Session and Browse Login Page
            //before log out clear all Section
            System.Web.HttpContext.Current.Session.Clear();
            return RedirectToAction("Index", "Login");
            #endregion
        }

        #endregion

        #region Customize Function
        public static User_REC getUser(string UserId, string Password)
        {

            UserModel oClass = new UserModel();
            var vResult = oClass.GetList().ToList();
            return vResult.FindAll(f => f.UserID.ToLower() == UserId.ToLower() && f.Password == Password).ToList().SingleOrDefault();
        }
        #endregion

    }
}
