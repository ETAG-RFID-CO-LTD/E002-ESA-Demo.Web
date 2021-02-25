using EagleServicesWebApp.Components;
using EagleServicesWebApp.Models;
using EagleServicesWebApp.Models.Item;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EagleServicesWebApp.Models.Enquiry
{

    public class Engine_REC
    {
        public string EneSerialNo { get; set; }
        public string Model { get; set; }
        public string CSN { get; set; }
        public string TSN { get; set; }
        public DateTime ImportedDate { get; set; }
        public string ImportedBy { get; set; }

        public string[] CheckItemNo { get; set; }
        public string CheckAll { get; set; }
    }

    //public class EngineMaster_REC
    //{
    //    [Key]
    //    [Required]
    //    [Display(Name = "Serial No :")]
    //    public string SerialNo { get; set; }
    //    [Display(Name = "Model :")]
    //    public string Model { get; set; }
    //    [Display(Name = "CSN :")]
    //    public string CSN { get; set; }
    //    [Display(Name = "TSN :")]
    //    public string TSN { get; set; }
       
    //}

    public class Item_REC
    {
        //public string EngSN { get; set; }
        public string ItemNo { get; set; }
        public string IPCRef { get; set; }
        public string Nomenclature { get; set; }
        public string PartType { get; set; }
        public bool PwelStd { get; set; }
        public string PartStatus { get; set; }
        public string Remarks { get; set; }
        public string Photo { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        //
        public string VPN { get; set; }
        public string PWPN { get; set; }
        public string SN { get; set; }
        public string PwelStdStr { get; set; }

        public string Timestamp { get; set; }
        public string User { get; set; }
        //public string PhotoLink { get; set; }
        public string Status { get; set; }
    }

    public class ItemExport_REC
    {
        public string ItemNo { get; set; }
        public string IPCRef { get; set; }
        public string Nomenclature { get; set; }
        public string PartType { get; set; }
        //public bool PwelStd { get; set; }
        public string PwelStdStr { get; set; }
        public string PartStatus { get; set; }
        public string VPN { get; set; }
        public string PWPN { get; set; }
        public string SN { get; set; }
        public string Remarks { get; set; }
        public string Timestamp { get; set; }
        public string User { get; set; }
        //public string PhotoLink { get; set; }
        public string Status { get; set; }
    }

    public class ItemPartMapping_REC
    {
        public int ID { get; set; }
        public string ItemNo { get; set; }
        public string VPN { get; set; }
        public string PWPN { get; set; }
        public string SN { get; set; }
        public string QCStatus { get; set; }
        public DateTime QCDate { get; set; }
        public string QCBy { get; set; }
    }

    public class TransactionModel
    {
        public int ErrorNo { get; set; }
        public string ErrorMessage { get; set; }

        public TransactionModel()
        {
            ErrorNo = 0;
            ErrorMessage = "";
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<Item_REC> GetItemList()
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "" +
                        " SELECT " +
                        "    ISNULL(PartItemNo,' ') ItemNo   , ISNULL(IPCRef,' ') IPCRef, " +
                        "    ISNULL(Nomenclature,'') Nomenclature,ISNULL(PartType,'') PartType ,PwelStd , " +
                        "   ISNULL(PartStatus,' ') PartStatus , ISNULL(SN,' ') SN ," +
                        "  ISNULL(Remarks,' ') Remarks  " +
                        "  FROM  [dbo].[tblPartMaster] with (NOLOCK) " +
                        ";"
        ;

            var vQuery = oRemoteDB.Database.SqlQuery<Item_REC>(sSQL);

            return vQuery;
        }

        public bool DataImportInsert(List<Item_REC> vResult, List<Item_REC> tblDBItem, List<Engine_REC> vEngineResult, DateTime TransactionDate, out string OutputMessage, out string gtMessage, out string failMessage)
        {
            bool bReturn = true;
            bool tempReturn = true;
            bool RFIDReturn = true;
            gtMessage = "";
            failMessage = "";
            OutputMessage = "";
            string RFIDError = "";

            bool boolsuccess = true;
            bool ItemMappingSuccess = true;

            DatabaseContext oRemoteDB = new DatabaseContext();
            var oTransaction = oRemoteDB.Database.BeginTransaction();
            string sSQL = "";
            string AssetSQL = "";
            string ItemMappingSQL = "";
            List<ItemPartMapping_REC> itemPartsListing = new List<ItemPartMapping_REC>();
            ItemPartMapping_REC itemPart;
            try
            {
                //if (vEngineResult.Count > 0)
                //{
                //    foreach (Engine_REC engine in vEngineResult)
                //    {
                //        sSQL = "";
                //        sSQL = "" +
                //         "INSERT INTO tblEngineMaster " +
                //         "      ( " +
                //         "      EneSerialNo,Model,CSN,TSN,CreatedDate,CreatedBy,UpdatedDate,UpdatedBy " +
                //         "      ) " +
                //         "      VALUES " +
                //         "      (" +
                //          "      @SerialNo,@Model,@CSN,@TSN,GETDATE(),@User,@ImportedDate,@ImportedBy " +
                //         "      )" +
                //         ";"
                //         ;

                //        List<SqlParameter> oParameters = new List<SqlParameter>();
                //        oParameters.Add(new SqlParameter("@SerialNo", engine.SerialNo.Trim()));
                //        oParameters.Add(new SqlParameter("@Model", engine.Model.Trim()));
                //        oParameters.Add(new SqlParameter("@CSN", engine.CSN.Trim()));
                //        oParameters.Add(new SqlParameter("@TSN", engine.TSN.Trim()));
                //        oParameters.Add(new SqlParameter("@User", "admin"));
                //        oParameters.Add(new SqlParameter("@ImportedDate", DateTime.Now));
                //        oParameters.Add(new SqlParameter("@ImportedBy", "admin"));//System.Web.HttpContext.Current.Session["UserID"].ToString()

                //        SqlParameter[] vSqlParameter = oParameters.ToArray();

                //        int success = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
                //        if (success > 0)
                //        {

                //        }
                //        else
                //        {
                //            boolsuccess = false;
                //        }
                //    }
                //}

                if (boolsuccess)
                {
                    foreach (Item_REC item in vResult)
                    {
                        //if (tempReturn)
                        //{
                        //List<string> CheckItemNo = (from value in tblDBItem where value.ItemNo.Trim() == item.ItemNo.Trim() select value.ItemNo.Trim()).ToList<string>();//check have data

                        //if (CheckItemNo.Count > 0)
                        //{
                        //    tempReturn = false;
                        //    RFIDReturn = false;
                        //    RFIDError += item.ItemNo.Trim() + ",";
                        //}

                        if (tempReturn)
                        {
                            AssetSQL = "[dbo].[INS_PartMasterforImport] " +
                                        "@PartNo,@IPC,@Nomencla,@PtType," +
                                        "@PwelStd,@PtStatus,@PtSN,@rmk,@IDate,@IBy";

                            List<SqlParameter> oParameters = new List<SqlParameter>();
                            oParameters.Add(new SqlParameter("@PartNo", item.ItemNo.Trim()));
                            oParameters.Add(new SqlParameter("@IPC", item.IPCRef.Trim()));
                            oParameters.Add(new SqlParameter("@Nomencla", item.Nomenclature.Trim()));
                            oParameters.Add(new SqlParameter("@PtType", item.PartType.Trim()));
                            oParameters.Add(new SqlParameter("@PwelStd", item.PwelStd));
                            oParameters.Add(new SqlParameter("@PtStatus", item.PartStatus.Trim()));
                            oParameters.Add(new SqlParameter("@PtSN", item.SN.Trim()));
                            oParameters.Add(new SqlParameter("@rmk", item.Remarks.Trim()));
                            oParameters.Add(new SqlParameter("@IDate", TransactionDate));
                            oParameters.Add(new SqlParameter("@IBy", "admin"));

                            SqlParameter[] vSqlParameter = oParameters.ToArray();

                            int success2 = oRemoteDB.Database.ExecuteSqlCommand(AssetSQL, vSqlParameter);
                            if (success2 > 0)
                            {
                                itemPart = new ItemPartMapping_REC();
                                itemPart.ItemNo = item.ItemNo.Trim();
                                itemPart.VPN = item.VPN.Trim();
                                itemPart.PWPN = item.PWPN.Trim();
                                itemPart.SN = item.SN.Trim();
                                itemPartsListing.Add(itemPart);
                            }
                            else
                            {
                                tempReturn = false;
                                failMessage += item.ItemNo.Trim();
                            }
                        }
                        // }               
                    }
                    #region VPN/PVN 
                    if (tempReturn)
                    {
                        foreach (ItemPartMapping_REC itemPartMapping in itemPartsListing)
                        {
                            //string []VPNArray;// itemPartMapping.VPN.Split(',');
                            //string[] PWPNArray;//itemPartMapping.PWPN.Split(',');

                            String[] VPNArray = new String[0];
                            String[] PWPNArray = new String[0];

                            bool boolPWPN = true;
                            bool boolVPN = true;
                            bool NAboolPWPN = false;
                            bool NAboolVPN = false;

                            //PWPN
                            if (itemPartMapping.PWPN.Trim() == "")
                            {
                                boolPWPN = false;
                            }
                            else if (itemPartMapping.PWPN.Trim() == "N/A")
                            {
                                NAboolPWPN = true;
                            }
                            else
                            {
                                PWPNArray = itemPartMapping.PWPN.Split(',');
                            }

                            //VPN
                            if (itemPartMapping.VPN.Trim() == "")
                            {
                                boolVPN = false;
                            }
                            else if (itemPartMapping.VPN.Trim() == "N/A")
                            {
                                NAboolVPN = true;
                            }
                            else
                            {
                                VPNArray = itemPartMapping.VPN.Split(',');
                            }


                            if (VPNArray.Length > PWPNArray.Length)
                            {
                                for (int i = 0; i < VPNArray.Length; i++)
                                {
                                    ItemMappingSQL = "";

                                    if (i < PWPNArray.Length || NAboolPWPN == true)
                                    {
                                        ItemMappingSQL = "INS_PartMasterDetailsForImport @ItemNo,@VPN,@PWPN ";
                                        //"INSERT INTO tblPartMasterDetails " +
                                        //"      ( " +
                                        //"      PartItemNo,VPN,PWPN " +
                                        //"      ) " +
                                        //"      VALUES " +
                                        //"      (" +
                                        //"      @ItemNo,@VPN,@PWPN " +
                                        //"      )" +
                                        //";"
                                        ;
                                    }
                                    else
                                    {
                                        boolPWPN = false;
                                        ItemMappingSQL = "INS_PartMasterDetailsForImport @ItemNo,@VPN ";
                                        //"INSERT INTO tblPartMasterDetails " +
                                        //"      ( " +
                                        //"      PartItemNo,VPN " +
                                        //"      ) " +
                                        //"      VALUES " +
                                        //"      (" +
                                        //"      @ItemNo,@VPN " +
                                        //"      )" +
                                        //";"
                                        ;
                                    }

                                    List<SqlParameter> oParameters = new List<SqlParameter>();

                                    oParameters.Add(new SqlParameter("@ItemNo", itemPartMapping.ItemNo.Trim()));
                                    oParameters.Add(new SqlParameter("@VPN", VPNArray[i].Trim()));
                                    if (NAboolPWPN)
                                    {
                                        oParameters.Add(new SqlParameter("@PWPN", "N/A"));
                                    }
                                    else
                                    {
                                        if (boolPWPN)
                                            oParameters.Add(new SqlParameter("@PWPN", PWPNArray[i].Trim()));
                                    }
                                    //oParameters.Add(new SqlParameter("@SN", itemPartMapping.SN.Trim()));

                                    SqlParameter[] vSqlParameter = oParameters.ToArray();

                                    if (oRemoteDB.Database.ExecuteSqlCommand(ItemMappingSQL, vSqlParameter) > 0)
                                    {

                                    }
                                    else
                                    {
                                        ItemMappingSuccess = false;
                                        failMessage += itemPartMapping.ItemNo.Trim();
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < PWPNArray.Length; i++)
                                {
                                    ItemMappingSQL = "";

                                    if (i < VPNArray.Length || NAboolVPN == true)
                                    {
                                        ItemMappingSQL = "INS_PartMasterDetailsForImport @ItemNo,@VPN,@PWPN ";
                                        //"INSERT INTO tblPartMasterDetails " +
                                        //"      ( " +
                                        //"      PartItemNo,VPN,PWPN " +
                                        //"      ) " +
                                        //"      VALUES " +
                                        //"      (" +
                                        //"      @ItemNo,@VPN,@PWPN " +
                                        //"      )" +
                                        //";"
                                        //;
                                    }
                                    else
                                    {
                                        boolVPN = false;
                                        ItemMappingSQL = "INS_PartMasterDetailsForImport @ItemNo,@PWPN ";
                                        //"INSERT INTO tblPartMasterDetails " +
                                        //"      ( " +
                                        //"      PartItemNo,PWPN " +
                                        //"      ) " +
                                        //"      VALUES " +
                                        //"      (" +
                                        //"      @ItemNo,@PWPN " +
                                        //"      )" +
                                        //";"
                                        //;
                                    }

                                    List<SqlParameter> oParameters = new List<SqlParameter>();

                                    oParameters.Add(new SqlParameter("@ItemNo", itemPartMapping.ItemNo.Trim()));
                                    if (NAboolVPN)
                                    {
                                        oParameters.Add(new SqlParameter("@VPN", "N/A"));
                                    }
                                    else
                                    {
                                        if (boolVPN)
                                            oParameters.Add(new SqlParameter("@VPN", VPNArray[i].Trim()));
                                    }

                                    oParameters.Add(new SqlParameter("@PWPN", PWPNArray[i].Trim()));
                                    //oParameters.Add(new SqlParameter("@SN", itemPartMapping.SN.Trim()));

                                    SqlParameter[] vSqlParameter = oParameters.ToArray();

                                    if (oRemoteDB.Database.ExecuteSqlCommand(ItemMappingSQL, vSqlParameter) >= 0)
                                    {

                                    }
                                    else
                                    {
                                        ItemMappingSuccess = false;
                                        failMessage += itemPartMapping.ItemNo.Trim();
                                    }
                                }
                            }
                        }

                        if (ItemMappingSuccess)
                        {
                            oTransaction.Commit();
                            bReturn = true;
                        }
                        else
                        {
                            oTransaction.Rollback();
                            bReturn = false;
                        }
                    }

                    else
                    {
                        oTransaction.Rollback();
                        bReturn = false;
                        if (RFIDReturn == false)
                        {
                            OutputMessage += "ItemNo (" + RFIDError.TrimEnd(',') + ") </br>";
                        }
                    }
                    #endregion
                }
                else
                {
                    oTransaction.Rollback();
                    gtMessage = "Error";
                    bReturn = false;
                }
            }
            catch (Exception exc)
            {
                oTransaction.Rollback();
                ErrorMessage = exc.Message;
                bReturn = false;
            }
            //------------------------------
            return bReturn;
        }

        #region Export

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<Engine_REC> GetEngineList()
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "" +
                        "SELECT " +
                         "   EneSerialNo,Model,CSN,TSN " +
                         "    FROM tblEngineMaster with (nolock) " + "" +
                         "   ORDER BY EneSerialNo " +
                        ";"
        ;

            var vQuery = oRemoteDB.Database.SqlQuery<Engine_REC>(sSQL);

            return vQuery;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<EngineRigDetails_Rec> GetEngineListByEngineRig()
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "" +
                        "SELECT " +
                         "   EneSerialNo " +
                         "    FROM tblEngineRigDetails with (nolock) " + "" +
                         "   ORDER BY EneSerialNo " +
                        ";"
        ;

            var vQuery = oRemoteDB.Database.SqlQuery<EngineRigDetails_Rec>(sSQL);

            return vQuery;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<Engine_REC> GetEngineDataBySerialNo(string SerialNo)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "" +
                        "SELECT " +
                         "   EneSerialNo,Model,CSN,TSN " +
                         "    FROM tblEngineMaster with (nolock) " + "" +
                         "    WHERE EneSerialNo= '" + SerialNo + "' " +
                         "   ORDER BY EneSerialNo " +
                        ";"
        ;

            var vQuery = oRemoteDB.Database.SqlQuery<Engine_REC>(sSQL);

            return vQuery;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<Engine_REC> GetEngineData()
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "" +
                        "SELECT " +
                         "   EneSerialNo,Model,CSN,TSN " +
                         "    FROM tblEngineMaster with (nolock) " + "" +
                         "   ORDER BY EneSerialNo " +
                        ";"
        ;

            var vQuery = oRemoteDB.Database.SqlQuery<Engine_REC>(sSQL);

            return vQuery;
        }


        public System.Data.Entity.Infrastructure.DbRawSqlQuery<ItemModel> GetPartMasterData()
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "" +
                        "select PartItemNo,IPCRef,Nomenclature,PartType,PwelStd,PartStatus,Remarks, " +
                        " case PwelStd when 1 then 'YES' Else 'NO' End As PwelStdStr from tblPartMaster with (nolock);"
        ;

            var vQuery = oRemoteDB.Database.SqlQuery<ItemModel>(sSQL);

            return vQuery;
        }


        public System.Data.Entity.Infrastructure.DbRawSqlQuery<ItemModel> GetEnginePartDetailByFilter(string EngineSerialNo)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "" +
                        "select PartItemNo,IPCRef,Nomenclature,PartType,PwelStd,PartStatus,Remarks, case PwelStd when 1 then 'YES' Else 'NO' End As PwelStdStr " +
                        " from tblEnginePartDetails with (nolock) where EneSerialNo = '" + EngineSerialNo + "';"
                        ;

            var vQuery = oRemoteDB.Database.SqlQuery<ItemModel>(sSQL);

            return vQuery;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<ItemModel> GetPartMasterDataByFilter(string EngineSerialNo,string[] DataInGrid,string deleteCheck)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "";

            if (deleteCheck == "false")
            {
                sSQL = "" +
                        "select PartItemNo,IPCRef,Nomenclature,PartType,PwelStd,PartStatus,Remarks, " +
                        " case PwelStd when 1 then 'YES' Else 'NO' End As PwelStdStr from tblPartMaster with (nolock) where PartItemNo not in " +
                        " ( select partitemno from tblEnginePartDetails with (nolock) where EneSerialNo = '" + EngineSerialNo + "' );"
                ;
            }
            else
            {
                if (DataInGrid != null)
                {
                    var ParameterFilter = " Where ( ";
                    for (int a = 0; a < DataInGrid.Length; a++)
                    {
                        if (a == DataInGrid.Length - 1)
                            ParameterFilter += " PartItemNo <>'" + DataInGrid[a] + "' )";
                        else
                            ParameterFilter += " PartItemNo <>'" + DataInGrid[a] + "' and";
                    }

                    sSQL = "" +
                           "select PartItemNo,IPCRef,Nomenclature,PartType,PwelStd,PartStatus,Remarks, " +
                           " case PwelStd when 1 then 'YES' Else 'NO' End As PwelStdStr from tblPartMaster with (nolock) " + ParameterFilter;
                    ;
                }
                else
                {
                    sSQL = "" +
                            "select PartItemNo,IPCRef,Nomenclature,PartType,PwelStd,PartStatus,Remarks, " +
                            " case PwelStd when 1 then 'YES' Else 'NO' End As PwelStdStr from tblPartMaster with (nolock) "
                    ;
                }
            }

            var vQuery = oRemoteDB.Database.SqlQuery<ItemModel>(sSQL);

            return vQuery;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<Item_REC> GetAssetExportByParameter(string EngSN)
        {
            //string _photoExt = ".png";
            bool isAdminUser = Convert.ToBoolean(System.Web.HttpContext.Current.Session["isAdmin"]);
            string UserID = System.Web.HttpContext.Current.Session["UserID"] as String;
            string sSQL = string.Empty;
            DatabaseContext oRemoteDB = new DatabaseContext();
            //
            //if (Department == " No Department")
            //   Department = ";"

            var ParameterFilter = EngSN.ToUpper() == "ALL" ? " " : "  AND EneSerialNo='" + EngSN + "'";

            var ConditionFilter = " "; //isAdminUser == true ? " " : " AND Dept in (Select Dept from tblDepartmentMapping where UserID='" + UserID + "')";
                                      //sSQL = "" +
                                      //" SELECT " +
                                      //"    ISNULL(EngSN,'') EngSN , ISNULL(ItemNo,' ') ItemNo   , ISNULL(IPCRef,' ') IPCRef, " +
                                      //"    ISNULL(Nomenclature,'') Nomenclature,ISNULL(PartType,'') PartType ,PwelStd ,case PwelStd when 1 then 'YES' Else 'NO' End As PwelStdStr, " +
                                      //"   ISNULL(PartStatus,' ') PartStatus , ISNULL(Remarks,' ') Remarks,dbo.VPNListByItemNo (ItemNo) as VPN,dbo.PWPNListByItemNo (ItemNo) as PWPN,dbo.SNListByItemNo (ItemNo) as SN  " +
                                      //"  FROM  tblItem with (NOLOCK)  WHERE ISNULL(EngSN,' ') <>  ' ' " + ParameterFilter + ConditionFilter +
                                      //";"

            sSQL = "" +
         "  SELECT  ISNULL(EneSerialNo,'') EngSN ,ISNULL(PartItemNo, ' ') ItemNo   , " +
         "  ISNULL(IPCRef, ' ') IPCRef,  ISNULL(Nomenclature, '') Nomenclature,ISNULL(PartType, '') PartType ," +
         "  PwelStd ,case PwelStd when 1 then 'YES' Else 'NO' End As PwelStdStr,  ISNULL(PartStatus, ' ') PartStatus , " +
         " dbo.VPNListByItemNo(PartItemNo,ISNULL(EneSerialNo,'')) as VPN,dbo.PWPNListByItemNo(PartItemNo,ISNULL(EneSerialNo,'')) as PWPN,  " +
         "  (CASE WHEN PartType is null or PartType = '' THEN '' ELSE dbo.SNListByItemNo(PartItemNo,ISNULL(EneSerialNo,'')) END) As SN,  " +
         "  ISNULL(Remarks, ' ') Remarks,  dbo.ItemDateAndUserByItemNo(PartItemNo,'Date',ISNULL(EneSerialNo,'')) As Timestamp,  " +
         "  dbo.ItemDateAndUserByItemNo(PartItemNo,'User',ISNULL(EneSerialNo,'')) As[User],  dbo.ItemStatusByItemNo(PartItemNo,ISNULL(EneSerialNo,'')) as Status  " +
         " FROM [dbo].[tblEnginePartDetails] with(NOLOCK) " +
         " WHERE ISNULL(EneSerialNo,' ') <> ''  " + ParameterFilter + ConditionFilter +
         ";"


;

            var vQuery = oRemoteDB.Database.SqlQuery<Item_REC>(sSQL);

            return vQuery;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<ItemExport_REC> AllExcelExportListByParameter(string Engine, string PhotoPath)
        {
            bool isAdminUser = Convert.ToBoolean(System.Web.HttpContext.Current.Session["isAdmin"]);
            string UserID = System.Web.HttpContext.Current.Session["UserID"] as String;
            string sSQL = string.Empty;
            DatabaseContext oRemoteDB = new DatabaseContext();

            var ParameterFilter = "  AND EneSerialNo='" + Engine + "'";

            var ConditionFilter = " ";//isAdminUser == true ? " " : " AND Dept in (Select Dept from tblDepartmentMapping where UserID='" + UserID + "')";
                                     //sSQL = "" +
                                     //            " SELECT " +
                                     //             "    ISNULL(ItemNo,' ') ItemNo   , ISNULL(IPCRef,' ') IPCRef, " +
                                     //             "    ISNULL(Nomenclature,'') Nomenclature,ISNULL(PartType,'') PartType ,case PwelStd when 1 then 'YES' Else 'NO' End As PwelStdStr, " +
                                     //             "   ISNULL(PartStatus,' ') PartStatus , dbo.VPNListByItemNo (ItemNo) as VPN,dbo.PWPNListByItemNo (ItemNo) as PWPN, " +
                                     //             //" dbo.SNListByItemNo (ItemNo) as SN , " +
                                     //            " (CASE WHEN PartType is null or PartType = '' THEN '' ELSE dbo.SNListByItemNo(ItemNo) END) As SN, " +
                                     //             " ISNULL(Remarks,' ') Remarks, " +
                                     //             //"  FORMAT(CONVERT(DATETIME,ImgUploadedDate,108),'dd/MM/yyyy HH:mm:ss') As Timestamp , " +
                                     //             //" ISNULL(ImgUploadedBy,' ') As [User], " +
                                     //            " (CASE WHEN PartType is null or PartType = '' THEN '' ELSE FORMAT(CONVERT(DATETIME, ImgUploadedDate,108),'dd/MM/yyyy HH:mm:ss') END) As Timestamp, " +
                                     //            " (CASE WHEN PartType is null or PartType = '' THEN '' ELSE  ISNULL(ImgUploadedBy, ' ') END) As[User], " +
                                     //            " CASE  WHEN Photo is null or photo = '' THEN ''  ELSE '" + PhotoPath + "' + ItemNo + " + "'.png'" + " END As PhotoLink " +
                                     //            "  FROM  tblItem with (NOLOCK) WHERE ISNULL(EngSN,' ') <>  ' ' " + ParameterFilter + ConditionFilter +
                                     //            ";"
                                     //            " ISNULL(Remarks,' ') Remarks, " +
                                     //"  FORMAT(CONVERT(DATETIME,ImgUploadedDate,108),'dd/MM/yyyy HH:mm:ss') As Timestamp , " +
                                     //" ISNULL(ImgUploadedBy,' ') As [User], " +

            sSQL = "" +
                     "  SELECT  ISNULL(PartItemNo, ' ') ItemNo   , " +
                     "  ISNULL(IPCRef, ' ') IPCRef,  ISNULL(Nomenclature, '') Nomenclature,ISNULL(PartType, '') PartType ," +
                     "  PwelStd ,case PwelStd when 1 then 'YES' Else 'NO' End As PwelStdStr,  ISNULL(PartStatus, ' ') PartStatus , " +
                     " dbo.VPNListByItemNo(PartItemNo,ISNULL(EneSerialNo,'')) as VPN,dbo.PWPNListByItemNo(PartItemNo,ISNULL(EneSerialNo,'')) as PWPN,  " +
                     "  (CASE WHEN PartType is null or PartType = '' THEN '' ELSE dbo.SNListByItemNo(PartItemNo,ISNULL(EneSerialNo,'')) END) As SN,  " +
                     "  ISNULL(Remarks, ' ') Remarks,  dbo.ItemDateAndUserByItemNo(PartItemNo,'Date',ISNULL(EneSerialNo,'')) As Timestamp,  " +
                     "  dbo.ItemDateAndUserByItemNo(PartItemNo,'User',ISNULL(EneSerialNo,'')) As[User],  dbo.ItemStatusByItemNo(PartItemNo,ISNULL(EneSerialNo,'')) as Status  " +
                     " FROM [dbo].[tblEnginePartDetails] with(NOLOCK) " +
                     " WHERE ISNULL(EneSerialNo,' ') <> ''   " + ParameterFilter + ConditionFilter +
                     ";";

            var vQuery = oRemoteDB.Database.SqlQuery<ItemExport_REC>(sSQL);

            return vQuery;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<ItemExport_REC> ExcelExportListByParameter(string ItemNoStr, string Engine, string PhotoPath)
        {
            bool isAdminUser = Convert.ToBoolean(System.Web.HttpContext.Current.Session["isAdmin"]);
            string UserID = System.Web.HttpContext.Current.Session["UserID"] as String;
            string sSQL = string.Empty;
            DatabaseContext oRemoteDB = new DatabaseContext();

            var ParameterFilter = "  AND EneSerialNo='" + Engine + "'";
            ParameterFilter += " AND (";
            var ItemNoArry = ItemNoStr.TrimEnd(',').Split(',');
            for (int a = 0; a < ItemNoArry.Length; a++)
            {
                if (a == ItemNoArry.Length - 1)
                    ParameterFilter += " PartItemNo ='" + ItemNoArry[a] + "' )";
                else
                    ParameterFilter += " PartItemNo ='" + ItemNoArry[a] + "' or";
            }

            var ConditionFilter = " "; //isAdminUser == true ? " " : " AND Dept in (Select Dept from tblDepartmentMapping where UserID='" + UserID + "')";
                                      //sSQL = "" +
                                      //            " SELECT " +
                                      //             "    ISNULL(ItemNo,' ') ItemNo   , ISNULL(IPCRef,' ') IPCRef, " +
                                      //             "    ISNULL(Nomenclature,'') Nomenclature,ISNULL(PartType,'') PartType ,case PwelStd when 1 then 'YES' Else 'NO' End As PwelStdStr, " +
                                      //             "   ISNULL(PartStatus,' ') PartStatus , dbo.VPNListByItemNo (ItemNo) as VPN,dbo.PWPNListByItemNo (ItemNo) as PWPN,dbo.SNListByItemNo (ItemNo) as SN ,ISNULL(Remarks,' ') Remarks, " +
                                      //             "  FORMAT(CONVERT(DATETIME,ImgUploadedDate,108),'dd/MM/yyyy HH:mm:ss') As Timestamp , ISNULL(ImgUploadedBy,' ') As [User], " +
                                      //            " CASE  WHEN Photo is null or photo = '' THEN ''  ELSE '" + PhotoPath + "' + ItemNo + " + "'.png'" + " END As PhotoLink " +
                                      //            "  FROM  tblItem with (NOLOCK) WHERE ISNULL(EngSN,' ') <>  ' ' " + ParameterFilter + ConditionFilter +
                                      //            ";"
            sSQL = " SELECT ISNULL(ed.PartItemNo, ' ') ItemNo   , ISNULL(ed.IPCRef, ' ') IPCRef, " +
                   "  ISNULL(ed.Nomenclature, '') Nomenclature,ISNULL(ed.PartType, '') PartType , " +
                   "  case ed.PwelStd when 1 then 'YES' Else 'NO' End As PwelStdStr,  " +
                   " ISNULL(ed.PartStatus, ' ') PartStatus , dbo.VPNListByItemNo(ed.PartItemNo, ed.EneSerialNo) as VPN, " +
                   " dbo.PWPNListByItemNo(ed.PartItemNo, ed.EneSerialNo) as PWPN,dbo.SNListByItemNo(ed.PartItemNo, ed.EneSerialNo) as SN ,ISNULL(ed.Remarks, ' ') Remarks, " +
                  " FORMAT(CONVERT(DATETIME, UpdatedDate, 108), 'dd/MM/yyyy HH:mm:ss') As Timestamp, ISNULL(UpdatedBy, ' ') As[User], " +
                      " CASE  WHEN ed.Photo is null or ed.photo = '' THEN ''  ELSE '" + PhotoPath + "' + ed.PartItemNo + " + "'.png'" + " END As PhotoLink " +
                      "  FROM  [dbo].[tblEnginePartDetails] ed left join tblPartMaster pm on pm.PartItemNo=ed.PartItemNo WHERE ISNULL(SN,' ') <>  ' ' " + ParameterFilter + ConditionFilter +
                      ";"
        ;

            var vQuery = oRemoteDB.Database.SqlQuery<ItemExport_REC>(sSQL);

            return vQuery;
        }

        public bool ItemNoDelete(string AssetID)
        {
            bool bReturn = true;
            bool tempReturn = true;
            bool tempReturn2 = true;
            //------------------------------
            DatabaseContext oRemoteDB = new DatabaseContext();
            var oTransaction = oRemoteDB.Database.BeginTransaction();
            try
            {
                string sSQL = "";
                var AssetIDArry = AssetID.TrimEnd(',').Split(',');
                for (int a = 0; a < AssetIDArry.Length; a++)
                {
                    sSQL = " DELETE " +
                               " FROM tblItemPartMapping " +
                               " WHERE ItemNo = '" + AssetIDArry[a] + "'";
                    List<SqlParameter> oParameters = new List<SqlParameter>();

                    SqlParameter[] vSqlParameter = oParameters.ToArray();
                    int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL);

                    if (nReturn >= 1)
                    {

                    }
                    else
                    {
                        tempReturn = false;
                    }
                }

                if (tempReturn)
                {
                    //Success
                    for (int a = 0; a < AssetIDArry.Length; a++)
                    {
                        string sItemSQL = "";
                        sItemSQL = " DELETE " +
                               " FROM tblItem " +
                               " WHERE ItemNo = '" + AssetIDArry[a] + "'";
                        List<SqlParameter> oParameters = new List<SqlParameter>();

                        SqlParameter[] vSqlParameter = oParameters.ToArray();
                        int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sItemSQL);

                        if (nReturn >= 1)
                        {

                        }
                        else
                        {
                            tempReturn2 = false;
                        }
                    }

                    if (tempReturn2)
                    {
                        oTransaction.Commit();
                    }
                    else
                    {
                        oTransaction.Rollback();
                        ErrorMessage = Fields.itemandMappingDeleteFail;
                        bReturn = false;
                    }

                }
                else
                {
                    //Fail (RollBack)
                    oTransaction.Rollback();
                    ErrorMessage = Fields.itemandMappingDeleteFail;
                    bReturn = false;
                }
            }
            catch (Exception ex)
            {
                //RollBack  
                oTransaction.Rollback();
                ErrorMessage = ex.Message;
                bReturn = false;
            }
            //------------------------------
            return bReturn;
        }

        public bool ItemNoDeleteAllByEngine(string Engine)
        {
            bool bReturn = true;
            //------------------------------
            DatabaseContext oRemoteDB = new DatabaseContext();
            var oTransaction = oRemoteDB.Database.BeginTransaction();
            try
            {
                string sSQL = "";
                sSQL = " DELETE " +
                               " FROM tblItemPartMapping " +
                               " WHERE ItemNo in (select ItemNo from tblItem where EngSN = Engine) ";

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@Engine", Engine));

                SqlParameter[] vSqlParameter = oParameters.ToArray();

                if (oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter) >= 1)
                {
                    //Success

                    string sItemSQL = "";
                    sItemSQL = " DELETE " +
                                   " FROM tblItem " +
                                   " WHERE ItemNo= ";

                    List<SqlParameter> oParameters1 = new List<SqlParameter>();
                    oParameters1.Add(new SqlParameter("@Engine", Engine));

                    SqlParameter[] vSqlParameter1 = oParameters1.ToArray();
                    if (oRemoteDB.Database.ExecuteSqlCommand(sItemSQL, vSqlParameter1) >= 1)
                    {
                        oTransaction.Commit();
                    }
                    else
                    {
                        oTransaction.Rollback();
                        ErrorMessage = Fields.itemandMappingDeleteFail;
                        bReturn = false;
                    }
                }
                else
                {
                    //Fail (RollBack)
                    oTransaction.Rollback();
                    ErrorMessage = Fields.itemandMappingDeleteFail;
                    bReturn = false;
                }
            }
            catch (Exception ex)
            {
                //RollBack  
                oTransaction.Rollback();
                ErrorMessage = ex.Message;
                bReturn = false;
            }
            //------------------------------
            return bReturn;
        }

        public bool Insert(Engine_REC poRecord)
        {
            bool bReturn = true;
            //------------------------------
            try
            {

                string sSQL = "" +
                    "INSERT INTO tblEngineMaster " +
                    "      ( " +
                    "      EneSerialNo,Model,CSN,TSN,CreatedDate,CreatedBy " +
                    "      ) " +
                    "      VALUES " +
                    "      (" +
                    "      @EneSerialNo,@Model,@CSN,@TSN,@CreatedDate,@CreatedBy " +
                    "      )" +
                    ";"
                    ;

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@EneSerialNo", poRecord.EneSerialNo));
                oParameters.Add(new SqlParameter("@Model", poRecord.Model));
                oParameters.Add(new SqlParameter("@CSN", poRecord.CSN));
                oParameters.Add(new SqlParameter("@TSN", poRecord.TSN));
                oParameters.Add(new SqlParameter("@CreatedDate", DateTime.Now));
                oParameters.Add(new SqlParameter("@CreatedBy", System.Web.HttpContext.Current.Session["UserID"].ToString()));
                SqlParameter[] vSqlParameter = oParameters.ToArray();

                DatabaseContext oRemoteDB = new DatabaseContext();
                int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

                if (nReturn == 1)
                {

                }
                else
                {
                    ErrorMessage = "Failed to insert record!";
                    bReturn = false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                bReturn = false;
            }
            //------------------------------
            return bReturn;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<ItemModel> GetPartByFilter(string[] ItemNoArray,string CheckAll)
        {
            string sSQL = string.Empty;
            DatabaseContext oRemoteDB = new DatabaseContext();

            if (CheckAll == "true")
            {
                sSQL = "" +
                            " SELECT " +
                            " * " +
                            "  FROM  tblPartMaster with (NOLOCK) " +
                            ";"
            ;
            }
            else
            {
                var ParameterFilter = "  (";

                for (int a = 0; a < ItemNoArray.Length; a++)
                {
                    if (a == ItemNoArray.Length - 1)
                        ParameterFilter += " PartItemNo ='" + ItemNoArray[a] + "' )";
                    else
                        ParameterFilter += " PartItemNo ='" + ItemNoArray[a] + "' or";
                }

                var ConditionFilter = "";
                sSQL = "" +
                            " SELECT " +
                            " * " +
                            "  FROM  tblPartMaster with (NOLOCK) WHERE " + ParameterFilter + ConditionFilter +
                            ";"
            ;
            }

            var vQuery = oRemoteDB.Database.SqlQuery<ItemModel>(sSQL);

            return vQuery;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<ItemPartMapping> GetPartDetailByFilter(string[] ItemNoArray, string CheckAll)
        {
            string sSQL = string.Empty;
            DatabaseContext oRemoteDB = new DatabaseContext();

            if (CheckAll == "true")
            {
                sSQL = "" +
                            " SELECT " +
                            " * " +
                            "  FROM  tblPartMasterDetails with (NOLOCK) " +
                            ";"
            ;
            }
            else
            {
                var ParameterFilter = " (";

                for (int a = 0; a < ItemNoArray.Length; a++)
                {
                    if (a == ItemNoArray.Length - 1)
                        ParameterFilter += " PartItemNo ='" + ItemNoArray[a] + "' )";
                    else
                        ParameterFilter += " PartItemNo ='" + ItemNoArray[a] + "' or";
                }

                var ConditionFilter = "";
                sSQL = "" +
                            " SELECT " +
                            " * " +
                            "  FROM  tblPartMasterDetails with (NOLOCK) WHERE " + ParameterFilter + ConditionFilter +
                            ";"
            ;
            }

            var vQuery = oRemoteDB.Database.SqlQuery<ItemPartMapping>(sSQL);

            return vQuery;
        }

        public bool InsertMasterEngine(Engine_REC poRecord,List<ItemModel> PartMaster,List<ItemPartMapping> partDetail)
        {
            bool bReturn = true;
            bool bEngineMaster = true;
            bool bEngineDetail = true;
            bool bEngineQCDetail = true;

            DatabaseContext oRemoteDB = new DatabaseContext();
            var oTransaction = oRemoteDB.Database.BeginTransaction();
            string sPartSQL = "";
            string sPartDetailSQL = "";

            //------------------------------
            try
            {
                string sSQL = "" +
                    "INSERT INTO tblEngineMaster " +
                    "      ( " +
                    "      EneSerialNo,Model,CSN,TSN,CreatedDate,CreatedBy " +
                    "      ) " +
                    "      VALUES " +
                    "      (" +
                    "      @EneSerialNo,@Model,@CSN,@TSN,@CreatedDate,@CreatedBy " +
                    "      )" +
                    ";"
                    ;

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@EneSerialNo", poRecord.EneSerialNo));
                oParameters.Add(new SqlParameter("@Model", (object)poRecord.Model ?? DBNull.Value));
                oParameters.Add(new SqlParameter("@CSN", (object)poRecord.CSN ?? DBNull.Value));
                oParameters.Add(new SqlParameter("@TSN", (object)poRecord.TSN ?? DBNull.Value));
                oParameters.Add(new SqlParameter("@CreatedDate", DateTime.Now));
                oParameters.Add(new SqlParameter("@CreatedBy", System.Web.HttpContext.Current.Session["UserID"].ToString()));
                SqlParameter[] vSqlParameter = oParameters.ToArray();

                if (oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter) > 0)
                {

                }
                else
                {
                    bEngineMaster = false;
                }

                if (bEngineMaster)
                {
                    if (PartMaster.Count > 0)
                    {
                        foreach (ItemModel model in PartMaster)
                        {
                            sPartSQL = "";
                            sPartSQL = "" +
                            "INSERT INTO tblEnginePartDetails " +
                            "      ( " +
                            "      EneSerialNo,PartItemNo,IPCRef,Nomenclature,PartType,PwelStd,PartStatus,Remarks,Photo,SN,CreatedDate,CreatedBy " +
                            "      ) " +
                            "      VALUES " +
                            "      (" +
                            "      @EneSerialNo,@PartItemNo,@IPCRef,@Nomenclature,@PartType,@PwelStd,@PartStatus,@Remarks,@Photo,@SN,@CreatedDate,@CreatedBy " +
                            "      )" +
                            ";"
                            ;

                            List<SqlParameter> oParameters1 = new List<SqlParameter>();
                            oParameters1.Add(new SqlParameter("@EneSerialNo", poRecord.EneSerialNo));
                            oParameters1.Add(new SqlParameter("@PartItemNo", model.PartItemNo));
                            oParameters1.Add(new SqlParameter("@IPCRef", model.IPCRef));
                            oParameters1.Add(new SqlParameter("@Nomenclature", model.Nomenclature));
                            oParameters1.Add(new SqlParameter("@PartType", model.PartType));
                            oParameters1.Add(new SqlParameter("@PwelStd", model.PwelStd));
                            oParameters1.Add(new SqlParameter("@PartStatus", model.PartStatus));
                            oParameters1.Add(new SqlParameter("@Remarks", model.Remarks));
                            oParameters1.Add(new SqlParameter("@Photo", (object)model.Photo ?? DBNull.Value));
                            oParameters1.Add(new SqlParameter("@SN", model.SN));
                            oParameters1.Add(new SqlParameter("@CreatedDate", DateTime.Now));
                            oParameters1.Add(new SqlParameter("@CreatedBy", System.Web.HttpContext.Current.Session["UserID"].ToString()));

                            SqlParameter[] vSqlParameter1 = oParameters1.ToArray();

                            if (oRemoteDB.Database.ExecuteSqlCommand(sPartSQL, vSqlParameter1) > 0)
                            {

                            }
                            else
                            {
                                bEngineDetail = false;
                            }
                        }

                        if (bEngineDetail)
                        {
                            if (partDetail.Count > 0)
                            {
                                foreach (ItemPartMapping mapping in partDetail)
                                {
                                    sPartDetailSQL = "";
                                    sPartDetailSQL = "" +
                                "INSERT INTO tblEnginePartQCDetails " +
                                "      ( " +
                                "      EneSerialNo,PartItemNo,VPN,PWPN,QCStatus " +
                                "      ) " +
                                "      VALUES " +
                                "      (" +
                                "      @EneSerialNo,@PartItemNo,@VPN,@PWPN,@QCStatus " +
                                "      )" +
                                ";"
                                ;

                                    List<SqlParameter> oParameters2 = new List<SqlParameter>();
                                    oParameters2.Add(new SqlParameter("@EneSerialNo", poRecord.EneSerialNo));
                                    oParameters2.Add(new SqlParameter("@PartItemNo", mapping.PartItemNo));
                                    oParameters2.Add(new SqlParameter("@VPN", (object)mapping.VPN ?? DBNull.Value));
                                    oParameters2.Add(new SqlParameter("@PWPN", (object)mapping.PWPN ?? DBNull.Value));
                                    oParameters2.Add(new SqlParameter("@QCStatus", "Not Completed"));

                                    SqlParameter[] vSqlParameter2 = oParameters2.ToArray();

                                    if (oRemoteDB.Database.ExecuteSqlCommand(sPartDetailSQL, vSqlParameter2) > 0)
                                    {

                                    }
                                    else
                                    {
                                        bEngineQCDetail = false;
                                    }
                                }

                                if (bEngineQCDetail)
                                {
                                    oTransaction.Commit();
                                    bReturn = true;
                                }
                                else
                                {
                                    oTransaction.Rollback();
                                    bReturn = false;
                                }
                            }
                            else
                            {
                                oTransaction.Commit();
                                bReturn = true;
                            }
                        }
                        else
                        {
                            oTransaction.Rollback();
                            bReturn = false;
                        }
                    }
                    else
                    {
                        oTransaction.Commit();
                        bReturn = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                oTransaction.Rollback();
                bReturn = false;
            }
            //------------------------------
            return bReturn;
        }

        public bool EngineDelete(Engine_REC poRecord)
        {
            bool bReturn = true;
            //------------------------------
            try
            {
                //----------
                string sSQL = "" +
                     " DELETE tblEnginePartQCDetails WHERE (EneSerialNo = @EneSerialNo);" +
                     " DELETE tblEnginePartDetails WHERE (EneSerialNo = @EneSerialNo);" +
                     " DELETE tblEngineMaster WHERE (EneSerialNo = @EneSerialNo);" +
                     " DELETE tblEngineBladeDetails WHERE (EneSerialNo = @EneSerialNo);" +
                     " DELETE tblEngineRigDetails WHERE (EneSerialNo = @EneSerialNo);"
                    ;

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@EneSerialNo", poRecord.EneSerialNo));
                SqlParameter[] vSqlParameter = oParameters.ToArray();

                DatabaseContext oRemoteDB = new DatabaseContext();
                var oTransaction = oRemoteDB.Database.BeginTransaction();
                int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
                if (nReturn >= 1)
                {
                    oTransaction.Commit();

                }
                else
                {
                    oTransaction.Rollback();
                    //ErrorMessage = nReturn.ToString() + "<br>" + "Failed to delete record!"  ;
                    ErrorMessage = "Failed to delete record!";

                    bReturn = false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                bReturn = false;
            }
            //------------------------------
            return bReturn;
        }

        public bool UpdateMasterEngine(Engine_REC poRecord, List<ItemModel> PartMaster, List<ItemPartMapping> partDetail,int CheckEngineDataCount)
        {
            bool bReturn = true;
            bool bEngineMaster = true;
            bool bEngineDetail = true;
            bool bEngineQCDetail = true;
            bool DeleteResult = true;

            DatabaseContext oRemoteDB = new DatabaseContext();
            var oTransaction = oRemoteDB.Database.BeginTransaction();
            string sPartSQL = "";
            string sPartDetailSQL = "";
            string DeleteSQL = "";

            //------------------------------
            try
            {
                string sSQL = "" +
                    " UPDATE tblEngineMaster  " +
                    "  SET " +
                     "     Model=@Model,CSN=@CSN,TSN=@TSN,UpdatedDate=@UpdatedDate,UpdatedBy=@UpdatedBy " +
                    "      WHERE (EneSerialNo=@EneSerialNo) " +
                    ";"
                    ;

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@EneSerialNo", poRecord.EneSerialNo));
                oParameters.Add(new SqlParameter("@Model", (object)poRecord.Model ?? DBNull.Value));
                oParameters.Add(new SqlParameter("@CSN", (object)poRecord.CSN ?? DBNull.Value));
                oParameters.Add(new SqlParameter("@TSN", (object)poRecord.TSN ?? DBNull.Value));
                oParameters.Add(new SqlParameter("@UpdatedDate", DateTime.Now));
                oParameters.Add(new SqlParameter("@UpdatedBy", System.Web.HttpContext.Current.Session["UserID"].ToString()));
                SqlParameter[] vSqlParameter = oParameters.ToArray();

                if (oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter) > 0)
                {

                }
                else
                {
                    bEngineMaster = false;
                }

                if (bEngineMaster)
                {
                    if (CheckEngineDataCount > 0)
                    {
                        DeleteSQL = "" +
                        " DELETE tblEnginePartQCDetails WHERE (EneSerialNo = @EneSerialNo);" +
                        " DELETE tblEnginePartDetails WHERE (EneSerialNo = @EneSerialNo);"
                       ;

                        List<SqlParameter> oParameterdelete = new List<SqlParameter>();
                        oParameterdelete.Add(new SqlParameter("@EneSerialNo", poRecord.EneSerialNo));
                        SqlParameter[] vSqlParameterdelete = oParameterdelete.ToArray();

                        if (oRemoteDB.Database.ExecuteSqlCommand(DeleteSQL, vSqlParameterdelete) > 0)
                        {

                        }
                        else
                        {
                            DeleteResult = false;
                        }
                    }

                    if (DeleteResult)
                    {
                        if (PartMaster.Count > 0)
                        {
                            foreach (ItemModel model in PartMaster)
                            {
                                sPartSQL = "";
                                sPartSQL = "" +
                                "INSERT INTO tblEnginePartDetails " +
                                "      ( " +
                                "      EneSerialNo,PartItemNo,IPCRef,Nomenclature,PartType,PwelStd,PartStatus,Remarks,Photo,SN,CreatedDate,CreatedBy " +
                                "      ) " +
                                "      VALUES " +
                                "      (" +
                                "      @EneSerialNo,@PartItemNo,@IPCRef,@Nomenclature,@PartType,@PwelStd,@PartStatus,@Remarks,@Photo,@SN,@CreatedDate,@CreatedBy " +
                                "      )" +
                                ";"
                                ;

                                List<SqlParameter> oParameters1 = new List<SqlParameter>();
                                oParameters1.Add(new SqlParameter("@EneSerialNo", poRecord.EneSerialNo));
                                oParameters1.Add(new SqlParameter("@PartItemNo", model.PartItemNo));
                                oParameters1.Add(new SqlParameter("@IPCRef", model.IPCRef));
                                oParameters1.Add(new SqlParameter("@Nomenclature", model.Nomenclature));
                                oParameters1.Add(new SqlParameter("@PartType", model.PartType));
                                oParameters1.Add(new SqlParameter("@PwelStd", model.PwelStd));
                                oParameters1.Add(new SqlParameter("@PartStatus", model.PartStatus));
                                oParameters1.Add(new SqlParameter("@Remarks", model.Remarks));
                                oParameters1.Add(new SqlParameter("@Photo", (object)model.Photo ?? DBNull.Value));
                                oParameters1.Add(new SqlParameter("@SN", model.SN));
                                oParameters1.Add(new SqlParameter("@CreatedDate", DateTime.Now));
                                oParameters1.Add(new SqlParameter("@CreatedBy", System.Web.HttpContext.Current.Session["UserID"].ToString()));

                                SqlParameter[] vSqlParameter1 = oParameters1.ToArray();

                                if (oRemoteDB.Database.ExecuteSqlCommand(sPartSQL, vSqlParameter1) > 0)
                                {

                                }
                                else
                                {
                                    bEngineDetail = false;
                                }
                            }

                            if (bEngineDetail)
                            {
                                if (partDetail.Count > 0)
                                {
                                    foreach (ItemPartMapping mapping in partDetail)
                                    {
                                        sPartDetailSQL = "";
                                        sPartDetailSQL = "" +
                                    "INSERT INTO tblEnginePartQCDetails " +
                                    "      ( " +
                                    "      EneSerialNo,PartItemNo,VPN,PWPN,QCStatus " +
                                    "      ) " +
                                    "      VALUES " +
                                    "      (" +
                                    "      @EneSerialNo,@PartItemNo,@VPN,@PWPN,@QCStatus " +
                                    "      )" +
                                    ";"
                                    ;

                                        List<SqlParameter> oParameters2 = new List<SqlParameter>();
                                        oParameters2.Add(new SqlParameter("@EneSerialNo", poRecord.EneSerialNo));
                                        oParameters2.Add(new SqlParameter("@PartItemNo", mapping.PartItemNo));
                                        oParameters2.Add(new SqlParameter("@VPN", (object)mapping.VPN ?? DBNull.Value));
                                        oParameters2.Add(new SqlParameter("@PWPN", (object)mapping.PWPN ?? DBNull.Value));
                                        oParameters2.Add(new SqlParameter("@QCStatus", "Not Completed"));

                                        SqlParameter[] vSqlParameter2 = oParameters2.ToArray();

                                        if (oRemoteDB.Database.ExecuteSqlCommand(sPartDetailSQL, vSqlParameter2) > 0)
                                        {

                                        }
                                        else
                                        {
                                            bEngineQCDetail = false;
                                        }
                                    }

                                    if (bEngineQCDetail)
                                    {
                                        oTransaction.Commit();
                                        bReturn = true;
                                    }
                                    else
                                    {
                                        oTransaction.Rollback();
                                        bReturn = false;
                                    }
                                }
                                else
                                {
                                    oTransaction.Commit();
                                    bReturn = true;
                                }
                            }
                            else
                            {
                                oTransaction.Rollback();
                                bReturn = false;
                            }
                        }
                        else
                        {
                            oTransaction.Commit();
                            bReturn = true;
                        }
                    }                  
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                oTransaction.Rollback();
                bReturn = false;
            }
            //------------------------------
            return bReturn;
        }

        public bool EngineUpdateByFirstDelete(Engine_REC poRecord)
        {
            bool bReturn = true;
            //------------------------------
            try
            {
                //----------
                string sSQL = "" +
                     " DELETE tblEnginePartQCDetails WHERE (EneSerialNo = @EneSerialNo);" +
                     " DELETE tblEnginePartDetails WHERE (EneSerialNo = @EneSerialNo);"
                    ;

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@EneSerialNo", poRecord.EneSerialNo));
                SqlParameter[] vSqlParameter = oParameters.ToArray();

                DatabaseContext oRemoteDB = new DatabaseContext();
                var oTransaction = oRemoteDB.Database.BeginTransaction();
                int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
                if (nReturn >= 1)
                {
                    oTransaction.Commit();

                }
                else
                {
                    oTransaction.Rollback();
                    //ErrorMessage = nReturn.ToString() + "<br>" + "Failed to delete record!"  ;
                    ErrorMessage = "Failed to delete record!";

                    bReturn = false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                bReturn = false;
            }
            //------------------------------
            return bReturn;
        }

        #endregion
    }
}