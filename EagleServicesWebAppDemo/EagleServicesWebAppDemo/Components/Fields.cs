using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EagleServicesWebApp.Components
{
    public class Fields
    {
        #region MessagesInItemEnquiry
        public const string emptyDescription = "Item Description cannot be empty";
        public const string emptyProductGroup = "Product Group cannot be empty";
        public const string emptySKU = "SKU cannot be empty";
        public const string emptyEPC = "EPC cannot be empty";
        public const string alreadyExistEPC = "ID already exists";
        public const string wrongLengthEPC = "EPC length must be 24";

        public const string alreadyExistItemDescription = "Item Description already exists";

        public const string itemSaveSuccess= "Successfully saved";
        public const string itemSaveFail = "Save Item Failed";
        public const string itemUpdateSuccess = "Successfully saved";
        public const string itemUpdateFail = "Updaing Item Failed";
        public const string itemDeleteSuccess = "Successfully deleted";

        public const string itemDeleteFail = "Deleting Item Failed";

        public const string itemandMappingDeleteFail = "Deleting Item and ItemPartMapping is failed";
        #endregion

        #region MessagesInUsers
        public const string emptyFirstName = "First Name field is required";
        public const string emptyUserRole = "Role Name field is required";
        //public const string emptyCardID = "Card ID field is required";
        public const string emptyStatus = "Status field is required";

        public const string userSaveSuccess = "Successfully saved";
        public const string userUpdateSuccess = "Successfully saved";
        public const string userDeleteSuccess = "Successfully deleted";
        public const string userPasswordResetSuccess = "Successfully saved";
        public const string userPasswordChangeSuccess = "Successfully saved";

        public const string alreadyUserFirstName = "First Name already exists";
        public const string alreadyUserCardID = "Card ID already exists";

        public const string userInUse="User is currently in use";
        #endregion

        #region commomMessages
        public const string no_errorcode = "0";
        public const string error_code = "100";

        public const string errorcode_text = "errorcode";
        public const string title_text = "title";
        public const string Warning_Noti = "warning";
        public const string msg_text = "msg";

        public const string primary_key_constraint_error = "Violation of PRIMARY KEY constraint";

        public const string success_title = "Success";
      #endregion

      //DataImport Column Check 
      public const string Item_Description = "Item Description";
      public const string Product_Group = "Product Group";
      public const string Machine = "Machine";
      public const string Sku = "SKU";
      public const string Epc = "EPC";
      public const string Qty = "Quantity";

      //message 
     public const string Req_Column = " column name is missing in excel file";
      public const string DataTypeInvalid = " data format is invalid";
      public const string Epcdigit = " length must be 24 digits";
      public const string ExcelData_Empty = " should not be empty. Please check excel content!";
      public const string EPC_Duplicated = " are duplicated";
      public const string DuplicationPass = "Pass";
      public const string DuplicationFail = "Fail! Please check excel content!";
      public const string NoData_InExcel = "No Data In Excel";
      public const string MissingColumn = " Columns are missing in excel file.Please check Download Sample Format file!";
      public const string Excel_Data_Insert_Success = "Data successfully imported to DB";
      public const string Excel_Data_Insert_Fail = "Data import failed";
      public const string Qty_Error = "Quantity must be greater than 0";
   }
}