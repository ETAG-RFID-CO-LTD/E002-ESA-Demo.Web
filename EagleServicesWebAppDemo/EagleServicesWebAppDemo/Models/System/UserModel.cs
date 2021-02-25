using EagleServicesWebApp.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;

namespace EagleServicesWebApp.Models.RollRoyceSystem
{
   public class User_REC
   {
      [Key]
      [Required]
      [Display(Name = "User ID")]
      public string UserID { get; set; }
      public string Password { get; set; }
      [NotMapped]
      public string ConfirmPassowrd { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      //public long RoleID { get; set; }
      public bool isActive { get; set; }
      public DateTime CreatedDate { get; set; }
      public DateTime UpdatedDate { get; set; }
      public string CreatedBy { get; set; }
      public string UpdatedBy { get; set; }
      public string LogQuery { get; set; }     
      public bool isAdmin { get; set; }
      public string Department { get; set; }
      [Key]
      [Required]
      [Display(Name = "Role Name")]
      public string RoleName { get; set; }

      [Required]
      [Display(Name = "Status")]
      public string Status { get; set; }
      public string DepartmentName { get; set; } 
   }

   public class Role_REC
   {
      public string RoleName { get; set; }
      public string Description { get; set; }
      public DateTime CreatedDate { get; set; }
      public DateTime UpdatedDate { get; set; }
      public string CreatedBy { get; set; }
      public string UpdatedBy { get; set; }
      public string LogQuery { get; set; }
   }

   public class RolePermission_REC
   {
      public string CreatedBy { get; set; }
      public DateTime CreatedDate { get; set; }
      public string UpdatedBy { get; set; }
      public DateTime UpdatedDate { get; set; }
      public string RoleName { get; set; }
      public string MenuID { get; set; }
      public bool IsAllowAdd { get; set; }
      public bool IsAllowEdit { get; set; }
      public bool IsAllowDelete { get; set; }
      public string IsListing { get; set; }
      public string IsAdd { get; set; }
      public string IsUpdate { get; set; }
      public string IsDelete { get; set; }
      public string MobileID { get; set; }
   }

   public class FindDataCount
   {
      public int Rcount { get; set; }
   }

   public class UserModel
   {
      public int ErrorNo { get; set; }
      public string ErrorMessage { get; set; }

      public UserModel()
      {
         ErrorNo = 0;
         ErrorMessage = "";
      }

        #region For Login User Information Select,Insert,Update and Delete

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<User_REC> GetNotMappingUserList()
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "" +
                        "SELECT distinct " +
                         "    UserID " +
                         "    FROM LogIn_vw with (nolock) WHERE isAdmin <>1 AND UserID not in " + "" +
                         " (Select distinct UserID from tblDepartmentMapping with (nolock)) " +
                         "   ORDER BY UserID " +
                        ";"
        ;

            var vQuery = oRemoteDB.Database.SqlQuery<User_REC>(sSQL);

            return vQuery;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<User_REC> GetList()
      {
         DatabaseContext oRemoteDB = new DatabaseContext();

         string sSQL = "" +
                     "SELECT " +
                      "    UserID,Password,RoleName, FirstName,LastName, " +//Department,
                      "    isAdmin,isActive,Status " +
                      "    FROM LogIn_vw with (nolock) " + "" +
                      " GROUP BY UserID,Password,RoleName, FirstName,LastName,isAdmin,isActive,Status " +
                      "   ORDER BY UserID " +
                     ";"
     ;

         var vQuery = oRemoteDB.Database.SqlQuery<User_REC>(sSQL);

         return vQuery;
      }

      public System.Data.Entity.Infrastructure.DbRawSqlQuery<User_REC> GetListingExceptSystemAdmin()
      {
         DatabaseContext oRemoteDB = new DatabaseContext();


         string sSQL = "" +
                     "SELECT " +
                      "    UserID,Password,isActive,RoleName, " +
                      "    FirstName,LastName,Status,isActive,isAdmin " + //,Department,DepartmentName
                      "   FROM LogIn_vw with (nolock)  WHERE isAdmin=0 " +
                        " GROUP BY UserID,Password,RoleName, FirstName,LastName,isAdmin,isActive,Status " +
                      //"   AND  isActive=1 " +
                      "   ORDER BY UserID " +
                     ";"
     ;

         var vQuery = oRemoteDB.Database.SqlQuery<User_REC>(sSQL);

         return vQuery;
      }

      public bool Insert(User_REC poRecord)
      {
         bool bReturn = true;
         //------------------------------
         try
         {
            poRecord.CreatedDate = DateTime.Now;
            poRecord.RoleName = poRecord.RoleName;
            poRecord.UserID = poRecord.UserID.Trim().ToString();

            #region Log Query

            if ((poRecord.FirstName == "-") && (poRecord.LastName == "-"))
               poRecord.LogQuery = "Insert into tblUser Table==>UserID=" + poRecord.UserID + ",Password=" + poRecord.ConfirmPassowrd + ",RoleName=" + poRecord.RoleName + ",isActive=" + poRecord.isActive + ",isAdmin=" + poRecord.isAdmin;
            else if (poRecord.FirstName == "-")
               poRecord.LogQuery = "Insert into tblUser Table==>UserID=" + poRecord.UserID + ",Password=" + poRecord.ConfirmPassowrd + ",Last Name =" + poRecord.LastName + ",RoleName=" + poRecord.RoleName + ",isActive=" + poRecord.isActive + ",isAdmin=" + poRecord.isAdmin;
            else if (poRecord.LastName == "-")
               poRecord.LogQuery = "Insert into tblUser Table==>UserID=" + poRecord.UserID + ",Password=" + poRecord.ConfirmPassowrd + ",First Name =" + poRecord.FirstName + ",RoleName=" + poRecord.RoleName + ",isActive=" + poRecord.isActive + ",isAdmin=" + poRecord.isAdmin;
            else
               poRecord.LogQuery = "Insert into tblUser Table==>UserID=" + poRecord.UserID + ",Password=" + poRecord.ConfirmPassowrd + ",First Name =" + poRecord.FirstName + ",Last Name=" + poRecord.LastName + ",RoleName=" + poRecord.RoleName + ",isActive=" + poRecord.isActive + ",isAdmin=" + poRecord.isAdmin;

            poRecord.Password = GlobalFunction.CreateMD5Hash(poRecord.Password.ToLower());

            #endregion

            string sSQL = "" +
                "INSERT INTO tblUser " +
                "      ( " +
                "      UserID,Password, " +
                "      FirstName,LastName,RoleName,isAdmin,isActive,CreatedDate,CreatedBy,LogQuery " +
                "      ) " +
                "      VALUES " +
                "      (" +
                "      @UserID,@Password, " +
                "      @FirstName,@LastName,@RoleName,@isAdmin,@isActive,@CreatedDate,@CreatedBy,@LogQuery " +
                "      )" +
                ";" 
                ;

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@UserID", poRecord.UserID));
            oParameters.Add(new SqlParameter("@Password", poRecord.Password));
            oParameters.Add(new SqlParameter("@FirstName", poRecord.FirstName));
            oParameters.Add(new SqlParameter("@LastName", poRecord.LastName));
            oParameters.Add(new SqlParameter("@RoleName", poRecord.RoleName));
            oParameters.Add(new SqlParameter("@isAdmin", poRecord.isAdmin));
            oParameters.Add(new SqlParameter("@isActive", poRecord.isActive));
            oParameters.Add(new SqlParameter("@CreatedDate", poRecord.CreatedDate));
            oParameters.Add(new SqlParameter("@CreatedBy", poRecord.CreatedBy));
            oParameters.Add(new SqlParameter("@LogQuery", poRecord.LogQuery));
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

      public bool UpdatePwd(User_REC poRecord)
      {
         bool bReturn = true;
         //------------------------------
         try
         {
            poRecord.UserID = poRecord.UserID;
            poRecord.UpdatedBy = poRecord.UpdatedBy;
            poRecord.UpdatedDate = DateTime.Now;

            poRecord.Password = GlobalFunction.CreateMD5Hash(poRecord.Password.ToLower());

            string sSQL = "" +
                "UPDATE tblUser " +
                "   SET " +
                "      password = @password, " +
                "      UpdatedDate = @UpdatedDate, " +
                "      UpdatedBy = @UpdatedBy, " +
                "      LogQuery = @LogQuery " +
                "   WHERE (UserID = @UserID) " +

                ";"
                ;

            List<SqlParameter> oParameters = new List<SqlParameter>();

            oParameters.Add(new SqlParameter("@UserID", poRecord.UserID));
            oParameters.Add(new SqlParameter("@password", poRecord.Password));
            oParameters.Add(new SqlParameter("@UpdatedDate", poRecord.UpdatedDate));
            oParameters.Add(new SqlParameter("@UpdatedBy", poRecord.UpdatedBy));
            oParameters.Add(new SqlParameter("@LogQuery", poRecord.LogQuery));
            SqlParameter[] vSqlParameter = oParameters.ToArray();

            DatabaseContext oRemoteDB = new DatabaseContext();
            var oTransaction = oRemoteDB.Database.BeginTransaction();
            int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
            if (nReturn == 1)
            {
               oTransaction.Commit();
            }
            else
            {
               oTransaction.Rollback();
               ErrorMessage = "Failed to update record!" + "<br>" + "Record has been updated or deleted by another user.";
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

      public bool UpdateInfo(User_REC poRecord)
      {
         bool bReturn = true;
         //------------------------------
         try
         {
            poRecord.UpdatedDate = DateTime.Now;
            poRecord.RoleName = poRecord.RoleName;
            poRecord.UserID = poRecord.UserID.Trim().ToString();

            #region Log Query

            if ((poRecord.FirstName == "-") && (poRecord.LastName == "-"))
               poRecord.LogQuery = "Insert into tblUser Table==>UserID=" + poRecord.UserID + ",Password=" + poRecord.ConfirmPassowrd + ",RoleName=" + poRecord.RoleName + ",isActive=" + poRecord.isActive + ",isAdmin=" + poRecord.isAdmin;
            else if (poRecord.FirstName == "-")
               poRecord.LogQuery = "Insert into tblUser Table==>UserID=" + poRecord.UserID + ",Password=" + poRecord.ConfirmPassowrd + ",Last Name =" + poRecord.LastName + ",RoleName=" + poRecord.RoleName + ",isActive=" + poRecord.isActive + ",isAdmin=" + poRecord.isAdmin;
            else if (poRecord.LastName == "-")
               poRecord.LogQuery = "Insert into tblUser Table==>UserID=" + poRecord.UserID + ",Password=" + poRecord.ConfirmPassowrd + ",First Name =" + poRecord.FirstName + ",RoleName=" + poRecord.RoleName + ",isActive=" + poRecord.isActive + ",isAdmin=" + poRecord.isAdmin;
            else
               poRecord.LogQuery = "Insert into tblUser Table==>UserID=" + poRecord.UserID + ",Password=" + poRecord.ConfirmPassowrd + ",First Name =" + poRecord.FirstName + ",Last Name=" + poRecord.LastName + ",RoleName=" + poRecord.RoleName + ",isActive=" + poRecord.isActive + ",isAdmin=" + poRecord.isAdmin;

            poRecord.Password = GlobalFunction.CreateMD5Hash(poRecord.UserID.ToLower() + poRecord.Password.ToLower());

            #endregion

            string sSQL = "" +
                "UPDATE tblUser " +
                "   SET " +
                "      UserID = @UserID, " +
                "      FirstName = @FirstName, " +
                "      LastName = @LastName, " +
                "      RoleName = @RoleName, " +
                "      isAdmin = @isAdmin, " +
                "      isActive = @isActive, " +
                "      UpdatedDate = @UpdatedDate, " +
                "      UpdatedBy = @UpdatedBy, " +
                "      LogQuery = @LogQuery " +
                "   WHERE (UserID = @UserID) " +
                ";"
                ;

            List<SqlParameter> oParameters = new List<SqlParameter>();

            oParameters.Add(new SqlParameter("@UserID", poRecord.UserID));
            oParameters.Add(new SqlParameter("@FirstName", poRecord.FirstName));
            oParameters.Add(new SqlParameter("@LastName", poRecord.LastName));
            oParameters.Add(new SqlParameter("@RoleName", poRecord.RoleName));
            oParameters.Add(new SqlParameter("@isAdmin", poRecord.isAdmin));
            oParameters.Add(new SqlParameter("@isActive", poRecord.isActive));
            oParameters.Add(new SqlParameter("@UpdatedDate", poRecord.UpdatedDate));
            oParameters.Add(new SqlParameter("@UpdatedBy", poRecord.UpdatedBy));
            oParameters.Add(new SqlParameter("@LogQuery", poRecord.LogQuery));
            SqlParameter[] vSqlParameter = oParameters.ToArray();

            DatabaseContext oRemoteDB = new DatabaseContext();
            var oTransaction = oRemoteDB.Database.BeginTransaction();
            int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
            if (nReturn == 1)
            {
               oTransaction.Commit();
            }
            else
            {
               oTransaction.Rollback();
               ErrorMessage = "Failed to update record!" + "<br>" + "Record has been updated or deleted by another user.";
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

      public bool Delete(User_REC poRecord, string _CurrentLoginID)
      {
         bool bReturn = true;
         //------------------------------
         try
         {
            //----------
            string sSQL = "" +
                "DELETE tblUser " +
                "   WHERE (UserID = @UserID) ; " +
                ";"
                ;

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@UserID", poRecord.UserID));
            SqlParameter[] vSqlParameter = oParameters.ToArray();

            DatabaseContext oRemoteDB = new DatabaseContext();
            var oTransaction = oRemoteDB.Database.BeginTransaction();
            if (poRecord.UserID != _CurrentLoginID)
            {
               int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
               if (nReturn == 1)
               {
                  oTransaction.Commit();
               }
               else
               {
                  oTransaction.Rollback();
                  ErrorMessage = "Failed to delete record!"  ;

                  bReturn = false;
               }
            }
            else
            {
               //If Delete Login User
               oTransaction.Rollback();
               ErrorMessage = "Failed to delete record!" + "<br>" + "UserID is currently in use!.";
               bReturn = false;

            }
         }
         catch (Exception ex)
         {
            //ErrorMessage = ex.Message;
            ErrorMessage = "UserID is currently in use!.";
            bReturn = false;
         }
         //------------------------------
         return bReturn;
      }

      #endregion

      #region Filter Role Name Search
      public System.Data.Entity.Infrastructure.DbRawSqlQuery<Role_REC> GetListForRoleSearch()
      {
         DatabaseContext oRemoteDB = new DatabaseContext();

         string sSQL = "" +
             "SELECT " +
             "       distinct RoleName,Description " +
             "   FROM tblRole  with (nolock) WHERE IsVisible=0 " +
             "   ORDER BY RoleName  " +
             ";"
             ;

         var vQuery = oRemoteDB.Database.SqlQuery<Role_REC>(sSQL);

         return vQuery;
      }

      public System.Data.Entity.Infrastructure.DbRawSqlQuery<Role_REC> GetRoleListing()
      {
         DatabaseContext oRemoteDB = new DatabaseContext();

         string sSQL = "" +
               "SELECT " +
               "       distinct RoleName,Description " +
               "   FROM tblRole  with (nolock) " +
               "   ORDER BY RoleName  " +
               ";"
               ;

         var vQuery = oRemoteDB.Database.SqlQuery<Role_REC>(sSQL);

         return vQuery;
      }

      public System.Data.Entity.Infrastructure.DbRawSqlQuery<Role_REC> GetRoleListingExceptIsAdmin()
        {
            DatabaseContext oRemoteDB = new DatabaseContext();

            string sSQL = "" +
                  "SELECT " +
                  "       distinct RoleName,Description " +
                  "   FROM tblRole  with (nolock)  WHERE IsVisible=0 " +
                  "   ORDER BY RoleName  " +
                  ";"
                  ;

            var vQuery = oRemoteDB.Database.SqlQuery<Role_REC>(sSQL);

            return vQuery;
        }
        public System.Data.Entity.Infrastructure.DbRawSqlQuery<FindDataCount> CheckUserRoleName(string _RoleName)
      {
         DatabaseContext oRemoteDB = new DatabaseContext();
         string sSQL = "";

         sSQL = "select count(*) As  Rcount from tblRole with (nolock) where RoleName='" + _RoleName + "'";

         var vQuery = oRemoteDB.Database.SqlQuery<FindDataCount>(sSQL);
         return vQuery;
      }

      public bool Insert(Role_REC poRecord)
      {
         bool bReturn = true;
         //------------------------------
         try
         {
            poRecord.LogQuery = "Insert Role as following => RoleName " + poRecord.RoleName;

            //----------
            string sSQL = "" +
                "INSERT INTO tblRole WITH (UPDLOCK, HOLDLOCK)  " +
                "      ( " +
                "      RoleName, Description, " +
                "      CreatedDate, CreatedBy,LogQuery " +
                "      ) " +
                "      VALUES " +
                "      (" +
                "      @RoleName, @Description, " +
                "      @CreatedDate,@CreatedBy,@LogQuery" +
                "      )" +
                ";" 
                ;

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@RoleName", poRecord.RoleName));
            oParameters.Add(new SqlParameter("@Description", poRecord.Description));
            oParameters.Add(new SqlParameter("@CreatedBy", poRecord.CreatedBy));
            oParameters.Add(new SqlParameter("@CreatedDate", poRecord.CreatedDate));
            oParameters.Add(new SqlParameter("@LogQuery", poRecord.LogQuery));

            SqlParameter[] vSqlParameter = oParameters.ToArray();

            DatabaseContext oRemoteDB = new DatabaseContext();
            int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

            if (nReturn >= 1)
            {
               bReturn = true;
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

      public bool AllInfoInsertRolePermission(RolePermission_REC poRecord)
      {
         bool bReturn = true;
         //------------------------------
         try
         {
            poRecord.CreatedDate = DateTime.Now;
            //----------
            string sSQL = "";
               sSQL = "" +
             "INSERT INTO tblRolePermission  " +
              "      ( " +
              "      RoleName, MenuID, IsAllowAdd, IsAllowEdit, IsAllowDelete, CreatedDate, CreatedBy " +
              "      ) " +
              "      select " +
              "      @RoleName as RoleName,MenuID,1 as IsAllowAdd,1 as IsAllowEdit,1 as IsAllowDelete,@CreatedDate as CreatedDate,@CreatedBy as CreatedBy FROM tblMenu " +
              ";"
              ;          

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@RoleName", poRecord.RoleName));
            oParameters.Add(new SqlParameter("@CreatedDate", poRecord.CreatedDate));
            oParameters.Add(new SqlParameter("@CreatedBy", poRecord.CreatedBy));

            SqlParameter[] vSqlParameter = oParameters.ToArray();

            DatabaseContext oRemoteDB = new DatabaseContext();
            int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

            if (nReturn > 0)
            {
               bReturn = true;
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
        public bool AllInfoInsertRolePermissionExceptMobile(RolePermission_REC poRecord)
        {
            bool bReturn = true;
            //------------------------------
            try
            {
                poRecord.CreatedDate = DateTime.Now;
                //----------
                string sSQL = "";
                sSQL = "" +
              "INSERT INTO tblRolePermission  " +
               "      ( " +
               "      RoleName, MenuID, IsAllowAdd, IsAllowEdit, IsAllowDelete, CreatedDate, CreatedBy " +
               "      ) " +
               "      select " +
               "      @RoleName as RoleName,MenuID,1 as IsAllowAdd,1 as IsAllowEdit,1 as IsAllowDelete,@CreatedDate as CreatedDate,@CreatedBy as CreatedBy FROM tblMenu  WHERE MenuID!='M4'" +
               ";"
               ;

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@RoleName", poRecord.RoleName));
                oParameters.Add(new SqlParameter("@CreatedDate", poRecord.CreatedDate));
                oParameters.Add(new SqlParameter("@CreatedBy", poRecord.CreatedBy));

                SqlParameter[] vSqlParameter = oParameters.ToArray();

                DatabaseContext oRemoteDB = new DatabaseContext();
                int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

                if (nReturn > 0)
                {
                    bReturn = true;
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
        public bool InsertRolePermission(List<RolePermission_REC> poRecord)
      {
         bool bReturn = true;
         DatabaseContext oRemoteDB = new DatabaseContext();
         var oTransaction = oRemoteDB.Database.BeginTransaction();
         //------------------------------
         try
         {
            //----------
            string sSQL = "";
            foreach(RolePermission_REC rolePermission_REC in poRecord)
            {
               sSQL = "" +
                "INSERT INTO tblRolePermission  " +
                "      ( " +
                "     RoleName, MenuID, IsAllowAdd, IsAllowEdit, IsAllowDelete, CreatedDate, CreatedBy " +
                "      ) " +
                "      VALUES " +
                "      (" +
                "    @RoleName, @MenuID, @IsAllowAdd, @IsAllowEdit, @IsAllowDelete, @CreatedDate, @CreatedBy " +
                "      )" +
                ";"
                ;

               List<SqlParameter> oParameters = new List<SqlParameter>();
               oParameters.Add(new SqlParameter("@RoleName", rolePermission_REC.RoleName));
               oParameters.Add(new SqlParameter("@MenuID", rolePermission_REC.MenuID));
               oParameters.Add(new SqlParameter("@CreatedDate", DateTime.Now));
               oParameters.Add(new SqlParameter("@CreatedBy", System.Web.HttpContext.Current.Session["UserID"].ToString()));
               oParameters.Add(new SqlParameter("@IsAllowAdd", rolePermission_REC.IsAllowAdd));
               oParameters.Add(new SqlParameter("@IsAllowEdit", rolePermission_REC.IsAllowEdit));
               oParameters.Add(new SqlParameter("@IsAllowDelete", rolePermission_REC.IsAllowDelete));

               SqlParameter[] vSqlParameter = oParameters.ToArray();
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

            if (bReturn)
            {
               oTransaction.Commit();
            }
            else
            {
               oTransaction.Rollback();
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

      public bool DeleteOnlyUserRole(Role_REC poRecord)
      {
         bool bReturn = true;
         //------------------------------
         try
         {
            //----------
            string sSQL = "" +
                  "DELETE tblRole " +
                "   WHERE (RoleName = @RoleName) " +
                ";"
                ;

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@RoleName", poRecord.RoleName));
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

      public System.Data.Entity.Infrastructure.DbRawSqlQuery<RolePermission_REC> GetRolePermissionData(string _RoleName)
      {
         DatabaseContext oRemoteDB = new DatabaseContext();
         string sSQL = "";

         sSQL = "select permission.RoleName,menu.MenuID,menu.MenuName, " +
       " case " +
       " when menu.MenuID <> 'M1' and menu.MenuID <> 'M2' and menu.MenuID <> 'M3' and menu.MenuID <> 'M4' then " +
            " case " +
                  " when permission.IsAllowAdd = 1 or permission.IsAllowEdit = 1 or permission.IsAllowDelete = 1 or permission.IsAllowAdd = 0 or permission.IsAllowEdit = 0 or permission.IsAllowDelete = 0 then menu.MenuName + 'Listing' " +
                  " else '' " +
            " end " +
        "else '' " +
      " end as IsListing, " +
       " case " +
      " when menu.MenuID <> 'M1' and menu.MenuID <> 'M2' and menu.MenuID <> 'M3' then " +
        " case permission.IsAllowAdd " +
           " when 1 then menu.MenuName + 'Insert' " +
           " else '' " +
       " end " +
    " else '' " +
" end as IsAdd, " +
" case " +
   " when menu.MenuID <> 'M1' and menu.MenuID <> 'M2' and menu.MenuID <> 'M3' then " +
       " case permission.IsAllowEdit " +
           " when 1 then menu.MenuName + 'Update' " +
           " else '' " +
       " end " +
    " else '' " +
" end as IsUpdate, " +
" case " +
  "  when menu.MenuID <> 'M1' and menu.MenuID <> 'M2' and menu.MenuID <> 'M3' then " +
       " case permission.IsAllowDelete " +
           " when 1 then menu.MenuName + 'Delete' " +
            " else '' " +
       " end " +
   " else '' " +
" end as IsDelete " +
" from tblRolePermission permission with(nolock) " +
" inner join tblMenu menu with(nolock) on menu.MenuID = permission.MenuID where permission.RoleName = '" + _RoleName + "'" + ";"
;

         var vQuery = oRemoteDB.Database.SqlQuery<RolePermission_REC>(sSQL);
         return vQuery;
      }

        public bool DeleteOnlyRolePermission(Role_REC poRecord)
        {
            bool bReturn = true;
            DatabaseContext oRemoteDB = new DatabaseContext();
            var oTransaction = oRemoteDB.Database.BeginTransaction();
            //------------------------------
            try
            {
                //----------
                string sSQL = "" +
                "DELETE tblRolePermission " +
              "   WHERE (RoleName = @RoleName) " +
              ";"
              ;

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@RoleName", poRecord.RoleName));
                SqlParameter[] vSqlParameter = oParameters.ToArray();


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
                oTransaction.Rollback();
                ErrorMessage = ex.Message;
                bReturn = false;
            }
            //------------------------------
            return bReturn;
        }

      public System.Data.Entity.Infrastructure.DbRawSqlQuery<FindDataCount> CheckRoleNameCurrentInUse(string _RoleName)
      {
         DatabaseContext oRemoteDB = new DatabaseContext();
         string sSQL = "";

         sSQL = "select count(*) As  Rcount from tblUser with (nolock) where RoleName='" + _RoleName + "'";

         var vQuery = oRemoteDB.Database.SqlQuery<FindDataCount>(sSQL);
         return vQuery;
      }
      #endregion
   }

}