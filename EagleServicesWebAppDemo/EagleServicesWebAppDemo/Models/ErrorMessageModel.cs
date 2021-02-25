using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EagleServicesWebApp.Components;

namespace EagleServicesWebApp.Models
{
    public class LogInMsg
    {
        public string NewPwd_Empty { get; set; }
        public string textbox_Noti { get; set; }
        public string Data_Add_Success { get; set; }
        public string Data_Update_Success { get; set; }
        public string Data_Delete_Success { get; set; }
        public string success_title { get; set; }
        public string success_Noti { get; set; }
        public string LogIn_Success { get; set; }
        public string LogIn_Failed { get; set; }
        public string LogIn_Invalid { get; set; }
        public string LogIn_InActive { get; set; }
        public string LogIn_ChangePwd { get; set; }
        public string Pwd_Empty { get; set; }
        public string ConfirmPwd_Empty { get; set; }
        public string Pwd_Length_Validation { get; set; }
        public string ConfirmPwd_Length_Validation { get; set; }
        public string NotEqual_Pwd { get; set; }
        public string Pwd_UpdateInfo { get; set; }
        public string DefaultPwd_resetInfo { get; set; }
        public string UserID_Inused { get; set; }
        public string AssetNumberdigits { get; set; }
}

    public class CommonUseMsg
    {
        //public string App_Permission_Error_text { get; set; }
        //public string App_Permission_Error_Desp_text { get; set; }

        public string textbox_Noti { get; set; }
        public string Warning_Noti { get; set; }
        public string info_Noti { get; set; }
        public string success_Noti { get; set; }
        public string danger_Noti { get; set; }
        public string errorcode_text { get; set; }
        public string email_errorcode_text { get; set; }
        public string title_text { get; set; }
        public string App_Permission_Error_text { get; set; }
        public string msg_text { get; set; }
        public string info_text { get; set; }
        public string form_validation_text { get; set; }
        public string no_errorcode { get; set; }
        public string error_code { get; set; }
        public string success_title { get; set; }
        public string failed_title { get; set; }
        public string Data_Add_Success { get; set; }
        public string Data_Update_Success { get; set; }
        public string Data_Delete_Success { get; set; }
        public string primary_key_constraint_error { get; set; }
        public string Permission_constraint_error { get; set; }
        public string Ok_text { get; set; }
        public string Error_text { get; set; }
        public string SQL_Insert_Fail_Error { get; set; }
        public string SQL_Delete_Fail_Error { get; set; }
        public string SQL_Delete_Fail_Error1 { get; set; }
        public string SQL_Update_Fail_Error { get; set; }
        public string SQL_Update_Fail_Error1 { get; set; }
        public string LoadingImagePath { get; set; }
        public string DefaultPhotoName { get; set; }
        public string logo_Path { get; set; }
        public string DuplicationFail { get; set; }
        public string AssetNumberdigits { get; set; }

    }

    public class DashboardMsg
    {
        public string Overall { get; set; }
        public string Molding { get; set; }
        public string Plating { get; set; }
        public string Stamping { get; set; }
        public string Assembly { get; set; }
    }

    public class DepartmentMapingMsg
    {
        public string UserID_Empty { get; set; }
        public string Department_Empty { get; set; }
        public string textbox_Noti { get; set; }
        public string Data_Add_Success { get; set; }
        public string Data_Update_Success { get; set; }
        public string Data_Delete_Success { get; set; }
        public string success_title { get; set; }
        public string success_Noti { get; set; }
        public string UserID_Inused { get; set; }
        public string Warning_Noti { get; set; }
        public string info_Noti { get; set; }
        public string danger_Noti { get; set; }

    }
    public class GeneralTableMsg
    {
        public string textbox_Noti { get; set; }
        public string Data_Still_Using { get; set; }
        public string Data_AlreadyExit { get; set; }
        public string Data_Add_Success { get; set; }
        public string Data_Update_Success { get; set; }
        public string Data_Delete_Success { get; set; }
        public string GeneralTable_Req { get; set; }
        public string Map_Department_Req { get; set; }
        public string Map_CostCenter_Req { get; set; }
        public string Map_both_Req { get; set; }
        public string DepartmentMapping { get; set; }
        public string Department { get; set; }
        public string CostCenter { get; set; }
        public string DEPARTMENT { get; set; }
        public string AssetPIC { get; set; }
        public string PAR { get; set; }
        public string Responser { get; set; }
        public string AssetClass { get; set; }
        public string ActivityType { get; set; }
        public string VendorCode { get; set; }
        public string CustodyPerson { get; set; }
        public string CustodyPeron { get; set; }
        public string Location { get; set; }
        public string Room { get; set; }
        public string TaxIncentive { get; set; }

    }

    public class EnquiryMsg
    {
        public string AssetNumberdigits { get; set; }
        public string success_title { get; set; }
        public string failed_title { get; set; }
        public string success_Noti { get; set; }
        public string Data_Add_Success { get; set; }
        public string Data_Update_Success { get; set; }
        public string Data_Delete_Success { get; set; }
        public string danger_Noti { get; set; }
        public string textbox_Noti { get; set; }
        public string NewAsset_Successfully_Save { get; set; }
        public string AssetNo_Dup_msg { get; set; }
        public string DefaultPhotoPath { get; set; }
        public string UploadPhotoPath { get; set; }

        public string CostCenter_empty_Noti { get; set; }
        public string Department_empty_Noti { get; set; }
        public string AcqDate_empty_Noti { get; set; }
        public string RFIDID_empty_Noti { get; set; }
        public string RFID_Dup_Noti { get; set; }
        public string NFSAssetDesc_empty_Noti { get; set; }
        public string ILMSAssetDesc_empty_Noti { get; set; }

        public string FixedAssetPIC_empty_Noti { get; set; }
        public string AssetNo_empty_Noti { get; set; }
        public string CapitalizationPeriod_empty_Noti { get; set; }
        public string AcquisitionValue_empty_Noti { get; set; }
        public string AssetStatus_empty_Noti { get; set; }
        public string AssetClass_empty_Noti { get; set; }
        public string ActivityType_empty_Noti { get; set; }
        public string Responser_empty_Noti { get; set; }
        public string AssetName1_empty_Noti { get; set; }
        public string Scrap_empty_Noti { get; set; }
        public string Quantity_empty_Noti { get; set; }
        public string Location_empty_Noti { get; set; }
        public string Invalid_Asset_Cat_iLMS { get; set; }
        public string Room1_empty_Noti { get; set; }
        public string PAR_empty_Noti { get; set; }
        public string IONumber_empty_Noti { get; set; }
        public string IODescription_empty_Noti { get; set; }
        public string IOCreationYear_empty_Noti { get; set; }
        public string CustFunding_empty_Noti { get; set; }
        public string Invalid_SubAsset_Noti { get; set; }
        public string InvalidMainAsset_Noti { get; set; }
        public string Prefix_notMatch_Noti { get; set; }
        public string AssetNumber_notMatch_Noti { get; set; }
        public string AlreadyChoose_SubAsset_Noti { get; set; }
        public string MainAsset_Add_Success { get; set; }
        public string AssetNumber_AlreadyExit { get; set; }
        public string RFIDGdigit { get; set; }
        public string ScrapFormNoEmpty_Noti { get; set; }
        public string History_Action_NewAssetCreation { get; set; }
        public string History_Action_AssetUpdate { get; set; }
        public string History_New_log { get; set; }
        public string History_Transfer_Log { get; set; }
        public string History_Scrap_Log { get; set; }
        public string History_Update_Log { get; set; }
        public string ScrapStatusChange_Noti { get; set; }
        public string Favemsg { get; set; }
    }

    public class UserAccessRightMsg
    {
        public string NoMenu_Noti { get; set; }
        public string textbox_Noti { get; set; }
        public string success_title { get; set; }
        public string Data_Delete_Success { get; set; }
        public string success_Noti { get; set; }
        public string danger_Noti { get; set; }
        public string Warning_Noti { get; set; }
        public string UserAcces_Delete_Inused { get; set; }
        public string UserRole_Empty_Noti { get; set; }
        public string UserRole_Dup_Noti { get; set; }
        public string DataAccess_Empty_Noti { get; set; }
        public string Deparment_Empty_Noti { get; set; }
        public string NoDeparment_Noti { get; set; }
        public string DataAccess_Empty_Notis { get; set; }
        public string AdminUserRole_Delete_Notis { get; set; }
        public string SelectAllDep_msg { get; set; }
        public string PICDep_SelectMorethanOne_msg { get; set; }
        public string SelectAllDep_SelectAll_msgNotify { get; set; }
        public string AllDep_Select_msgNotify { get; set; } 
        public string Common_Empty_Noti { get; set; }
    }

    public class ErrorMessageModel
    {
        public int ErrorNo { get; set; }
        public string ErrorMessage { get; set; }

        public ErrorMessageModel()
        {
            ErrorNo = 0;
            ErrorMessage = "";
        }

        public LogInMsg LogInMsgSelect()
        {
            LogInMsg LogInMsg = new LogInMsg();

            LogInMsg.NewPwd_Empty = GlobalFunction.GetStatus_MsgByKey("NewPwd_Empty");
            LogInMsg.ConfirmPwd_Length_Validation = GlobalFunction.GetStatus_MsgByKey("ConfirmPwd_Length_Validation");
            LogInMsg.textbox_Noti = GlobalFunction.GetStatus_MsgByKey("textbox_Noti");
            LogInMsg.Data_Add_Success = GlobalFunction.GetStatus_MsgByKey("Data_Add_Success");
            LogInMsg.Data_Update_Success = GlobalFunction.GetStatus_MsgByKey("Data_Update_Success");
            LogInMsg.Data_Delete_Success = GlobalFunction.GetStatus_MsgByKey("Data_Delete_Success");
            LogInMsg.success_title = GlobalFunction.GetStatus_MsgByKey("success_title");
            LogInMsg.success_Noti = GlobalFunction.GetStatus_MsgByKey("success_Noti");
            LogInMsg.LogIn_Success = GlobalFunction.GetStatus_MsgByKey("LogIn_Success");
            LogInMsg.LogIn_Failed = GlobalFunction.GetStatus_MsgByKey("LogIn_Failed");
            LogInMsg.LogIn_Invalid = GlobalFunction.GetStatus_MsgByKey("LogIn_Invalid");
            LogInMsg.LogIn_InActive = GlobalFunction.GetStatus_MsgByKey("LogIn_InActive");
            LogInMsg.LogIn_ChangePwd = GlobalFunction.GetStatus_MsgByKey("LogIn_ChangePwd");
            LogInMsg.Pwd_Empty = GlobalFunction.GetStatus_MsgByKey("Pwd_Empty");
            LogInMsg.ConfirmPwd_Empty = GlobalFunction.GetStatus_MsgByKey("ConfirmPwd_Empty");
            LogInMsg.Pwd_Length_Validation = GlobalFunction.GetStatus_MsgByKey("Pwd_Length_Validation");
            LogInMsg.NotEqual_Pwd = GlobalFunction.GetStatus_MsgByKey("NotEqual_Pwd");
            LogInMsg.Pwd_UpdateInfo = GlobalFunction.GetStatus_MsgByKey("Pwd_UpdateInfo");
            LogInMsg.DefaultPwd_resetInfo = GlobalFunction.GetStatus_MsgByKey("DefaultPwd_resetInfo");
            LogInMsg.UserID_Inused = GlobalFunction.GetStatus_MsgByKey("UserID_Inused");

            return LogInMsg;
        }

        public CommonUseMsg CommonUseMsgSelect()
        {
            CommonUseMsg CommonUseMsg = new CommonUseMsg();
            CommonUseMsg.AssetNumberdigits = GlobalFunction.GetStatus_MsgByKey("AssetNumberdigits");
            CommonUseMsg.textbox_Noti = GlobalFunction.GetStatus_MsgByKey("textbox_Noti");
            CommonUseMsg.Warning_Noti = GlobalFunction.GetStatus_MsgByKey("Warning_Noti");
            CommonUseMsg.info_Noti  = GlobalFunction.GetStatus_MsgByKey("info_Noti");
            CommonUseMsg.success_Noti = GlobalFunction.GetStatus_MsgByKey("success_Noti");
            CommonUseMsg.danger_Noti = GlobalFunction.GetStatus_MsgByKey("danger_Noti");
            CommonUseMsg.errorcode_text = GlobalFunction.GetStatus_MsgByKey("errorcode_text");
            CommonUseMsg.email_errorcode_text = GlobalFunction.GetStatus_MsgByKey("email_errorcode_text");
            CommonUseMsg.title_text = GlobalFunction.GetStatus_MsgByKey("title_text");
            CommonUseMsg.msg_text = GlobalFunction.GetStatus_MsgByKey("msg_text");
            CommonUseMsg.info_text = GlobalFunction.GetStatus_MsgByKey("info_text");
            CommonUseMsg.form_validation_text = GlobalFunction.GetStatus_MsgByKey("form_validation_text");
            CommonUseMsg.no_errorcode = GlobalFunction.GetStatus_MsgByKey("no_errorcode");
            CommonUseMsg.error_code = GlobalFunction.GetStatus_MsgByKey("error_code");
            CommonUseMsg.success_title = GlobalFunction.GetStatus_MsgByKey("success_title");
            CommonUseMsg.failed_title = GlobalFunction.GetStatus_MsgByKey("failed_title");
            CommonUseMsg.Data_Add_Success = GlobalFunction.GetStatus_MsgByKey("Data_Add_Success");
            CommonUseMsg.Data_Update_Success = GlobalFunction.GetStatus_MsgByKey("Data_Update_Success");
            CommonUseMsg.Data_Delete_Success = GlobalFunction.GetStatus_MsgByKey("Data_Delete_Success");
            CommonUseMsg.primary_key_constraint_error = GlobalFunction.GetStatus_MsgByKey("primary_key_constraint_error");
            CommonUseMsg.Permission_constraint_error = GlobalFunction.GetStatus_MsgByKey("Permission_constraint_error");
            CommonUseMsg.Ok_text = GlobalFunction.GetStatus_MsgByKey("Ok_text");
            CommonUseMsg.Error_text = GlobalFunction.GetStatus_MsgByKey("Error_text");
            CommonUseMsg.SQL_Insert_Fail_Error = GlobalFunction.GetStatus_MsgByKey("SQL_Insert_Fail_Error");
            CommonUseMsg.SQL_Delete_Fail_Error = GlobalFunction.GetStatus_MsgByKey("SQL_Delete_Fail_Error");
            CommonUseMsg.SQL_Delete_Fail_Error1 = GlobalFunction.GetStatus_MsgByKey("SQL_Delete_Fail_Error1");
            CommonUseMsg.SQL_Update_Fail_Error = GlobalFunction.GetStatus_MsgByKey("SQL_Update_Fail_Error");
            CommonUseMsg.SQL_Update_Fail_Error1 = GlobalFunction.GetStatus_MsgByKey("SQL_Update_Fail_Error1");
            CommonUseMsg.LoadingImagePath = GlobalFunction.GetStatus_MsgByKey("LoadingImagePath");
            CommonUseMsg.DefaultPhotoName = GlobalFunction.GetStatus_MsgByKey("DefaultPhotoName");
            CommonUseMsg.logo_Path = GlobalFunction.GetStatus_MsgByKey("logo_Path"); 
            CommonUseMsg.DuplicationFail = GlobalFunction.GetStatus_MsgByKey("DuplicationFail");
            return CommonUseMsg;
        }

        public DashboardMsg DashboardMsgSelect()
        {
            DashboardMsg DashboardMsg = new DashboardMsg();
            DashboardMsg.Overall = GlobalFunction.GetStatus_MsgByKey("Overall");
            DashboardMsg.Molding = GlobalFunction.GetStatus_MsgByKey("Molding");
            DashboardMsg.Plating = GlobalFunction.GetStatus_MsgByKey("Plating");
            DashboardMsg.Stamping = GlobalFunction.GetStatus_MsgByKey("Stamping");
            DashboardMsg.Assembly = GlobalFunction.GetStatus_MsgByKey("Assembly");
            return DashboardMsg;
        }

        public GeneralTableMsg GeneralTableMsgSelect()
        {
            GeneralTableMsg GeneralTableMsg = new GeneralTableMsg();
            GeneralTableMsg.textbox_Noti = GlobalFunction.GetStatus_MsgByKey("textbox_Noti");
            GeneralTableMsg.Data_Still_Using = GlobalFunction.GetStatus_MsgByKey("Data_Still_Using");
            GeneralTableMsg.Data_AlreadyExit = GlobalFunction.GetStatus_MsgByKey("Data_AlreadyExit");
            GeneralTableMsg.Data_Add_Success  =GlobalFunction.GetStatus_MsgByKey("Data_Add_Success");
            GeneralTableMsg.Data_Update_Success  =GlobalFunction.GetStatus_MsgByKey("Data_Update_Success");
            GeneralTableMsg.Data_Delete_Success  =GlobalFunction.GetStatus_MsgByKey("Data_Delete_Success");
            GeneralTableMsg.GeneralTable_Req  =GlobalFunction.GetStatus_MsgByKey("GeneralTable_Req");
            GeneralTableMsg.Map_Department_Req  =GlobalFunction.GetStatus_MsgByKey("Map_Department_Req");
            GeneralTableMsg.Map_CostCenter_Req  =GlobalFunction.GetStatus_MsgByKey("Map_CostCenter_Req");
            GeneralTableMsg.Map_both_Req  =GlobalFunction.GetStatus_MsgByKey("Map_both_Req");
            GeneralTableMsg.DepartmentMapping  =GlobalFunction.GetStatus_MsgByKey("DEPARTMENTMAPPING");
            GeneralTableMsg.Department  =GlobalFunction.GetStatus_MsgByKey("Department");
            GeneralTableMsg.CostCenter  =GlobalFunction.GetStatus_MsgByKey("CostCenter");
            GeneralTableMsg.AssetPIC  =GlobalFunction.GetStatus_MsgByKey("AssetPIC");
            GeneralTableMsg.PAR  =GlobalFunction.GetStatus_MsgByKey("PAR");
            GeneralTableMsg.Responser  =GlobalFunction.GetStatus_MsgByKey("Responser");
            GeneralTableMsg.AssetClass  =GlobalFunction.GetStatus_MsgByKey("AssetClass");
            GeneralTableMsg.ActivityType  =GlobalFunction.GetStatus_MsgByKey("ActivityType");
            GeneralTableMsg.VendorCode  =GlobalFunction.GetStatus_MsgByKey("VendorCode");
            GeneralTableMsg.CustodyPerson  =GlobalFunction.GetStatus_MsgByKey("CustodyPerson");
            GeneralTableMsg.CustodyPeron  =GlobalFunction.GetStatus_MsgByKey("CustodyPeron");
            GeneralTableMsg.Location  =GlobalFunction.GetStatus_MsgByKey("Location");
            GeneralTableMsg.Room  =GlobalFunction.GetStatus_MsgByKey("Room");
            GeneralTableMsg.TaxIncentive = GlobalFunction.GetStatus_MsgByKey("TaxIncentive");

            return GeneralTableMsg;
        }

        public EnquiryMsg EnquiryMsgSelect()
        {
            EnquiryMsg EnquiryMsg = new EnquiryMsg();
            EnquiryMsg.Favemsg = GlobalFunction.GetStatus_MsgByKey("Favemsg");
            EnquiryMsg.AssetNumberdigits = GlobalFunction.GetStatus_MsgByKey("AssetNumberdigits");
            EnquiryMsg.ScrapStatusChange_Noti = GlobalFunction.GetStatus_MsgByKey("ScrapStatusChange_Noti");
            EnquiryMsg.ScrapFormNoEmpty_Noti = GlobalFunction.GetStatus_MsgByKey("ScrapFormNoEmpty_Noti");
            EnquiryMsg.success_Noti = GlobalFunction.GetStatus_MsgByKey("success_Noti");
            EnquiryMsg.danger_Noti = GlobalFunction.GetStatus_MsgByKey("danger_Noti");
            EnquiryMsg.textbox_Noti = GlobalFunction.GetStatus_MsgByKey("textbox_Noti");
            EnquiryMsg.NewAsset_Successfully_Save = GlobalFunction.GetStatus_MsgByKey("NewAsset_Successfully_Save");
            EnquiryMsg.AssetNo_Dup_msg = GlobalFunction.GetStatus_MsgByKey("AssetNo_Dup_msg");
            EnquiryMsg.DefaultPhotoPath = GlobalFunction.GetStatus_MsgByKey("DefaultPhotoPath");
            EnquiryMsg.UploadPhotoPath = GlobalFunction.GetStatus_MsgByKey("UploadPhotoPath");
            EnquiryMsg.CostCenter_empty_Noti = GlobalFunction.GetStatus_MsgByKey("CostCenter_empty_Noti");
            EnquiryMsg.Department_empty_Noti = GlobalFunction.GetStatus_MsgByKey("Department_empty_Noti");
            EnquiryMsg.AcqDate_empty_Noti = GlobalFunction.GetStatus_MsgByKey("AcqDate_empty_Noti");
            EnquiryMsg.RFIDID_empty_Noti = GlobalFunction.GetStatus_MsgByKey("RFIDID_empty_Noti");
            EnquiryMsg.RFID_Dup_Noti = GlobalFunction.GetStatus_MsgByKey("RFID_Dup_Noti");
            EnquiryMsg.NFSAssetDesc_empty_Noti = GlobalFunction.GetStatus_MsgByKey("NFSAssetDesc_empty_Noti");
            EnquiryMsg.ILMSAssetDesc_empty_Noti = GlobalFunction.GetStatus_MsgByKey("ILMSAssetDesc_empty_Noti");

            EnquiryMsg.Data_Add_Success = GlobalFunction.GetStatus_MsgByKey("Data_Add_Success");
            EnquiryMsg.Data_Update_Success = GlobalFunction.GetStatus_MsgByKey("Data_Update_Success");
            EnquiryMsg.Data_Delete_Success = GlobalFunction.GetStatus_MsgByKey("Data_Delete_Success");
            EnquiryMsg.success_title = GlobalFunction.GetStatus_MsgByKey("success_title");
            EnquiryMsg.failed_title = GlobalFunction.GetStatus_MsgByKey("failed_title");

            EnquiryMsg.FixedAssetPIC_empty_Noti = GlobalFunction.GetStatus_MsgByKey("FixedAssetPIC_empty_Noti");
            EnquiryMsg.AssetNo_empty_Noti = GlobalFunction.GetStatus_MsgByKey("AssetNo_empty_Noti");
            EnquiryMsg.CapitalizationPeriod_empty_Noti = GlobalFunction.GetStatus_MsgByKey("CapitalizationPeriod_empty_Noti");
            EnquiryMsg.AcquisitionValue_empty_Noti = GlobalFunction.GetStatus_MsgByKey("AcquisitionValue_empty_Noti");
            EnquiryMsg.AssetStatus_empty_Noti = GlobalFunction.GetStatus_MsgByKey("AssetStatus_empty_Noti");
            EnquiryMsg.AssetClass_empty_Noti = GlobalFunction.GetStatus_MsgByKey("AssetClass_empty_Noti");
            EnquiryMsg.ActivityType_empty_Noti = GlobalFunction.GetStatus_MsgByKey("ActivityType_empty_Noti");
            EnquiryMsg.Responser_empty_Noti = GlobalFunction.GetStatus_MsgByKey("Responser_empty_Noti");
            EnquiryMsg.AssetName1_empty_Noti = GlobalFunction.GetStatus_MsgByKey("AssetName1_empty_Noti");
            EnquiryMsg.Scrap_empty_Noti = GlobalFunction.GetStatus_MsgByKey("Scrap_empty_Noti");
            EnquiryMsg.Quantity_empty_Noti = GlobalFunction.GetStatus_MsgByKey("Quantity_empty_Noti");
            EnquiryMsg.Invalid_Asset_Cat_iLMS = GlobalFunction.GetStatus_MsgByKey("Invalid_Asset_Cat_iLMS");
            EnquiryMsg.Location_empty_Noti = GlobalFunction.GetStatus_MsgByKey("Location_empty_Noti");
            EnquiryMsg.Room1_empty_Noti = GlobalFunction.GetStatus_MsgByKey("Room1_empty_Noti");
            EnquiryMsg.PAR_empty_Noti = GlobalFunction.GetStatus_MsgByKey("PAR_empty_Noti");
            EnquiryMsg.IONumber_empty_Noti = GlobalFunction.GetStatus_MsgByKey("IONumber_empty_Noti");
            EnquiryMsg.IODescription_empty_Noti = GlobalFunction.GetStatus_MsgByKey("IODescription_empty_Noti");
            EnquiryMsg.IOCreationYear_empty_Noti = GlobalFunction.GetStatus_MsgByKey("IOCreationYear_empty_Noti");
            EnquiryMsg.CustFunding_empty_Noti = GlobalFunction.GetStatus_MsgByKey("CustFunding_empty_Noti");
            EnquiryMsg.Invalid_SubAsset_Noti = GlobalFunction.GetStatus_MsgByKey("Invalid_SubAsset_Noti");
            EnquiryMsg.InvalidMainAsset_Noti = GlobalFunction.GetStatus_MsgByKey("InvalidMainAsset_Noti");
            EnquiryMsg.Prefix_notMatch_Noti = GlobalFunction.GetStatus_MsgByKey("Prefix_notMatch_Noti");
            EnquiryMsg.AssetNumber_notMatch_Noti = GlobalFunction.GetStatus_MsgByKey("AssetNumber_notMatch_Noti");
            EnquiryMsg.AlreadyChoose_SubAsset_Noti = GlobalFunction.GetStatus_MsgByKey("AlreadyChoose_SubAsset_Noti");
            EnquiryMsg.MainAsset_Add_Success = GlobalFunction.GetStatus_MsgByKey("MainAsset_Add_Success");
            EnquiryMsg.AssetNumber_AlreadyExit = GlobalFunction.GetStatus_MsgByKey("AssetNumber_AlreadyExit");
            EnquiryMsg.RFIDGdigit = GlobalFunction.GetStatus_MsgByKey("RFIDGdigit");
            EnquiryMsg.History_Action_NewAssetCreation = GlobalFunction.GetStatus_MsgByKey("History_Action_NewAssetCreation");
            EnquiryMsg.History_Action_AssetUpdate = GlobalFunction.GetStatus_MsgByKey("History_Action_AssetUpdate");
            EnquiryMsg.History_New_log = GlobalFunction.GetStatus_MsgByKey("History_New_log");
            EnquiryMsg.History_Transfer_Log = GlobalFunction.GetStatus_MsgByKey("History_Transfer_Log");
            EnquiryMsg.History_Scrap_Log = GlobalFunction.GetStatus_MsgByKey("History_Scrap_Log");
            EnquiryMsg.History_Update_Log = GlobalFunction.GetStatus_MsgByKey("History_Update_Log");

            return EnquiryMsg;
        }

        public UserAccessRightMsg UserAccessRightMsgSelect()
        {
            UserAccessRightMsg UserAccessRightMsg = new UserAccessRightMsg();
            UserAccessRightMsg.NoMenu_Noti = GlobalFunction.GetStatus_MsgByKey("NoMenu_Noti");
            UserAccessRightMsg.textbox_Noti = GlobalFunction.GetStatus_MsgByKey("textbox_Noti");
            UserAccessRightMsg.success_title = GlobalFunction.GetStatus_MsgByKey("success_title");
            UserAccessRightMsg.Data_Delete_Success = GlobalFunction.GetStatus_MsgByKey("Data_Delete_Success");
            UserAccessRightMsg.success_Noti = GlobalFunction.GetStatus_MsgByKey("success_Noti");
            UserAccessRightMsg.danger_Noti = GlobalFunction.GetStatus_MsgByKey("danger_Noti");
            UserAccessRightMsg.Warning_Noti = GlobalFunction.GetStatus_MsgByKey("Warning_Noti");
            UserAccessRightMsg.UserAcces_Delete_Inused = GlobalFunction.GetStatus_MsgByKey("UserAcces_Delete_Inused");
            UserAccessRightMsg.UserRole_Empty_Noti = GlobalFunction.GetStatus_MsgByKey("UserRole_Empty_Noti");
            UserAccessRightMsg.UserRole_Dup_Noti = GlobalFunction.GetStatus_MsgByKey("UserRole_Dup_Noti");
            UserAccessRightMsg.DataAccess_Empty_Noti = GlobalFunction.GetStatus_MsgByKey("DataAccess_Empty_Noti");
            UserAccessRightMsg.Deparment_Empty_Noti = GlobalFunction.GetStatus_MsgByKey("Deparment_Empty_Noti");
            UserAccessRightMsg.DataAccess_Empty_Notis = GlobalFunction.GetStatus_MsgByKey("DataAccess_Empty_Notis");
            UserAccessRightMsg.AdminUserRole_Delete_Notis = GlobalFunction.GetStatus_MsgByKey("AdminUserRole_Delete_Notis");
            UserAccessRightMsg.SelectAllDep_msg = GlobalFunction.GetStatus_MsgByKey("SelectAllDep_msg");
            UserAccessRightMsg.PICDep_SelectMorethanOne_msg = GlobalFunction.GetStatus_MsgByKey("PICDep_SelectMorethanOne_msg");
            UserAccessRightMsg.SelectAllDep_SelectAll_msgNotify = GlobalFunction.GetStatus_MsgByKey("SelectAllDep_SelectAll_msgNotify");
            UserAccessRightMsg.AllDep_Select_msgNotify = GlobalFunction.GetStatus_MsgByKey("AllDep_Select_msgNotify");
            UserAccessRightMsg.Common_Empty_Noti = GlobalFunction.GetStatus_MsgByKey("Common_Empty_Noti");
            UserAccessRightMsg.NoDeparment_Noti = GlobalFunction.GetStatus_MsgByKey("NoDeparment_Noti");
           
            return UserAccessRightMsg;
        }

        public DepartmentMapingMsg DepartmentMsgSelect()
        {
            DepartmentMapingMsg Msg = new DepartmentMapingMsg();
            Msg.UserID_Empty = GlobalFunction.GetStatus_MsgByKey("UserID_Empty");
            Msg.Department_Empty = GlobalFunction.GetStatus_MsgByKey("Department_Empty");
            Msg.textbox_Noti = GlobalFunction.GetStatus_MsgByKey("textbox_Noti");
            Msg.Data_Add_Success = GlobalFunction.GetStatus_MsgByKey("Data_Add_Success");
            Msg.Data_Update_Success = GlobalFunction.GetStatus_MsgByKey("Data_Update_Success");
            Msg.Data_Delete_Success = GlobalFunction.GetStatus_MsgByKey("Data_Delete_Success");
            Msg.danger_Noti = GlobalFunction.GetStatus_MsgByKey("danger_Noti");
            Msg.Warning_Noti = GlobalFunction.GetStatus_MsgByKey("Warning_Noti");
            Msg.info_Noti = GlobalFunction.GetStatus_MsgByKey("info_Noti");
            Msg.UserID_Inused = GlobalFunction.GetStatus_MsgByKey("UserID_Inused");
            Msg.success_Noti = GlobalFunction.GetStatus_MsgByKey("success_Noti");
            Msg.success_title = GlobalFunction.GetStatus_MsgByKey("success_title");
            return Msg;
        }
    }
}