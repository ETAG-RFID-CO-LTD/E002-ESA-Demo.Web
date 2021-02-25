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
using EagleServicesWebApp.Models;
using System.IO;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;

using System.Globalization;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Data.OleDb;
using System.Data;
using System.Text.RegularExpressions;
using EagleServicesWebApp.Models.Item;

namespace EagleServicesWebApp.Controllers
{
    public class AssetController : Controller
    {

        private static Mutex mutex = new Mutex();
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        #region Read From XML
        string ScrapFormNoEmpty_Noti = GlobalFunction.GetStatus_MsgByKey("ScrapFormNoEmpty_Noti");
        string sLogFilePath = ConfigurationManager.AppSettings.Get("LogFilePath");
        string errorcode_text = GlobalFunction.GetStatus_MsgByKey("errorcode_text");
        string title_text = GlobalFunction.GetStatus_MsgByKey("title_text");
        string msg_text = GlobalFunction.GetStatus_MsgByKey("msg_text");
        string no_errorcode = GlobalFunction.GetStatus_MsgByKey("no_errorcode");
        string error_code = GlobalFunction.GetStatus_MsgByKey("error_code");
        string Warning_Noti = GlobalFunction.GetStatus_MsgByKey("Warning_Noti");
        string Error_text = GlobalFunction.GetStatus_MsgByKey("Error_text");
        string success_Noti = GlobalFunction.GetStatus_MsgByKey("success_Noti");
        string form_validation_text = GlobalFunction.GetStatus_MsgByKey("form_validation_text");
        string success_title = GlobalFunction.GetStatus_MsgByKey("success_title");
        string failed_title = GlobalFunction.GetStatus_MsgByKey("failed_title");

        string engineSerialNo = GlobalFunction.GetStatus_MsgByKey("engineSerialNo");
        string engineModel = GlobalFunction.GetStatus_MsgByKey("engineModel");
        string engineCSN = GlobalFunction.GetStatus_MsgByKey("engineCSN");
        string engineTSN = GlobalFunction.GetStatus_MsgByKey("engineTSN");

        string item = GlobalFunction.GetStatus_MsgByKey("Item");
        string IPCReference = GlobalFunction.GetStatus_MsgByKey("IPCReference");
        string Nomenclature = GlobalFunction.GetStatus_MsgByKey("Nomenclature");
        string PartType = GlobalFunction.GetStatus_MsgByKey("PartType");
        string PwelStandard = GlobalFunction.GetStatus_MsgByKey("PwelStandard");
        string PartStatus = GlobalFunction.GetStatus_MsgByKey("PartStatus");

        string ReqColumns = GlobalFunction.GetStatus_MsgByKey("ReqColumn");
        string ExcelData_Empty = GlobalFunction.GetStatus_MsgByKey("ExcelData_Empty");
        string Invalid_date = GlobalFunction.GetStatus_MsgByKey("Invalid_date");
        string DataType_Invalid = GlobalFunction.GetStatus_MsgByKey("DataTypeInvalid");
        string Invalid_Scrap = GlobalFunction.GetStatus_MsgByKey("Invalid_Scrap");
        string Invalid_CustomerFunding = GlobalFunction.GetStatus_MsgByKey("Invalid_CustomerFunding");
        string Excel_Data_Insert_Success = GlobalFunction.GetStatus_MsgByKey("Excel_Data_Insert_Success");
        string GeneralTable_Auto_Insert_Fail = GlobalFunction.GetStatus_MsgByKey("GeneralTable_Auto_Insert_Fail");
        string AssetNo_NeedToCheck = GlobalFunction.GetStatus_MsgByKey("AssetNo_NeedToCheck");
        string Excel_Data_Insert_Fail = GlobalFunction.GetStatus_MsgByKey("Excel_Data_Insert_Fail");
        string Cap_date = GlobalFunction.GetStatus_MsgByKey("Invalid_DateFormat");
        string invalid_qty = GlobalFunction.GetStatus_MsgByKey("Invalid_Qty");
        string invalidzero_qty = GlobalFunction.GetStatus_MsgByKey("InvalidQtyZero");
        string invalid_sdg = GlobalFunction.GetStatus_MsgByKey("Invalid_SDG");
        string invalid_AssetStatus = GlobalFunction.GetStatus_MsgByKey("Invalid_AssetStatus");
        string invalid_AssetIndicator = GlobalFunction.GetStatus_MsgByKey("Invalid_AssetIndicator");
        string DuplicationPass = GlobalFunction.GetStatus_MsgByKey("DuplicationPass");
        string DuplicationFail = GlobalFunction.GetStatus_MsgByKey("DuplicationFail");
        string NoData_InExcel = GlobalFunction.GetStatus_MsgByKey("NoData_InExcel");
        string Excel_Data_Invalid = GlobalFunction.GetStatus_MsgByKey("Excel_Data_Invalid");
        string AssetNo_Duplicated = GlobalFunction.GetStatus_MsgByKey("AssetNo_Duplicated");
        string History_Action_AssetUpdate = GlobalFunction.GetStatus_MsgByKey("History_Action_AssetUpdate");
        string SubAssetNumberQuantity_Noti = GlobalFunction.GetStatus_MsgByKey("SubAssetNumberQuantity_Noti");
        string checkexcel = GlobalFunction.GetStatus_MsgByKey("checkexcel");
        string AssetNumberdigits = GlobalFunction.GetStatus_MsgByKey("AssetNumberdigits");
        string MissingColumn = GlobalFunction.GetStatus_MsgByKey("MissingColumn");
        #endregion

        // GET: Asset
        public ActionResult Index()
        {
            return View();
        }

        #region Data Import
        public ActionResult DataImport()
        {
            try
            {
                string password = System.Web.HttpContext.Current.Session["Password"] as String;
                if (!string.IsNullOrEmpty(password))
                {
                    #region Button Access Right
                    string RoleName = System.Web.HttpContext.Current.Session["RoleName"] as String;
                    RolePermissionModel rolePermission = new RolePermissionModel();
                    var accesibleRoles = rolePermission.GetMenuPermissionByMenuName(RoleName, "DataImport").ToList().SingleOrDefault();
                    if (accesibleRoles == null)
                    {
                        accesibleRoles.IsAllowAdd = false;
                        accesibleRoles.IsAllowEdit = false;
                        accesibleRoles.IsAllowDelete = false;
                    }

                    System.Web.HttpContext.Current.Session["Roles"] = accesibleRoles;
                    #endregion

                    #region Read From XML 
                    ErrorMessageModel Model = new ErrorMessageModel();
                    ViewData["CommonUseMsgSelect"] = Model.CommonUseMsgSelect();

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

        [HttpPost]
        public ActionResult SaveExcelInTempFolder([DataSourceRequest]DataSourceRequest poRequest, string extension)//error file name can duplicate and delete excel file from temp folder
        {
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            string _ResultMsg = string.Empty;

            if (extension == "Invalid")
            {
                _ResultMsg = GlobalFunction.GetStatus_MsgByKey("ImportFileInValid");
                vsLogMessage.Add(errorcode_text, error_code);
                vsLogMessage.Add(title_text, Warning_Noti);
                vsLogMessage.Add(msg_text, _ResultMsg);
            }
            else
            {
                try
                {
                    if (Request.Files.Count > 0)
                    {

                        //folder perssion
                        DirectorySecurity sec = Directory.GetAccessControl(Server.MapPath("~/Temp/"));
                        SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
                        sec.AddAccessRule(new FileSystemAccessRule(everyone, FileSystemRights.Modify | FileSystemRights.Synchronize, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                        Directory.SetAccessControl(Server.MapPath("~/Temp/"), sec);

                        HttpFileCollectionBase files = Request.Files;
                        string tempfname;
                        string fname;

                        for (int i = 0; i < files.Count; i++)
                        {
                            HttpPostedFileBase file = files[i];
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                tempfname = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                tempfname = file.FileName;
                            }

                            if (extension == "xls")
                            {
                                fname = tempfname.Replace(".xls", "").Trim() + DateTime.Now.ToString("ddMMyyyyhhmmss") + "." + extension;
                                fname = Path.Combine(Server.MapPath("~/Temp/"), fname);

                                int count = 1;
                                while (System.IO.File.Exists(fname))
                                {
                                    string tempFileName = string.Format("{0}({1})", Path.GetFileNameWithoutExtension(fname), count++) + "." + extension;
                                    fname = Path.Combine(Server.MapPath("~/Temp/"), tempFileName);
                                }
                                ViewBag.ExcelName = Path.GetFileNameWithoutExtension(fname) + "." + extension;
                                file.SaveAs(fname);
                            }
                            else if (extension == "xlsx")
                            {
                                fname = tempfname.Replace(".xlsx", "").Trim() + DateTime.Now.ToString("ddMMyyyyhhmmss") + "." + extension;
                                fname = Path.Combine(Server.MapPath("~/Temp/"), fname);

                                int count = 1;
                                while (System.IO.File.Exists(fname))
                                {
                                    string tempFileName = string.Format("{0}({1})", Path.GetFileNameWithoutExtension(fname), count++) + "." + extension;
                                    fname = Path.Combine(Server.MapPath("~/Temp/"), tempFileName);
                                }
                                ViewBag.ExcelName = Path.GetFileNameWithoutExtension(fname) + "." + extension;
                                file.SaveAs(fname);
                            }
                        }
                    }

                    vsLogMessage.Add(errorcode_text, no_errorcode);
                    vsLogMessage.Add(title_text, success_Noti);
                    vsLogMessage.Add(msg_text, ViewBag.ExcelName);
                }
                catch (Exception exc)
                {
                    GlobalFunction.SendErrorToText(exc);
                    vsLogMessage.Add(errorcode_text, error_code);
                    vsLogMessage.Add(title_text, Error_text);
                    vsLogMessage.Add(msg_text, "Message : " + exc.Message + " Source : " + exc.Source);
                }
            }
            return Json(vsLogMessage, JsonRequestBehavior.DenyGet);
        }

        public ActionResult HeaderDataToShow([DataSourceRequest]DataSourceRequest poRequest, string extension, string excelName)
        {

            #region Variables
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            string connstring = "";
            string pdfPath = Server.MapPath("~/Temp");
            string path = "";
            path = Path.Combine(pdfPath, excelName);
            ViewBag.ExcelName = excelName;
            ViewBag.ResultMessage = "";
            #endregion

            try
            {

                #region Excel file Extension Check and ConnectionString 
                if (extension == "xls")
                {
                    //connstring = "Provider=Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0; HDR=Yes;IMEX=1;'";
                    connstring = "Provider=Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0; HDR=No;IMEX=1;'";
                }
                else if (extension == "xlsx")
                {
                    //connstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0 Xml;HDR={1};IMEX=1;'";
                    connstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0 Xml;HDR=No;IMEX=1;'";
                }

                #endregion
                #region Header Engine
                //List<Engine_REC> EngineDataListing = GoEngineDataToPopup(connstring);

                var StrMessage = "";
                bool Empty = false;

                //if (EngineDataListing[0].SerialNo == "")
                //{
                //    StrMessage += engineSerialNo + ",";
                //    Empty = true;
                //}

                //if (EngineDataListing[0].Model == "")
                //{
                //    StrMessage += engineModel + ",";
                //    Empty = true;
                //}

                //if (EngineDataListing[0].CSN == "")
                //{
                //    StrMessage += engineCSN + ",";
                //    Empty = true;
                //}

                //if (EngineDataListing[0].TSN == "")
                //{
                //    StrMessage += engineTSN + ",";
                //    Empty = true;
                //}
                #endregion
                string[] columName = new string[] { "ITEM#", "IPC REFERENCE", "NOMENCLATURE", "PART TYPE", "PWEL STANDARD", "PART STATUS", "VENDOR PN", "PW PN", "SN", "REMARKS" };

                DataTable dt_xml = GoDataToPopup(connstring);

                if (dt_xml.Rows.Count > 0 && dt_xml.Columns.Count > 0)
                {
                    #region Excel Header Columns Declaration

                    bool GoR = true;
                    bool Go = true;
                    bool GoNext = true;

                    //List<string> ReqColumn = new List<string>();
                    //List<string> ExcelColumn = new List<string>();

                    //ReqColumn.Add(excel_CostCenter);
                    //ReqColumn.Add(excel_SubLocPOCName);
                    //ReqColumn.Add(excel_SubLocPOCDesignation);

                    if (columName.Count() != dt_xml.Columns.Count)
                    {
                        vsLogMessage.Add(errorcode_text, error_code);
                        vsLogMessage.Add(title_text, Warning_Noti);
                        vsLogMessage.Add(msg_text, " %" + "Number of column is invalid");
                        Go = false;
                        Go = false;
                        GoNext = false;
                    }
                    else
                    {
                        int j = 0;
                        foreach (string citem in columName)
                        {
                            if (dt_xml.Columns[j].ColumnName != citem)
                            {
                                Boolean columnExists = dt_xml.Columns.Contains(citem);

                                vsLogMessage.Add(errorcode_text, error_code);
                                vsLogMessage.Add(title_text, Warning_Noti);
                                vsLogMessage.Add(msg_text, " %" + "Column order is invalid.");
                                Go = false;
                                GoNext = false;
                                break;
                            }
                            j++;
                        }
                    }

                    #endregion


                    //for (int i = 0; i < dt_xml.Columns.Count; i++)
                    //{
                    //   ExcelColumn.Add(dt_xml.Columns[i].ToString().Trim());
                    //}

                    //string[] s3 = ReqColumn.Except(ExcelColumn).ToArray();
                    //string Req = "";


                    //if (s3.Length > 0)//all column contain check
                    //{
                    //   for (int x = 0; x < s3.Length; x++)
                    //   {
                    //      if (x == s3.Length - 1)
                    //      {
                    //         Req += s3[x].ToString();
                    //      }
                    //      else
                    //      {
                    //         Req += s3[x].ToString() + ",";
                    //      }
                    //   }
                    //   GoR = false;
                    //   ViewBag.ResultMessage += Req + ReqColumns + "&";
                    //}
                    if (Go == true)//column space and Invalid data in Excel 
                    {
                        try
                        {
                            #region Variable Declaration

                            var InvalidMessage = "";
                            bool InvalidData = false;

                            int temp0 = 0;
                            int temp1 = 0;
                            int temp2 = 0;
                            int temp3 = 0;
                            int temp4 = 0;
                            int temp5 = 0;
                            int tempInvalid1 = 0;

                            #endregion

                            #region Excel DataFormat Check
                            GlobalFunction.WriteToFile("------------------------Read Start------------------------------------  ");
                            for (int x = 0; x < dt_xml.Rows.Count; x++)
                            {
                                for (int y = 0; y < dt_xml.Columns.Count; y++)
                                {
                                    if (y == 0)
                                    {
                                        if (temp0 == 0)
                                        {
                                            if (dt_xml.Rows[x][y].ToString().Trim() == "")
                                            {
                                                temp0++;
                                                StrMessage += item + ",";
                                                Empty = true;
                                            }
                                        }
                                    }
                                    else if (y == 1)
                                    {
                                        if (temp1 == 0)
                                        {
                                            if (dt_xml.Rows[x][y].ToString().Trim() == "")
                                            {
                                                temp1++;
                                                StrMessage += IPCReference + ",";
                                                Empty = true;
                                            }
                                        }
                                    }
                                    else if (y == 2)
                                    {
                                        if (temp2 == 0)
                                        {
                                            if (dt_xml.Rows[x][y].ToString().Trim() == "")
                                            {
                                                temp2++;
                                                StrMessage += Nomenclature + ",";
                                                Empty = true;
                                            }
                                        }
                                    }
                                    else if (y == 3)
                                    {
                                        if (temp3 == 0)
                                        {
                                            if (dt_xml.Rows[x][y].ToString().Trim() == "")
                                            {
                                                temp3++;
                                                StrMessage += PartType + ", ";
                                                Empty = true;
                                            }
                                        }
                                    }
                                    else if (y == 4)
                                    {
                                        if (temp4 == 0)
                                        {
                                            if (dt_xml.Rows[x][y].ToString().Trim() == "")
                                            {
                                                temp4++;
                                                StrMessage += PwelStandard + ", ";
                                                Empty = true;
                                            }
                                        }
                                        if (dt_xml.Rows[x][y].ToString().Trim() != "")
                                        {
                                            if (tempInvalid1 == 0)
                                            {
                                                if (dt_xml.Rows[x][y].ToString().Trim().ToLower() != "yes" && dt_xml.Rows[x][y].ToString().Trim().ToLower() != "no")
                                                {
                                                    tempInvalid1++;
                                                    InvalidMessage += invalid_AssetStatus + " & ";
                                                    InvalidData = true;
                                                }
                                            }
                                        }
                                    }
                                    else if (y == 5)
                                    {
                                        if (temp5 == 0)
                                        {
                                            if (dt_xml.Rows[x][y].ToString().Trim() == "")
                                            {
                                                temp5++;
                                                StrMessage += PartStatus + ", ";
                                                Empty = true;
                                            }
                                        }
                                    }
                                }
                            }
                            GlobalFunction.WriteToFile("------------------------Read End ------------------------------------  ");
                            #endregion

                            if (Empty == true)
                            {
                                Go = false;
                                ViewBag.ResultMessage += StrMessage.TrimEnd(',') + ExcelData_Empty + " & ";
                            }
                            if (InvalidData == true)
                            {
                                Go = false;
                                ViewBag.ResultMessage += InvalidMessage.TrimEnd(','); //+ "&";//+ Excel_Data_Invalid
                            }
                        }
                        catch (Exception exc)
                        {
                            GlobalFunction.SendErrorToText(exc);
                        }
                    }
                    if (GoNext == true) //duplicate RFID ID
                    {
                        #region Variable Declaration

                        List<string> ItemList = new List<string>();
                        string Item_duplicate = "";
                        #endregion

                        for (int x = 0; x < dt_xml.Rows.Count; x++)
                        {
                            for (int y = 0; y < dt_xml.Columns.Count; y++)
                            {
                                if (y == 0)
                                {
                                    if (dt_xml.Rows[x][y].ToString().Trim() != "")
                                        ItemList.Add(dt_xml.Rows[x][y].ToString().Trim());
                                }
                            }
                        }

                        var duplicates = ItemList.GroupBy(x => x).Any(g => g.Count() > 1);

                        List<String> e = ItemList.GroupBy(x => x)
                                      .Where(g => g.Count() > 1)
                                      .Select(g => g.Key)
                                      .ToList();

                        if (e.Count > 0)
                        {
                            for (int x = 0; x < e.Count; x++)
                            {
                                if (x == e.Count - 1)
                                {
                                    Item_duplicate += e[x].ToString();
                                }
                                else
                                {
                                    Item_duplicate += e[x].ToString() + ", ";
                                }
                            }

                            ViewBag.ResultMessage += "ITEM " + "( " + Item_duplicate + " )" + " & " + AssetNo_Duplicated + " & ";
                        }

                        if (duplicates == false)
                        {
                            if (Go == true && GoR == true && GoNext == true)
                            {
                                //all pass

                                System.Web.HttpContext.Current.Session["extension"] = extension;
                                System.Web.HttpContext.Current.Session["excelName"] = excelName;

                                vsLogMessage.Add(errorcode_text, no_errorcode);
                                vsLogMessage.Add(title_text, success_Noti);
                                vsLogMessage.Add(msg_text, DuplicationPass + "%" + ViewBag.ExcelName);
                            }
                            else
                            {
                                // no duplicate
                                while (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }
                                vsLogMessage.Add(errorcode_text, error_code);
                                vsLogMessage.Add(title_text, Warning_Noti);
                                vsLogMessage.Add(msg_text, " %" + ViewBag.ResultMessage);
                            }
                        }
                        else
                        {
                            while (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }


                            if (Go == true && GoR == true)
                            {
                                vsLogMessage.Add(errorcode_text, error_code);
                                vsLogMessage.Add(title_text, Warning_Noti);
                                vsLogMessage.Add(msg_text, DuplicationFail + "%" + ViewBag.ResultMessage);
                            }
                            else
                            {

                                vsLogMessage.Add(errorcode_text, error_code);
                                vsLogMessage.Add(title_text, Warning_Noti);
                                vsLogMessage.Add(msg_text, DuplicationFail + "%" + ViewBag.ResultMessage);
                            }
                        }
                    }
                }
                else
                {
                    while (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    vsLogMessage.Add(errorcode_text, error_code);
                    vsLogMessage.Add(title_text, Warning_Noti);
                    vsLogMessage.Add(msg_text, " %" + NoData_InExcel);
                }
            }
            catch (Exception exc)
            {
                #region Add By WWN
                //while (System.IO.File.Exists(path))
                //{
                //    System.IO.File.Delete(path);
                //}

                GlobalFunction.SendErrorToText(exc);
                #endregion

                if (exc.Message.ToString() == "The source contains no DataRows.")
                {
                    vsLogMessage.Add(errorcode_text, error_code);
                    vsLogMessage.Add(title_text, Warning_Noti);
                    vsLogMessage.Add(msg_text, " %" + NoData_InExcel);
                }
                else if (exc.Message.ToString().ToLower().Contains("cannot find column"))
                {

                    vsLogMessage.Add(errorcode_text, error_code);
                    vsLogMessage.Add(title_text, Warning_Noti);
                    vsLogMessage.Add(msg_text, " %" + MissingColumn);
                }
                else
                {
                    vsLogMessage.Add(errorcode_text, error_code);
                    vsLogMessage.Add(title_text, Warning_Noti);
                    vsLogMessage.Add(msg_text, "%" + "Message : " + exc.Message + " Source : " + exc.Source);

                }


            }

            return Json(vsLogMessage, JsonRequestBehavior.DenyGet);
        }

        public static DataTable GoDataToPopup(string excel_conn)
        {
            System.Data.DataTable dt = new DataTable();
            try
            {
                string Excel_conn = excel_conn;
                dt = ReadExcelToTable(Excel_conn);
                foreach (DataRow row in dt.Rows)
                {
                    if (String.IsNullOrEmpty(row[0].ToString()) && String.IsNullOrEmpty(row[1].ToString())
                       && String.IsNullOrEmpty(row[6].ToString()) && String.IsNullOrEmpty(row[7].ToString())
                       && String.IsNullOrEmpty(row[9].ToString()) && String.IsNullOrEmpty(row[10].ToString()) && String.IsNullOrEmpty(row[11].ToString()))
                        row.Delete();
                }
                dt.AcceptChanges();
            }
            catch (Exception exc)
            {
                GlobalFunction.SendErrorToText(exc);
                throw exc;
            }

            return dt;
        }

        public static DataTable ReadExcelToTable(string excelConn)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(excelConn))
                {
                    conn.Open();

                    System.Data.DataTable sheetsName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
                    string firstSheetName = sheetsName.Rows[0][2].ToString();
                    string sql = string.Format("SELECT * FROM [{0}]", firstSheetName);
                    OleDbDataAdapter ada = new OleDbDataAdapter(sql, conn);
                    DataSet set = new DataSet();
                    ada.Fill(set);
                    conn.Close();
                    conn.Dispose();
                    DataTable tempTBL = new DataTable();
                    DataTable tempAll = new DataTable();
                    DataTable HeaderTBL = new DataTable();

                    HeaderTBL = set.Tables[0].AsEnumerable().CopyToDataTable<DataRow>(); //.Skip(4)

                    int count = 0;
                    foreach (DataRow row in HeaderTBL.Rows)
                    {
                        if (count == 0)
                        {
                            for (int i = 0; i < set.Tables[0].Columns.Count; i++)
                            {
                                tempTBL.Columns.Add(row[i].ToString());
                            }
                            break;
                        }
                        count++;
                    }
                    tempAll = set.Tables[0].AsEnumerable().Skip(1).CopyToDataTable<DataRow>();
                    foreach (DataRow row in tempAll.Rows)
                    {
                        if (String.IsNullOrEmpty(row[0].ToString()) && String.IsNullOrEmpty(row[1].ToString())
                        && String.IsNullOrEmpty(row[2].ToString()) && String.IsNullOrEmpty(row[3].ToString())
                        && String.IsNullOrEmpty(row[4].ToString()) && String.IsNullOrEmpty(row[4].ToString()))
                        {

                        }
                        else
                        {
                            tempTBL.Rows.Add(row.ItemArray);
                        }
                    }
                    return tempTBL;
                }
            }
            catch (OleDbException exc)
            {
                GlobalFunction.SendErrorToText(exc);
                throw exc;
            }
        }

        public static List<Engine_REC> GoEngineDataToPopup(string excel_conn)
        {
            List<Engine_REC> EngineList = new List<Engine_REC>();
            try
            {
                string Excel_conn = excel_conn;
                Engine_REC engine = ReadEngineExcelToTable(Excel_conn);
                EngineList.Add(engine);
            }
            catch (Exception exc)
            {
                GlobalFunction.SendErrorToText(exc);
                throw exc;
            }

            return EngineList;
        }

        public static Engine_REC ReadEngineExcelToTable(string excelConn)
        {
            try
            {
                using (OleDbConnection conn = new OleDbConnection(excelConn))
                {
                    conn.Open();

                    System.Data.DataTable sheetsName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
                    string firstSheetName = sheetsName.Rows[0][2].ToString();

                    string sql = string.Format("SELECT * FROM [{0}]", firstSheetName);
                    OleDbDataAdapter ada = new OleDbDataAdapter(sql, conn);
                    DataSet set = new DataSet();
                    ada.Fill(set);
                    conn.Close();
                    conn.Dispose();

                    DataTable tempAll = new DataTable();

                    tempAll = set.Tables[0].AsEnumerable().Skip(0).CopyToDataTable<DataRow>();

                    Engine_REC engine_REC = new Engine_REC();
                    int count = 0;
                    foreach (DataRow row in tempAll.Rows)
                    {
                        if (count < 4)
                        {
                            if (row[1].ToString().Contains("ENGINE SERIAL NUMBER"))
                                engine_REC.EneSerialNo = row[2].ToString();
                            if (row[1].ToString().Contains("ENGINE MODEL"))
                                engine_REC.Model = row[2].ToString();
                            if (row[1].ToString().Contains("ENGINE CSN"))
                                engine_REC.CSN = row[2].ToString();
                            if (row[1].ToString().Contains("ENGINE TSN"))
                                engine_REC.TSN = row[2].ToString();
                        }
                        count++;
                    }
                    return engine_REC;
                }
            }
            catch (OleDbException exc)
            {
                GlobalFunction.SendErrorToText(exc);
                throw exc;
            }
        }

        public List<Item_REC> ExcelDataBind(string extension, string excelName)
        {
            List<Item_REC> ExcelDataList = new List<Item_REC>();
            try
            {
                string connstring = "";
                string pdfPath = Server.MapPath("~/Temp");
                string path = "";
                path = Path.Combine(pdfPath, excelName);
                if (extension == "xls")
                {
                    //connstring = "Provider=Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0; HDR=Yes;IMEX=1;'";
                    connstring = "Provider=Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0; HDR=No;IMEX=1;'";
                }
                else if (extension == "xlsx")
                {
                    //connstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0 Xml;HDR={1};IMEX=1;'";
                    connstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0 Xml;HDR=No;IMEX=1;'";
                }

                DataTable dt_xml = GoDataToPopup(connstring);

                Item_REC obj;
                for (int x = 0; x < dt_xml.Rows.Count; x++)
                {
                    obj = new Item_REC();
                    for (int y = 0; y < dt_xml.Columns.Count; y++)
                    {
                        if (y == 0)
                            obj.ItemNo = dt_xml.Rows[x][y].ToString().Trim();
                        if (y == 1)
                            obj.IPCRef = dt_xml.Rows[x][y].ToString().Trim();
                        if (y == 2)
                            obj.Nomenclature = dt_xml.Rows[x][y].ToString().Trim();
                        if (y == 3)
                            obj.PartType = dt_xml.Rows[x][y].ToString().Trim();
                        if (y == 4)
                        {
                            if (dt_xml.Rows[x][y].ToString().Trim().ToLower() == "yes")
                            {
                                obj.PwelStd = true;
                                obj.PwelStdStr = "YES";
                            }
                            else
                            {
                                obj.PwelStd = false;
                                obj.PwelStdStr = "NO";
                            }
                        }
                        if (y == 5)
                            obj.PartStatus = dt_xml.Rows[x][y].ToString().Trim();
                        if (y == 6)
                            obj.VPN = dt_xml.Rows[x][y].ToString().Trim();
                        if (y == 7)
                            obj.PWPN = dt_xml.Rows[x][y].ToString().Trim();
                        if (y == 8)
                            obj.SN = dt_xml.Rows[x][y].ToString().Trim();
                        if (y == 9)
                            obj.Remarks = dt_xml.Rows[x][y].ToString().Trim();
                    }

                    ExcelDataList.Add(obj);
                }
                //Session["dtExcel"] = ExcelDataList;
            }
            catch (Exception exc)
            {
                GlobalFunction.SendErrorToText(exc);
                throw exc;
            }
            return ExcelDataList;
        }

        public ActionResult ExcelDataBind_Read([DataSourceRequest]DataSourceRequest poRequest, string extension, string excelName)
        {
            System.Collections.Generic.List<Item_REC> vResult = null;
            try
            {

                //vResult = Session["dtExcel"] as List<Asset_REC>;
                if (string.IsNullOrEmpty(extension) && string.IsNullOrEmpty(excelName))
                {
                    vResult = ExcelDataBind(System.Web.HttpContext.Current.Session["extension"] as String, System.Web.HttpContext.Current.Session["excelName"] as String);
                }
                else
                {
                    vResult = ExcelDataBind(extension, excelName);
                }
            }
            catch (Exception exc)
            {
                string ms = exc.Message;
                GlobalFunction.SendErrorToText(exc);
            }
            return new JsonResult() { Data = vResult.ToDataSourceResult(poRequest), JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BindEngineData([DataSourceRequest]DataSourceRequest poRequest, string extension, string excelName)
        {
            List<Engine_REC> BindEngineData = GetEngineDataToBind(extension, excelName);
            return Json(BindEngineData);
        }

        public List<Engine_REC> GetEngineDataToBind(string extension, string excelName)
        {
            List<Engine_REC> EngineDataList = new List<Engine_REC>();

            try
            {
                string connstring = "";
                string pdfPath = Server.MapPath("~/Temp");
                string path = "";
                path = Path.Combine(pdfPath, excelName);
                if (extension == "xls")
                {
                    //connstring = "Provider=Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0; HDR=Yes;IMEX=1;'";
                    connstring = "Provider=Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0; HDR=No;IMEX=1;'";
                }
                else if (extension == "xlsx")
                {
                    //connstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0 Xml;HDR={1};IMEX=1;'";
                    connstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0 Xml;HDR=No;IMEX=1;'";
                }

                EngineDataList = GoEngineDataToPopup(connstring);
            }
            catch (Exception exc)
            {
                GlobalFunction.SendErrorToText(exc);
                throw exc;
            }
            return EngineDataList;
        }

        public FileResult Download()
        {
            //return File(Server.MapPath("~/Temp/SampleFormat/SampleFormat.zip"),
            //                           "application/zip", "SampleFormat.zip");
            return File(Server.MapPath("~/Temp/SampleFormat/SampleImportFormat.xlsx"),
                              "application/excel", "SampleImportFormat.xlsx");

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DataImport_Insert([DataSourceRequest]DataSourceRequest poRequest, string extension, string excelName)
        {
            mutex.WaitOne();
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            System.Collections.Generic.List<Item_REC> vResult = null;
            System.Collections.Generic.List<Engine_REC> vEngineResult = null;

            List<Item_REC> tblDBItem = null;

            if ((ModelState.IsValid))
            {
                try
                {
                    vResult = ExcelDataBind(extension, excelName);//Excel data
                    //vEngineResult = GetEngineDataToBind(extension, excelName);//Excel Engine data

                    TransactionModel oClass = new TransactionModel();

                    //var CheckEngineExist = oClass.GetEngineDataBySerialNo(vEngineResult[0].SerialNo).ToList();

                    //if (CheckEngineExist.Count > 0)
                    //{
                    //    vsLogMessage.Add(errorcode_text, error_code);
                    //    vsLogMessage.Add(title_text, Error_text);
                    //    vsLogMessage.Add(msg_text, "Engine Info already exist");

                    //    while (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Temp/"), excelName)))
                    //    {
                    //        System.IO.File.Delete(Path.Combine(Server.MapPath("~/Temp/"), excelName));
                    //    }
                    //}
                    //else
                    //{
                    tblDBItem = oClass.GetItemList().ToList();// get data from tblAsset           

                    DateTime ImportedDate = DateTime.Now;
                    string GTMessage = "";
                    string FailMessage = "";
                    string outputMessage = "";
                    if (vResult.Count > 0)
                    {
                        if (!oClass.DataImportInsert(vResult, tblDBItem, vEngineResult, ImportedDate, out outputMessage, out GTMessage, out FailMessage))
                        {
                            if (outputMessage == "" && GTMessage == "")
                            {
                                vsLogMessage.Add(errorcode_text, error_code);
                                vsLogMessage.Add(title_text, Warning_Noti);
                                if (FailMessage == "")
                                    vsLogMessage.Add(msg_text, oClass.ErrorMessage.ToString() + " " + Excel_Data_Insert_Fail);
                                else
                                    vsLogMessage.Add(msg_text, "ItemNo ( " + FailMessage + " ) " + Excel_Data_Insert_Fail);
                            }
                            else if (outputMessage != "" && GTMessage == "")
                            {
                                vsLogMessage.Add(errorcode_text, error_code);
                                vsLogMessage.Add(title_text, Warning_Noti);

                                if (outputMessage == "")
                                    vsLogMessage.Add(msg_text, oClass.ErrorMessage.ToString() + " " + AssetNo_NeedToCheck);
                                else
                                    vsLogMessage.Add(msg_text, outputMessage + " already exist in database");
                            }
                            else if (outputMessage == "" && GTMessage != "")
                            {
                                vsLogMessage.Add(errorcode_text, error_code);
                                vsLogMessage.Add(title_text, Warning_Noti);
                                vsLogMessage.Add(msg_text, GeneralTable_Auto_Insert_Fail);
                            }
                        }
                        else
                        {
                            vsLogMessage.Add(errorcode_text, no_errorcode);
                            vsLogMessage.Add(title_text, success_Noti);
                            vsLogMessage.Add(msg_text, Excel_Data_Insert_Success);
                        }
                        //delete excel file from temp folder
                        while (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Temp/"), excelName)))
                        {
                            System.IO.File.Delete(Path.Combine(Server.MapPath("~/Temp/"), excelName));
                        }
                    }

                    //}
                }
                catch (Exception exc)
                {
                    #region Add By WWN
                    //delete excel file from temp folder
                    while (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Temp/"), excelName)))
                    {
                        System.IO.File.Delete(Path.Combine(Server.MapPath("~/Temp/"), excelName));
                    }
                    #endregion

                    GlobalFunction.SendErrorToText(exc);
                    vsLogMessage.Add(errorcode_text, error_code);
                    vsLogMessage.Add(title_text, Error_text);
                    vsLogMessage.Add(msg_text, "Message : " + exc.Message + "Source : " + exc.Source);
                }
            }
            mutex.ReleaseMutex();
            return Json(vsLogMessage, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult DeleteTempExcel([DataSourceRequest]DataSourceRequest poRequest)
        {
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            try
            {
                string sessionExcelName = System.Web.HttpContext.Current.Session["excelName"] as String;

                if (!string.IsNullOrEmpty(sessionExcelName))
                {
                    while (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Temp/"), sessionExcelName)))
                    {
                        System.IO.File.Delete(Path.Combine(Server.MapPath("~/Temp/"), sessionExcelName));
                    }
                }

                vsLogMessage.Add(errorcode_text, no_errorcode);
                vsLogMessage.Add(title_text, success_Noti);
            }
            catch (Exception exc)
            {
                GlobalFunction.SendErrorToText(exc);
                vsLogMessage.Add(errorcode_text, error_code);
                vsLogMessage.Add(title_text, Error_text);
                vsLogMessage.Add(msg_text, "Message : " + exc.Message + "Source : " + exc.Source);
            }
            return Json(vsLogMessage, JsonRequestBehavior.DenyGet);
        }
        #endregion

        #region Asset Export


        public ActionResult GetLookUpEngineListing(string Type, string text)
        {
            Department_REC department_REC = new Department_REC();
            //department_REC.Dept = " No Department";

            TransactionModel oClass = new TransactionModel();
            var vResultdata = oClass.GetEngineList().ToList();
            //vResultdata.Add(department_REC);
            var vResult = (from e in vResultdata
                           orderby e.EneSerialNo ascending
                           where e.EneSerialNo.ToLower().StartsWith(text.ToLower())
                           select new { e.EneSerialNo });

            return Json(vResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLookUpEngineListingByRig(string Type, string text)
        {
            Department_REC department_REC = new Department_REC();
            //department_REC.Dept = " No Department";

            TransactionModel oClass = new TransactionModel();
            var vResultdata = oClass.GetEngineListByEngineRig().ToList();
            //vResultdata.Add(department_REC);
            var vResult = (from e in vResultdata
                           orderby e.EneSerialNo ascending
                           where e.EneSerialNo.ToLower().StartsWith(text.ToLower())
                           select new { e.EneSerialNo });

            return Json(vResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AssetExport()
        {
            try
            {
                string password = System.Web.HttpContext.Current.Session["Password"] as String;
                if (!string.IsNullOrEmpty(password))
                {
                    #region Button Access Right
                    string RoleName = System.Web.HttpContext.Current.Session["RoleName"] as String;
                    RolePermissionModel rolePermission = new RolePermissionModel();
                    var accesibleRoles = rolePermission.GetMenuPermissionByMenuName(RoleName, "AssetExport").ToList().SingleOrDefault();
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


        public ActionResult AssetExport_Read([DataSourceRequest]DataSourceRequest poRequest, string EngSN)
        {

            TransactionModel oClass = new TransactionModel();
            System.Collections.Generic.List<Item_REC> vResult = null;


            try
            {
                int _actualPSize = poRequest.PageSize;
                int _actualPageNo = poRequest.Page;

                vResult = oClass.GetAssetExportByParameter(EngSN).ToList();

                //#region Pagination Click
                //if (_actualPSize > 21)
                //{
                //   if (vResult.Count() > 100)
                //   {
                //      poRequest.PageSize = 40;

                //   }
                //}
                //#endregion
            }
            catch (Exception ex)
            {
                GlobalFunction.SendErrorToText(ex);
                string ms = ex.Message;
            }
            return new JsonResult() { Data = vResult.ToDataSourceResult(poRequest), JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public FileContentResult Excel_ExportRead(string SelectedAll, string Engine, string[] ItemNoID)
        {
            try
            {
                string PhotoLink = ConfigurationManager.AppSettings.Get("PhotoLink");
                TransactionModel oClass = new TransactionModel();
                var vResult = new List<ItemExport_REC>();

                var EngineData = oClass.GetEngineDataBySerialNo(Engine).ToList();

                if (SelectedAll == "true")
                {
                    vResult = oClass.AllExcelExportListByParameter(Engine, PhotoLink).ToList();
                }
                else
                {
                    vResult = oClass.ExcelExportListByParameter(ItemNoID[0], Engine, PhotoLink).ToList();
                }

                //string[] columns = { "ITEM#", "IPC REFERENCE", "NOMENCLATURE", "PART TYPE", "PWEL STANDARD", "PART STATUS", "VENDOR PN", "PW PN", "SN", "REMARKS", "Timestamp", "User", "Photo Link" };
                string[] columns = { "ITEM#", "IPC REFERENCE", "NOMENCLATURE", "PART TYPE", "PWEL STANDARD", "PART STATUS", "VENDOR PN", "PW PN", "SN", "REMARKS", "Timestamp", "User", "Status" };

                byte[] filecontent = ExcelExportHelper.ExchangeExportExcel(EngineData, vResult, "ItemExport", false, columns);
                return File(filecontent, ExcelExportHelper.ExcelContentType, "ItemExport_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
            }
            catch (Exception ex)
            {
                GlobalFunction.SendErrorToText(ex);
                throw new Exception(ex.Message);
            }
        }

        public FileContentResult Excel_ExportAndDeleteRead(string SelectedAll, string Engine, string[] ItemNoID)
        {
            try
            {
                string PhotoLink = ConfigurationManager.AppSettings.Get("PhotoLink");
                TransactionModel oClass = new TransactionModel();
                var vResult = new List<ItemExport_REC>();

                var EngineData = oClass.GetEngineDataBySerialNo(Engine).ToList();

                if (SelectedAll == "true")
                {
                    vResult = oClass.AllExcelExportListByParameter(Engine, PhotoLink).ToList();
                }
                else
                {
                    vResult = oClass.ExcelExportListByParameter(ItemNoID[0], Engine, PhotoLink).ToList();
                }

                string[] columns = { "ITEM#", "IPC REFERENCE", "NOMENCLATURE", "PART TYPE", "PWEL STANDARD", "PART STATUS", "VENDOR PN", "PW PN", "SN", "REMARKS", "Timestamp", "User", "Photo Link" };
                byte[] filecontent = ExcelExportHelper.ExchangeExportExcel(EngineData, vResult, "ItemExport", false, columns);

                //delete Asset

                if (SelectedAll == "true")
                {
                    oClass.ItemNoDeleteAllByEngine(Engine);
                }
                else
                {
                    oClass.ItemNoDelete(ItemNoID[0]);
                }
                return File(filecontent, ExcelExportHelper.ExcelContentType, "ItemExport_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
            }
            catch (Exception ex)
            {
                GlobalFunction.SendErrorToText(ex);
                throw new Exception(ex.Message);
            }

        }

        #endregion

        #region Part Data Entry/Update/Delete and List
        public ActionResult Item()
        {

            try
            {
                string password = System.Web.HttpContext.Current.Session["Password"] as String;
                if (!string.IsNullOrEmpty(password))
                {
                    #region Button Access Right
                    string RoleName = System.Web.HttpContext.Current.Session["RoleName"] as String;
                    RolePermissionModel rolePermission = new RolePermissionModel();
                    var accesibleRoles = rolePermission.GetMenuPermissionByMenuName(RoleName, "Item").ToList().SingleOrDefault();
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
        public ActionResult Item_Read([DataSourceRequest]DataSourceRequest poRequest)
        {
            ItemModel oClass = new ItemModel();
            var result = oClass.GetItemList();
            return Json(result.ToDataSourceResult(poRequest));
        }

        [HttpPost]
        public JsonResult CheckPartID(string PartID)
        {
            ItemModel it = new ItemModel();
            //bool h = it.GetItemList().ToList().Exists(m => m.PartItemNo.Equals(PartID, StringComparison.CurrentCultureIgnoreCase));
            bool HasItem = it.CheckItem(PartID);
            return Json(HasItem);
        }
        public ActionResult ItemAddNewForm()
        {
            try
            {
                string pas = System.Web.HttpContext.Current.Session["Password"] as String;
                bool isAdminUser = Convert.ToBoolean(System.Web.HttpContext.Current.Session["isAdmin"]);

                if (!string.IsNullOrEmpty(pas))
                {
                    #region Asset Message From XML
                    ErrorMessageModel Model = new ErrorMessageModel();
                    ViewData["EnquiryMsgSelect"] = Model.EnquiryMsgSelect();
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
        public ActionResult ItemEditForm(string id)
        {
            try
            {
                string pas = System.Web.HttpContext.Current.Session["Password"] as String;
                bool isAdminUser = Convert.ToBoolean(System.Web.HttpContext.Current.Session["isAdmin"]);

                if (!string.IsNullOrEmpty(pas))
                {
                    #region Asset Message From XML
                    ErrorMessageModel Model = new ErrorMessageModel();
                    ViewData["EnquiryMsgSelect"] = Model.EnquiryMsgSelect();
                    #endregion

                    ItemModel oClass = new ItemModel();
                    var itemInfo = oClass.GetItem(id);

                    ViewData["ItemUpdate"] = itemInfo;

                    return View("ItemEditForm");
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
        public ActionResult DeleteItem([DataSourceRequest]DataSourceRequest poRequest, string itemNo)
        {
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            string _ResultMsg = string.Empty;
            if ((itemNo != "0"))
            {
                ItemModel oClass = new ItemModel();
                if (!oClass.Delete(itemNo))
                {
                    vsLogMessage.Add(Fields.errorcode_text, Fields.error_code);
                    vsLogMessage.Add(Fields.title_text, Fields.Warning_Noti);

                    vsLogMessage.Add(Fields.msg_text, Fields.itemDeleteFail);
                }
                else
                {
                    vsLogMessage.Add(Fields.errorcode_text, Fields.no_errorcode);
                    vsLogMessage.Add(Fields.title_text, Fields.success_title);
                    vsLogMessage.Add(Fields.msg_text, Fields.itemDeleteSuccess);

                }
            }
            return Json(vsLogMessage, JsonRequestBehavior.DenyGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult InsertItem([DataSourceRequest]DataSourceRequest poRequest, ItemModel poRecord)
        {
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            string _ResultMsg = string.Empty;

            if ((poRecord != null) && (ModelState.IsValid))
            {

                try
                {
                    ItemModel oClass = new ItemModel();

                    #region  Insert Data Info

                    poRecord.CreatedBy = System.Web.HttpContext.Current.Session["UserID"].ToString();
                    poRecord.CreatedDate = DateTime.Now;
                    poRecord.UpdatedBy = System.Web.HttpContext.Current.Session["UserID"].ToString();
                    poRecord.UpdatedDate = DateTime.Now;

                    bool HasItem = oClass.CheckItem(poRecord.PartItemNo);
                    if (!HasItem)
                    {
                        if (!oClass.Insert(poRecord))
                        {
                            vsLogMessage.Add(errorcode_text, error_code);
                            vsLogMessage.Add(title_text, Warning_Noti);
                            vsLogMessage.Add(msg_text, Fields.itemSaveFail);

                        }
                        else
                        {
                            _ResultMsg = GlobalFunction.GetStatus_MsgByKey("Data_Add_Success");
                            vsLogMessage.Add(errorcode_text, no_errorcode);
                            vsLogMessage.Add(title_text, success_title);
                            vsLogMessage.Add(msg_text, _ResultMsg);
                        }
                    }
                    else
                    {
                        vsLogMessage.Add(errorcode_text, error_code);
                        vsLogMessage.Add(title_text, Warning_Noti);
                        vsLogMessage.Add(msg_text, Fields.alreadyExistEPC);
                    }

                    #endregion

                }
                catch (Exception ex)
                {
                    GlobalFunction.SendErrorToText(ex);
                    vsLogMessage.Add(errorcode_text, error_code);
                    vsLogMessage.Add(title_text, Warning_Noti);
                    vsLogMessage.Add(msg_text, ex.Message);

                }
                return Json(vsLogMessage, JsonRequestBehavior.DenyGet);
            }
            else
                return Json(new[] { poRecord }.ToDataSourceResult(poRequest, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateItem([DataSourceRequest]DataSourceRequest poRequest, ItemModel poRecord)
        {
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            string _ResultMsg = string.Empty;

            if ((poRecord != null) && (ModelState.IsValid))
            {
                try
                {
                    ItemModel oClass = new ItemModel();

                    #region  Update Data Info
                    if (!oClass.Update(poRecord))
                    {
                        vsLogMessage.Add(errorcode_text, error_code);
                        vsLogMessage.Add(title_text, Warning_Noti);
                        vsLogMessage.Add(msg_text, Fields.itemUpdateFail);

                    }
                    else
                    {
                        _ResultMsg = GlobalFunction.GetStatus_MsgByKey("Data_Update_Success");
                        vsLogMessage.Add(errorcode_text, no_errorcode);
                        vsLogMessage.Add(title_text, success_title);
                        vsLogMessage.Add(msg_text, _ResultMsg);
                    }
                    #endregion

                }
                catch (Exception ex)
                {
                    GlobalFunction.SendErrorToText(ex);
                    vsLogMessage.Add(errorcode_text, error_code);
                    vsLogMessage.Add(title_text, Warning_Noti);
                    vsLogMessage.Add(msg_text, ex.Message);

                }
                return Json(vsLogMessage, JsonRequestBehavior.DenyGet);
            }
            else
                return Json(new[] { poRecord }.ToDataSourceResult(poRequest, ModelState));
        }
        public ActionResult ItemFormDetail(string id)
        {
            try
            {
                string pas = System.Web.HttpContext.Current.Session["Password"] as String;

                if (!string.IsNullOrEmpty(pas))
                {
                    #region Asset Message From XML
                    ErrorMessageModel Model = new ErrorMessageModel();
                    ViewData["EnquiryMsgSelect"] = Model.EnquiryMsgSelect();
                    #endregion

                    ItemModel oClass = new ItemModel();
                    var _AssetInfo = oClass.GetItem(id);
                    ViewData["ItemUpdate"] = _AssetInfo;
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

        public ActionResult ItemFormDetailByEdit(string id, string Engine)
        {
            try
            {
                string pas = System.Web.HttpContext.Current.Session["Password"] as String;

                if (!string.IsNullOrEmpty(pas))
                {
                    #region Asset Message From XML
                    ErrorMessageModel Model = new ErrorMessageModel();
                    ViewData["EnquiryMsgSelect"] = Model.EnquiryMsgSelect();
                    #endregion

                    ItemModel oClass = new ItemModel();
                    var _AssetInfo = oClass.GetItemDetailByEdit(id, Engine).FirstOrDefault();
                    ViewData["ItemUpdate"] = _AssetInfo;
                    //return View();
                    return View("~/Views/Asset/ItemFormDetail.cshtml");
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

        public ActionResult DownloadItemFile(string itemNo)
        {
            //Model binding.
            ImgObjResult fileInfo = new ImgObjResult();

            try
            {
                // Loading dile info.
                ItemModel oClass = new ItemModel();
                var itemInfo = oClass.GetItemPhoto(itemNo);
                return this.GetFile(itemInfo.Photo, ".png");

            }
            catch (Exception ex)
            {
                // Info
                GlobalFunction.SendErrorToText(ex);
                return null;
            }
            // Info.
            //return null;
        }

        private FileResult GetFile(string fileContent, string fileContentType)
        {
            // Initialization.
            FileResult file = null;

            try
            {
                if (!string.IsNullOrEmpty(fileContent))
                {
                    // Get file.
                    if (fileContent == "no_photo.png")
                    {
                        return null;
                    }

                    byte[] byteContent = Convert.FromBase64String(fileContent);
                    file = this.File(byteContent, fileContentType);
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                // Info.
                throw ex;
                //GlobalFunction.SendErrorToText(ex);
            }

            // info.
            return file;
        }
        public ActionResult ReadItemPartMapping([DataSourceRequest]DataSourceRequest poRequest, string itemNo)
        {
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            string _ResultMsg = string.Empty;

            if (itemNo != "")
            {
                ItemPartMapping oClass = new ItemPartMapping();
                var result = oClass.GetItemPartMapping(itemNo);
                return Json(result.ToDataSourceResult(poRequest));
            }
            return Json(vsLogMessage, JsonRequestBehavior.DenyGet);
        }
        [HttpPost]
        public JsonResult InsertVPN([DataSourceRequest]DataSourceRequest poRequest, ItemPartMapping poRecord, string ItemNo)
        {
            ItemPartMapping oClass = new ItemPartMapping();
            if (ItemNo == "")
            {
                oClass.ErrorMessage = "Item No should not be empty! Plz enter first!";
                ModelState.AddModelError(failed_title, oClass.ErrorMessage);
            }
            else
            {
                poRecord.PartItemNo = ItemNo;
                if ((poRecord.PWPN == null || poRecord.PWPN.Trim() == "" || poRecord.PWPN.Trim() == "N/A") && (poRecord.VPN == null || poRecord.VPN.Trim() == "" || poRecord.VPN.Trim() == "N/A"))
                {
                    oClass.ErrorMessage = "Value can be null either VPN or PWPN";
                    ModelState.AddModelError(failed_title, oClass.ErrorMessage);
                }
                else
                {
                    if (!oClass.Insert(poRecord))
                    {
                        if (oClass.ErrorMessage.Contains("Violation of PRIMARY KEY constraint"))
                        {

                            ModelState.AddModelError(failed_title, DuplicationFail);
                        }
                        else
                            ModelState.AddModelError(failed_title, oClass.ErrorMessage);
                    }
                }
            }
            return Json(new[] { poRecord }.ToDataSourceResult(poRequest, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult InsertItemPartDetail([DataSourceRequest]DataSourceRequest poRequest, ItemPartMapping poRecord, string itemNo)
        {
            if ((poRecord != null) && (ModelState.IsValid))
            {
                ItemPartMapping oClass = new ItemPartMapping();
            }
            return Json(new[] { poRecord }.ToDataSourceResult(poRequest, ModelState));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult CreateItemPartDetails([DataSourceRequest]DataSourceRequest poRequest, ItemPartMapping poRecord, string itemNo)
        {
            if ((poRecord != null) && (ModelState.IsValid))
            {
                ItemPartMapping oClass = new ItemPartMapping();

                #region Create New

                if (itemNo != "")
                {
                    poRecord.PartItemNo = itemNo;
                    if ((poRecord.PWPN == null || poRecord.PWPN.Trim() == "" || poRecord.PWPN.Trim() == "N/A") && (poRecord.VPN == null || poRecord.VPN.Trim() == "" || poRecord.VPN.Trim() == "N/A"))
                    {
                        oClass.ErrorMessage = "Value can be null either VPN or PWPN";
                        ModelState.AddModelError(Error_text, oClass.ErrorMessage);
                    }
                    else
                    {
                        if (!oClass.Insert(poRecord))
                        {
                            if (oClass.ErrorMessage.Contains("Violation of PRIMARY KEY constraint"))
                            {

                                ModelState.AddModelError(Error_text, DuplicationFail);
                            }
                            else
                                ModelState.AddModelError(Error_text, oClass.ErrorMessage);
                        }
                    }
                }
            }

            #endregion
            return Json(new[] { poRecord }.ToDataSourceResult(poRequest, ModelState));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateItemPartDetails([DataSourceRequest]DataSourceRequest poRequest, ItemPartMapping poRecord)
        {
            if ((poRecord != null) && (ModelState.IsValid))
            {
                //string UserID = System.Web.HttpContext.Current.Session["UserID"].ToString();
                //poRecord.CreatedBy = UserID;
                ItemPartMapping oClass = new ItemPartMapping();

                #region Update

                if (poRecord.PartItemNo != "" && !string.IsNullOrEmpty(poRecord.PartDetailsID.ToString()))
                {
                    if ((poRecord.PWPN == null || poRecord.PWPN.Trim() == "" || poRecord.PWPN.Trim() == "N/A") && (poRecord.VPN == null || poRecord.VPN.Trim() == "" || poRecord.VPN.Trim() == "N/A"))
                    {
                        oClass.ErrorMessage = "Value can be null either VPN or PWPN";
                        ModelState.AddModelError(Error_text, oClass.ErrorMessage);
                    }
                    else
                    {
                        if (!oClass.Update(poRecord))
                        {
                            if (oClass.ErrorMessage.Contains("Violation of PRIMARY KEY constraint"))
                            {

                                ModelState.AddModelError(Error_text, DuplicationFail);
                            }
                            else
                                ModelState.AddModelError(Error_text, oClass.ErrorMessage);
                        }
                    }
                }
            }

            #endregion
            return Json(new[] { poRecord }.ToDataSourceResult(poRequest, ModelState));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteItemPartDetails([DataSourceRequest]DataSourceRequest poRequest, ItemPartMapping poRecord)
        {
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            string _ResultMsg = string.Empty;

            if ((poRecord != null) && (ModelState.IsValid))
            {
                //string UserID = System.Web.HttpContext.Current.Session["UserID"].ToString();
                ItemPartMapping oClass = new ItemPartMapping();

                if (!oClass.Delete(poRecord))
                {
                    vsLogMessage.Add(errorcode_text, error_code);
                    vsLogMessage.Add(title_text, Warning_Noti);
                    vsLogMessage.Add(msg_text, Fields.itemSaveFail);

                }
                else
                {
                    _ResultMsg = GlobalFunction.GetStatus_MsgByKey("Data_Delete_Success");
                    vsLogMessage.Add(errorcode_text, no_errorcode);
                    vsLogMessage.Add(title_text, success_title);
                    vsLogMessage.Add(msg_text, _ResultMsg);
                }
            }

            return Json(vsLogMessage, JsonRequestBehavior.DenyGet);
        }
        #endregion

        #region Engine Master
        // CreatedBy Moe 

        public ActionResult Engine()
        {

            try
            {
                string password = System.Web.HttpContext.Current.Session["Password"] as String;
                if (!string.IsNullOrEmpty(password))
                {
                    #region Button Access Right
                    string RoleName = System.Web.HttpContext.Current.Session["RoleName"] as String;
                    RolePermissionModel rolePermission = new RolePermissionModel();
                    var accesibleRoles = rolePermission.GetMenuPermissionByMenuName(RoleName, "Engine").ToList().SingleOrDefault();
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

                    //return View();
                    return View("~/Views/Engine/Engine.cshtml");
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

        public ActionResult Engine_Read([DataSourceRequest]DataSourceRequest poRequest)
        {

            TransactionModel oClass = new TransactionModel();
            System.Collections.Generic.List<Engine_REC> vResult = null;


            try
            {
                int _actualPSize = poRequest.PageSize;
                int _actualPageNo = poRequest.Page;

                vResult = oClass.GetEngineData().ToList();


            }
            catch (Exception ex)
            {
                GlobalFunction.SendErrorToText(ex);
                string ms = ex.Message;
            }
            return new JsonResult() { Data = vResult.ToDataSourceResult(poRequest), JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Engine_Create([DataSourceRequest]DataSourceRequest poRequest, Engine_REC poRecord)
        {
            Engine_REC engine_REC = new Engine_REC();
            #region Validation
            engine_REC.EneSerialNo = poRecord.EneSerialNo;

            if (!string.IsNullOrEmpty(poRecord.Model))
                engine_REC.Model = poRecord.Model;
            else
                engine_REC.Model = "";

            if (!string.IsNullOrEmpty(poRecord.CSN))
                engine_REC.CSN = poRecord.CSN;
            else
                engine_REC.CSN = "";

            if (!string.IsNullOrEmpty(poRecord.TSN))
                engine_REC.TSN = poRecord.TSN;
            else
                engine_REC.TSN = "";


            #endregion

            if ((poRecord != null) && (ModelState.IsValid))
            {
                TransactionModel oClass = new TransactionModel();

                if (!oClass.Insert(engine_REC))
                {
                    if (oClass.ErrorMessage.Contains("Violation of PRIMARY KEY constraint"))
                    {
                        ModelState.AddModelError(Error_text, "SerialNo already exist");
                    }
                    else
                        ModelState.AddModelError(Error_text, oClass.ErrorMessage);
                }

            }
            return Json(new[] { poRecord }.ToDataSourceResult(poRequest, ModelState));
        }

        public ActionResult UpdateInfo(string id, string status)
        {
            string pas = System.Web.HttpContext.Current.Session["Password"] as String;

            if (!string.IsNullOrEmpty(pas))
            {
                TransactionModel model = new TransactionModel();
                var EngineMasterData = model.GetEngineDataBySerialNo(id).FirstOrDefault();
                ViewData["EngineMasterData"] = EngineMasterData;
                System.Web.HttpContext.Current.Session["TransferID"] = id;

                //return View("FormDetail");

                ViewData["FromStatus"] = status;

                return View("~/Views/Engine/FormEdit.cshtml");
            }
            else
                return RedirectToAction("Index", "Login");
        }


        public ActionResult AddNewEngine()
        {
            string pas = System.Web.HttpContext.Current.Session["Password"] as String;

            if (!string.IsNullOrEmpty(pas))
            {
                return View("~/Views/Engine/FormNew.cshtml");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult PartMaster_Read([DataSourceRequest]DataSourceRequest poRequest)
        {

            TransactionModel oClass = new TransactionModel();
            System.Collections.Generic.List<ItemModel> vResult = null;


            try
            {
                int _actualPSize = poRequest.PageSize;
                int _actualPageNo = poRequest.Page;

                vResult = oClass.GetPartMasterData().ToList();


            }
            catch (Exception ex)
            {
                GlobalFunction.SendErrorToText(ex);
                string ms = ex.Message;
            }
            return new JsonResult() { Data = vResult.ToDataSourceResult(poRequest), JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public ActionResult PartMasterpopup_Read([DataSourceRequest]DataSourceRequest poRequest, string selectedEngineSN, string[] DataInGrid, string deleteCheck)
        {

            TransactionModel oClass = new TransactionModel();
            System.Collections.Generic.List<ItemModel> vResult = null;


            try
            {
                int _actualPSize = poRequest.PageSize;
                int _actualPageNo = poRequest.Page;

                vResult = oClass.GetPartMasterDataByFilter(selectedEngineSN, DataInGrid, deleteCheck).ToList();


            }
            catch (Exception ex)
            {
                GlobalFunction.SendErrorToText(ex);
                string ms = ex.Message;
            }
            return new JsonResult() { Data = vResult.ToDataSourceResult(poRequest), JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EngineMaster_Create([DataSourceRequest]DataSourceRequest poRequest, Engine_REC poRecord)
        {
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            string _ResultMsg = string.Empty;

            if (poRecord != null)
            {
                try
                {
                    //check Engine Master already exist
                    TransactionModel oClass = new TransactionModel();
                    var vResult = oClass.GetEngineDataBySerialNo(poRecord.EneSerialNo).ToList();

                    //get data from Part Master
                    List<ItemModel> Partmaster = new List<ItemModel>();
                    if (poRecord.CheckAll != "0" || poRecord.CheckItemNo != null)
                        Partmaster = oClass.GetPartByFilter(poRecord.CheckItemNo, poRecord.CheckAll).ToList();

                    //get data from Part Detail
                    List<ItemPartMapping> partDetail = new List<ItemPartMapping>();
                    if (poRecord.CheckAll != "0" || poRecord.CheckItemNo != null)
                        partDetail = oClass.GetPartDetailByFilter(poRecord.CheckItemNo, poRecord.CheckAll).ToList();

                    if (vResult.Count > 0)
                    {
                        //_ResultMsg = GlobalFunction.GetStatus_MsgByKey("UserRole_Dup_Noti");
                        vsLogMessage.Add(errorcode_text, error_code);
                        vsLogMessage.Add(title_text, Warning_Noti);
                        vsLogMessage.Add(msg_text, "Engine SN already exist");
                    }
                    else
                    {
                        #region  Add New Data

                        if (oClass.InsertMasterEngine(poRecord, Partmaster, partDetail))
                        {
                            _ResultMsg = GlobalFunction.GetStatus_MsgByKey("Data_Add_Success");
                            vsLogMessage.Add(errorcode_text, no_errorcode);
                            vsLogMessage.Add(title_text, success_title);
                            vsLogMessage.Add(msg_text, "Successfully added");
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

        public ActionResult Engine_Destroy([DataSourceRequest]DataSourceRequest poRequest, Engine_REC poRecord)
        {
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            string _ResultMsg = string.Empty;
            if ((poRecord != null) && (ModelState.IsValid))
            {
                TransactionModel oClass = new TransactionModel();
                if (!oClass.EngineDelete(poRecord))
                {
                    vsLogMessage.Add(Fields.errorcode_text, Fields.error_code);
                    vsLogMessage.Add(Fields.title_text, Fields.Warning_Noti);

                    vsLogMessage.Add(Fields.msg_text, Fields.itemDeleteFail);
                }
                else
                {
                    vsLogMessage.Add(Fields.errorcode_text, Fields.no_errorcode);
                    vsLogMessage.Add(Fields.title_text, Fields.success_title);
                    vsLogMessage.Add(Fields.msg_text, Fields.itemDeleteSuccess);

                }
            }
            return Json(vsLogMessage, JsonRequestBehavior.DenyGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EnginePartDetails_Read([DataSourceRequest]DataSourceRequest poRequest)
        {
            string voID = System.Web.HttpContext.Current.Session["TransferID"] as String;

            TransactionModel oClassDetail = new TransactionModel();
            var vResultD = oClassDetail.GetEnginePartDetailByFilter(voID).ToList();
            return Json(vResultD.ToDataSourceResult(poRequest));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EngineMaster_Update([DataSourceRequest]DataSourceRequest poRequest, Engine_REC poRecord)
        {
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            string _ResultMsg = string.Empty;

            if (poRecord != null)
            {
                try
                {
                    TransactionModel oClass = new TransactionModel();

                    //get data from Part Master
                    List<ItemModel> Partmaster = new List<ItemModel>();
                    if (poRecord.CheckAll != "0" || poRecord.CheckItemNo != null)
                        Partmaster = oClass.GetPartByFilter(poRecord.CheckItemNo, poRecord.CheckAll).ToList();

                    //get data from Part Detail
                    List<ItemPartMapping> partDetail = new List<ItemPartMapping>();
                    if (poRecord.CheckAll != "0" || poRecord.CheckItemNo != null)
                        partDetail = oClass.GetPartDetailByFilter(poRecord.CheckItemNo, poRecord.CheckAll).ToList();

                    //check have data in EnginePartDetails
                    var CheckEngineData = oClass.GetEnginePartDetailByFilter(poRecord.EneSerialNo).ToList();

                    #region  update Data

                    if (oClass.UpdateMasterEngine(poRecord, Partmaster, partDetail, CheckEngineData.Count))
                    {
                        _ResultMsg = GlobalFunction.GetStatus_MsgByKey("Data_Update_Success");
                        vsLogMessage.Add(errorcode_text, no_errorcode);
                        vsLogMessage.Add(title_text, success_title);
                        vsLogMessage.Add(msg_text, _ResultMsg);
                    }
                    else
                    {
                        vsLogMessage.Add(errorcode_text, error_code);
                        vsLogMessage.Add(title_text, Warning_Noti);
                        vsLogMessage.Add(msg_text, oClass.ErrorMessage);
                    }
                    #endregion
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
        public ActionResult EngineMaster_PopUp([DataSourceRequest]DataSourceRequest poRequest)
        {
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            string _ResultMsg = string.Empty;

            _ResultMsg = GlobalFunction.GetStatus_MsgByKey("Data_Update_Success");
            vsLogMessage.Add(errorcode_text, no_errorcode);
            vsLogMessage.Add(title_text, success_title);
            vsLogMessage.Add(msg_text, _ResultMsg);
            return Json(vsLogMessage, JsonRequestBehavior.DenyGet);
        }

        #endregion
        
        #region Fan Blade
        public ActionResult Blade()
        {
            try
            {
                string password = System.Web.HttpContext.Current.Session["Password"] as String;
                if (!string.IsNullOrEmpty(password))
                {
                    #region Button Access Right
                    string RoleName = System.Web.HttpContext.Current.Session["RoleName"] as String;
                    RolePermissionModel rolePermission = new RolePermissionModel();
                    var accesibleRoles = rolePermission.GetMenuPermissionByMenuName(RoleName, "Blade").ToList().SingleOrDefault();
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

                    return View("~/Views/FanBlade/Blade.cshtml");
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
        public ActionResult Blade_Read([DataSourceRequest]DataSourceRequest poRequest, string EngSN)
        {
            BladeModel oClass = new BladeModel();
            var vResult = oClass.GetBladeDataBySerialNo(EngSN).ToList();
            return Json(vResult.ToDataSourceResult(poRequest));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult BindRigData([DataSourceRequest]DataSourceRequest poRequest, string EngSN)
        {
            List<EngineRigDetails_Rec> vResult = new List<EngineRigDetails_Rec>();
            try
            {
                BladeModel oClass = new BladeModel();
                vResult = oClass.GetRigDataBySerialNo(EngSN).ToList();
            }
            catch (Exception ex)
            {

            }
            return Json(vResult);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EngineRig_Create([DataSourceRequest]DataSourceRequest poRequest, EngineRigDetails_Rec poRecord)
        {
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            string _ResultMsg = string.Empty;
            BladeModel oClass = new BladeModel();

            if (poRecord != null)
            {
                try
                {
                    //List<EngineBladeDetails_Rec> SlotOrderResult = new List<EngineBladeDetails_Rec>();
                    //SlotOrderResult = oClass.GetSlotOrderResult(poRecord.EneSerialNo).ToList();  // Blade multiple

                    //if (poRecord.SlotsNumber < SlotOrderResult.Count)
                    //{
                    //    vsLogMessage.Add(errorcode_text, error_code);
                    //    vsLogMessage.Add(title_text, Warning_Noti);
                    //    vsLogMessage.Add(msg_text, "overload");
                    //}
                    //else
                    //{
                    #region  Update Data
                    if (oClass.UpdateEngineRigDetails(poRecord))
                    {
                        _ResultMsg = GlobalFunction.GetStatus_MsgByKey("Data_Add_Success");
                        vsLogMessage.Add(errorcode_text, no_errorcode);
                        vsLogMessage.Add(title_text, success_title);
                        vsLogMessage.Add(msg_text, _ResultMsg);
                    }
                    else
                    {
                        vsLogMessage.Add(errorcode_text, error_code);
                        vsLogMessage.Add(title_text, Warning_Noti);
                        vsLogMessage.Add(msg_text, oClass.ErrorMessage);
                    }
                    #endregion
                    //}
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

        public ActionResult EngineBlade_Destroy([DataSourceRequest]DataSourceRequest poRequest, EngineBladeDetails_Rec poRecord)
        {
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            string _ResultMsg = string.Empty;

            if ((poRecord != null) && (ModelState.IsValid))
            {
                BladeModel oClass = new BladeModel();
                if (!oClass.EngineBladeDelete(poRecord))
                {
                    vsLogMessage.Add(Fields.errorcode_text, Fields.error_code);
                    vsLogMessage.Add(Fields.title_text, Fields.Warning_Noti);

                    vsLogMessage.Add(Fields.msg_text, Fields.itemDeleteFail);
                }
                else
                {
                    vsLogMessage.Add(Fields.errorcode_text, Fields.no_errorcode);
                    vsLogMessage.Add(Fields.title_text, Fields.success_title);
                    vsLogMessage.Add(Fields.msg_text, Fields.itemDeleteSuccess);

                }
            }
            return Json(vsLogMessage, JsonRequestBehavior.DenyGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EngineBlade_Create([DataSourceRequest]DataSourceRequest poRequest, EngineBladeDetails_Rec poRecord)
        {
            bool findinner = false;
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            string _ResultMsg = string.Empty;
            if ((poRecord != null))
            {
                try
                {
                    BladeModel oClass = new BladeModel();
                    List<EngineBladeDetails_Rec> SlotOrderResult = new List<EngineBladeDetails_Rec>();
                    SlotOrderResult = oClass.GetSlotOrderResult(poRecord.EneSerialNo).ToList();  // Blade multiple

                    List<EngineRigDetails_Rec> GetSlotsNumber = oClass.GetRigDataBySerialNo(poRecord.EneSerialNo).ToList(); //Rig only one

                    if (SlotOrderResult.Count < GetSlotsNumber[0].SlotsNumber)
                    {
                        int i = 0;

                        for (int j = 0; j < SlotOrderResult.Count; j++)
                        {
                            i++;
                            if (SlotOrderResult[j].SlotOrder == (i))
                            {

                            }
                            else
                            {
                                findinner = true;
                                poRecord.SlotOrder = i;
                                poRecord.BladeNo = i.ToString();
                                break;
                            }
                        }

                        if (!findinner)
                        {
                            i++;
                            int temp = i;
                            poRecord.SlotOrder = temp;
                            poRecord.BladeNo = temp.ToString();
                        }

                        if (!oClass.EngineBladeInsert(poRecord))
                        {
                            if (oClass.ErrorMessage.Contains("Violation of PRIMARY KEY constraint"))
                            {
                                //ModelState.AddModelError(Error_text, "Engine Blade SerialNo already exist");
                                vsLogMessage.Add(msg_text, "Engine Blade SerialNo already exist");
                            }
                            else
                                //ModelState.AddModelError(Error_text, oClass.ErrorMessage);
                                vsLogMessage.Add(msg_text, oClass.ErrorMessage);


                            vsLogMessage.Add(errorcode_text, error_code);
                            vsLogMessage.Add(title_text, Warning_Noti);

                        }
                        else
                        {
                            _ResultMsg = GlobalFunction.GetStatus_MsgByKey("Data_Add_Success");
                            vsLogMessage.Add(errorcode_text, no_errorcode);
                            vsLogMessage.Add(title_text, success_title);
                            vsLogMessage.Add(msg_text, "Successfully added");
                        }
                    }
                    else
                    {
                        vsLogMessage.Add(errorcode_text, error_code);
                        vsLogMessage.Add(title_text, Warning_Noti);
                        vsLogMessage.Add(msg_text, "Record exceeds max number of slot.");
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

            //return Json(new[] { poRecord }.ToDataSourceResult(poRequest, ModelState));
            return Json(vsLogMessage, JsonRequestBehavior.DenyGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EngineBlade_Edit([DataSourceRequest]DataSourceRequest poRequest, EngineBladeDetails_Rec poRecord)
        {
            Dictionary<string, string> vsLogMessage = new Dictionary<string, string>();
            string _ResultMsg = string.Empty;
            BladeModel oClass = new BladeModel();

            if (poRecord != null)
            {
                try
                {
                    #region  Update Data
                    if (oClass.EngineBladeUpdate(poRecord))
                    {
                        _ResultMsg = GlobalFunction.GetStatus_MsgByKey("Data_Update_Success");
                        vsLogMessage.Add(errorcode_text, no_errorcode);
                        vsLogMessage.Add(title_text, success_title);
                        vsLogMessage.Add(msg_text, _ResultMsg);
                    }
                    else
                    {
                        vsLogMessage.Add(errorcode_text, error_code);
                        vsLogMessage.Add(title_text, Warning_Noti);
                        vsLogMessage.Add(msg_text, oClass.ErrorMessage);
                    }
                    #endregion
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
        #endregion
    }
}