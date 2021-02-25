using EagleServicesWebApp.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using EagleServicesWebApp.Models.GeneralTableModel;

namespace EagleServicesWebApp.Models.RollRoyceSystem
{
   public class UserRole
   {
      public int roleid { get; set; }
      public string rolename { get; set; }
      public string roledesc { get; set; }
      public string registeredby { get; set; }
      public DateTime? registereddate { get; set; }
      public string updatedby { get; set; }
      public DateTime? updateddate { get; set; }
      public string menuaccess { get; set; }
      public string dataaccess { get; set; }
      public bool IsUsing { get; set; }
   }

   public class UserRolePermission
   {
      public string registeredby { get; set; }
      public DateTime? registereddate { get; set; }
      public string updatedby { get; set; }
      public DateTime? updateddate { get; set; }
      public int roleid { get; set; }
      public int functionid { get; set; }
      public bool canInsert { get; set; }
      public bool canUpdate { get; set; }
      public bool canDelete { get; set; }
      public bool canViewDetail { get; set; }
      public bool canListing { get; set; }
   }

   public class SystemUserRolePermission
   {
      public string Role_Name { get; set; }
      public string Default_Access { get; set; }
      public string Access { get; set; }
      public int Role_ID { get; set; }
   }

   public class UserRoleFunction
   {
      public int functionid { get; set; }
      public string functionname { get; set; }
      public string functioncode { get; set; }
      public string description { get; set; }
   }

   public class UserRole_REC
    {
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isActive { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long RoleID { get; set; }
        public string LogQuery { get; set; }
        //------------------------------

        public string RoleName { get; set; }
        public string DefaultAccess { get; set; }
        public string MenuAccess { get; set; }
        public string Enquiry { get; set; }
        public string UserRight { get; set; }
        public string GeneralTable { get; set; }
        public string CommonAccess { get; set; }
        public string DataAccess { get; set; }

        public bool Finance { get; set; }
        public bool PIC { get; set; }
        public string DepartmentRight { get; set; }

        public bool IsUsing { get; set; }
    }

    public class UserRolePermission_REC
    {
        public long CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isActive { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long RoleID { get; set; }
        //------------------------------
        public string MenuID { get; set; }
        public bool IsAllowAdd { get; set; }
        public bool IsAllowEdit { get; set; }
        public bool IsAllowDelete { get; set; }
        
    }

    public class UserRoleModel
    {
        public int ErrorNo { get; set; }
        public string ErrorMessage { get; set; }

        #region Variable Declaration
        bool bReturn = true;
        bool _MenuAccessIDs = false;
        bool _EnquiryIDs = false;  // M3
        bool _UserRightIDs = false;  //M5
        bool _GeneralTableIDs = false; //M4, 
        bool _CommonAccessIDs = false;
        bool _DataAccessIDs = false; //
        UserRolePermission_REC _permissionInfo;

       string _Menufilter = string.Empty;
        string _DefaultAccess = string.Empty;

        #endregion

        public UserRoleModel()
        {
            ErrorNo = 0;
            ErrorMessage = "";
        }

        #region CRUD 
        #region Create

        public string ExtractString(string Parameters)
        {
            _DefaultAccess = string.Empty;
            string[] values = Parameters.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();

                if (values[i] != "0")
                {
                    if (_Menufilter != string.Empty)
                    {
                        _Menufilter = _Menufilter + ",";
                        _DefaultAccess = _DefaultAccess + ",";
                    }

                    _Menufilter = _Menufilter + "'" + values[i] + "'";
                    _DefaultAccess = _DefaultAccess + values[i];
                }
            }

            return _DefaultAccess;
        }

        public bool CreateUserRole(UserRole_REC poRecord)
        {
            _Menufilter = string.Empty;
            poRecord.DefaultAccess = string.Empty;

            #region Parameters
            _permissionInfo = new UserRolePermission_REC();
            _permissionInfo.IsAllowAdd = false;
            _permissionInfo.IsAllowEdit = false;
            _permissionInfo.IsAllowDelete = false;

            if (poRecord.MenuAccess.ToString().Contains("All"))
                _MenuAccessIDs = true;
            else
                _MenuAccessIDs = false;

            if ((poRecord.Enquiry.ToString().Contains("All")) || (poRecord.Enquiry.ToString().Contains("M3,")))
                _EnquiryIDs = true;
            else
                _EnquiryIDs = false;

            if ((poRecord.UserRight.ToString().Contains("All")) || (poRecord.UserRight.ToString().Contains("M5,")))
                _UserRightIDs = true;
            else
                _UserRightIDs = false; 

            if ((poRecord.GeneralTable.ToString().Contains("All"))|| (poRecord.GeneralTable.ToString().Contains("M4,")))
                _GeneralTableIDs = true;
            else
                _GeneralTableIDs = false;


            if (poRecord.CommonAccess.ToString().Contains("All"))
                _CommonAccessIDs = true;
            else
                _CommonAccessIDs = false;

            if (poRecord.DataAccess.ToString().Contains("All"))
                _DataAccessIDs = true;
            else
                _DataAccessIDs = false;

            if(poRecord.DepartmentRight.Trim() == "Finance".Trim())
            {
                poRecord.Finance = true;
                poRecord.PIC = false;
            }
            else
            {
                poRecord.PIC = true;
                poRecord.Finance = false;
            }

            #endregion

            #region AccessMenuAll ( AssetTagging,DataImport)
            if (_MenuAccessIDs == true)
            {
                string _strActual = "AssetTagging,DataImport";
                poRecord.MenuAccess = "All";
                _Menufilter = "'AssetTagging','DataImport'";
                poRecord.DefaultAccess = _strActual;
            }
            else
            {
                #region Access Menu parameters
                if (poRecord.MenuAccess != "0")
                {
                    _DefaultAccess = ExtractString(poRecord.MenuAccess.Trim().ToString());
                    poRecord.DefaultAccess = poRecord.DefaultAccess + _DefaultAccess;
                }
                #endregion
            }
            #endregion

            #region Enquiry ( M3,M3P1,M3P2,M3P3)
            if (_EnquiryIDs == true)
            {
                poRecord.Enquiry = "All";
                string _strActual = "AssetTagging,DataImport";

                if (_Menufilter != string.Empty)
                {
                    _Menufilter = _Menufilter + ",";
                    poRecord.DefaultAccess = poRecord.DefaultAccess + ",";
                }

                _Menufilter = _Menufilter + "'M3','M3P1','M3P2','M3P3'";
                poRecord.DefaultAccess = poRecord.DefaultAccess + _strActual;
            }
            else
            {
                #region Enquiry parameters
                if (poRecord.Enquiry != "0")
                {
                    _DefaultAccess = ExtractString(poRecord.Enquiry.Trim().ToString());
                    poRecord.DefaultAccess = poRecord.DefaultAccess + _DefaultAccess;
                }
                #endregion
            }
            #endregion

            #region User (M5,M5P1,M5P2)
            if (_UserRightIDs == true)
            {
                if (_Menufilter != string.Empty)
                {
                    _Menufilter = _Menufilter + ",";
                    poRecord.DefaultAccess = poRecord.DefaultAccess + ",";
                }
                string _strActual = "M5,M5P1,M5P2";
                poRecord.UserRight = "All";
                _Menufilter = _Menufilter + "'M5','M5P1','M5P2'";
                poRecord.DefaultAccess = poRecord.DefaultAccess + _strActual;

            }
            else
            {
                #region User parameters
                if (poRecord.UserRight != "0")
                {
                    _DefaultAccess = ExtractString(poRecord.UserRight.Trim().ToString());
                    poRecord.DefaultAccess = poRecord.DefaultAccess + _DefaultAccess;
                }
                #endregion
            }
            #endregion

            #region General Table (M4,M4P1,M4P2,M4P3,M4P4,M4P5,M4P6,M4P7,M4P8,M4P9,M4P10,M4P11,M4P12,M4P13)
            if (_GeneralTableIDs == true)
            {
                if (_Menufilter != string.Empty)
                {
                    _Menufilter = _Menufilter + ",";
                    poRecord.DefaultAccess = poRecord.DefaultAccess + ",";
                }
                string _strActual = "M4,M4P1,M4P2,M4P3,M4P4,M4P5,M4P6,M4P7,M4P8,M4P9,M4P10,M4P11,M4P12,M4P13";
                poRecord.GeneralTable = "All";
                _Menufilter = _Menufilter + "'M4','M4P1','M4P2','M4P3','M4P4','M4P5','M4P6','M4P7','M4P8','M4P9','M4P10','M4P11','M4P12','M4P13'";
                poRecord.DefaultAccess = poRecord.DefaultAccess + _strActual;
            }
            else
            {
                #region General Table parameters
                if (poRecord.GeneralTable != "0")
                {
                    _DefaultAccess = ExtractString(poRecord.GeneralTable.Trim().ToString());
                    poRecord.DefaultAccess = poRecord.DefaultAccess + _DefaultAccess;
                }

                #endregion
            }
            #endregion

            #region CommonAccessAll (Add,Edit,Delete)
            if (_CommonAccessIDs == true)
            {

                if (_Menufilter != string.Empty)
                {
                    poRecord.DefaultAccess = poRecord.DefaultAccess + ",";
                }
                string _strActual = "Add,Edit,Delete";
                poRecord.CommonAccess = "All";
                _permissionInfo.IsAllowAdd = true;
                _permissionInfo.IsAllowEdit = true;
                _permissionInfo.IsAllowDelete = true;
                poRecord.DefaultAccess = poRecord.DefaultAccess + _strActual;
            }
            else
            {
                #region Common Access Parameters
                string[] values = poRecord.CommonAccess.Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = values[i].Trim();
                    if (values[i] == "Add")
                        _permissionInfo.IsAllowAdd = true;

                    if (values[i] == "Edit")
                        _permissionInfo.IsAllowEdit = true;

                    if (values[i] == "Delete")
                        _permissionInfo.IsAllowDelete = true;

                }
                #endregion

            }
            #endregion

            #region DataAccessAll (Department Listing)
            if (_DataAccessIDs == true)
            {

                string _strActual = string.Empty ;

                #region Department Listing that mapped on Department Mapping Table
                GeneralModel oClass = new GeneralModel();

                // Extract Department List 
                var vResult = oClass.GetDepartmentListByDepartmentMapping().ToList();
                foreach(var Data in vResult)
                {
                    if (_strActual != string.Empty)
                    {
                        _strActual = _strActual + ",";
                    }
                    _strActual = _strActual + Data.ID.ToString();

                }


                #endregion

                poRecord.DataAccess = _strActual;//toSaves Department IDs 
                poRecord.DefaultAccess = poRecord.DefaultAccess + _strActual;

            }
            else
            {
                #region DataAccess Parameters
                string[] values = poRecord.DataAccess.Split(',');
                string _DepIDList = string.Empty;
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = values[i].Trim();
                    if (values[i] != "DataAccessAll")
                    {
                        if (_DepIDList != string.Empty)
                        {
                            _DepIDList = _DepIDList + ",";
                            poRecord.DefaultAccess = poRecord.DefaultAccess + ",";
                        }
                        poRecord.DefaultAccess = poRecord.DefaultAccess + values[i];
                        _DepIDList = _DepIDList + values[i];

                    }                 

                }

                poRecord.DataAccess = _DepIDList;

                #endregion
            }

            #endregion

            #region Processing
            if ((_MenuAccessIDs == true) && (_EnquiryIDs == true) && (_UserRightIDs == true) && (_GeneralTableIDs == true) && (_CommonAccessIDs == true) && (_DataAccessIDs == true))
            {
                #region Full Permission
                poRecord.DefaultAccess = "All";
                poRecord.MenuAccess = "All";
                poRecord.Enquiry = "All";
                poRecord.UserRight = "All";
                poRecord.GeneralTable = "All";

                bReturn = AllInfoInsertRolePermission(poRecord, true);
                #endregion

            }
            else
            {
                if ((poRecord.MenuAccess == "0") && (poRecord.Enquiry == "0") && (poRecord.UserRight == "0") && (poRecord.GeneralTable == "0"))
                    _Menufilter = string.Empty;

                #region Menu By Menu
                bReturn = InsertRolePermissionByPermission(poRecord, _Menufilter,_permissionInfo);
                #endregion
            }


            #endregion

            return bReturn;

        }

      public Int64 Insert(UserRole poRecord)
      {
         bool bReturn = true;
         Int64 _returnData = 0;
         //------------------------------
         try
         {
            poRecord.roleid = 0;
            //----------
            string sSQL = "" +
                "INSERT INTO DMUserRole " +
                "      ( " +
                "      role_name,role_desc,registered_date,registered_by, " + //update_date,update_by
                "       menu_access,data_access " +
                "      ) " +
                "      VALUES " +
                "      (" +
                "       @rolename,@roledesc,@registereddate,@registeredby, " + //@updateddate,@updatedby
                "       @menuaccess,@dataaccess " +
                "      )" +
                ";" +
                "SELECT @roleid = SCOPE_IDENTITY(); "
                ;

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter { ParameterName = "@roleid", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Output });
            oParameters.Add(new SqlParameter("@rolename", poRecord.rolename));
            oParameters.Add(new SqlParameter("@roledesc", poRecord.rolename));
            //oParameters.Add(new SqlParameter("@updateddate", DateTime.Now));
            //oParameters.Add(new SqlParameter("@updatedby", poRecord.registeredby));
            oParameters.Add(new SqlParameter("@registereddate", DateTime.Now));
            oParameters.Add(new SqlParameter("@registeredby", poRecord.registeredby));
            oParameters.Add(new SqlParameter("@menuaccess", poRecord.menuaccess));
            oParameters.Add(new SqlParameter("@dataaccess", poRecord.dataaccess));

            SqlParameter[] vSqlParameter = oParameters.ToArray();

            DatabaseContext oRemoteDB = new DatabaseContext();
            int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

            if (nReturn == 1)
            {
               poRecord.roleid = Convert.ToInt32(vSqlParameter[0].Value);
               _returnData = poRecord.roleid;
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
            // bReturn = false;
         }
         //------------------------------
         return _returnData;
      }

      public bool InsertRolePermissionByPermission(UserRole_REC poRecord, string _filter, UserRolePermission_REC Info)
        {
            bool bReturn = true;
            DatabaseContext oRemoteDB = new DatabaseContext();
            var oTransaction = oRemoteDB.Database.BeginTransaction();

            //------------------------------
            try
            {
                #region User Role Insert
                poRecord.RoleID = 0;
                poRecord.isActive = true;
                poRecord.CreatedDate = DateTime.Now;
                poRecord.UpdatedDate = DateTime.Now;

                string _LogQuery = "Insert into tblUserRole Table==>Role Name=" + poRecord.RoleName + ",Default Access =" + poRecord.DefaultAccess;
                //----------
                string sSQL = "" +
                    "INSERT INTO tblUserRole  " +
                    "      ( " +
                    "      RoleName, DefaultAccess, MenuAccess,Enquiry,UserRight,GeneralTable,CommonAccess,DataAccess,Finance,PIC,isActive," +
                    "      CreatedDate, CreatedBy,UpdatedDate,LogQuery " +
                    "      ) " +
                    "      VALUES " +
                    "      (" +
                    "      @RoleName, @DefaultAccess, @MenuAccess,@Enquiry,@UserRight,@GeneralTable,@CommonAccess,@DataAccess,@Finance,@PIC,@isActive, " +
                    "      @CreatedDate, @CreatedBy,@UpdatedDate,@LogQuery " +
                    "      )" +
                    ";" +
                    "SELECT @RoleID = SCOPE_IDENTITY(); "
                    ;

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter { ParameterName = "@RoleID", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Output });
                oParameters.Add(new SqlParameter("@RoleName", poRecord.RoleName));
                oParameters.Add(new SqlParameter("@DefaultAccess", poRecord.DefaultAccess));
                oParameters.Add(new SqlParameter("@MenuAccess", poRecord.MenuAccess));
                oParameters.Add(new SqlParameter("@Enquiry", poRecord.Enquiry));
                oParameters.Add(new SqlParameter("@UserRight", poRecord.UserRight));
                oParameters.Add(new SqlParameter("@GeneralTable", poRecord.GeneralTable));
                oParameters.Add(new SqlParameter("@CommonAccess", poRecord.CommonAccess));

                oParameters.Add(new SqlParameter("@DataAccess", poRecord.DataAccess));
                oParameters.Add(new SqlParameter("@Finance", poRecord.Finance));
                oParameters.Add(new SqlParameter("@PIC", poRecord.PIC));

                oParameters.Add(new SqlParameter("@isActive", poRecord.isActive));
                oParameters.Add(new SqlParameter("@CreatedDate", poRecord.CreatedDate));
                oParameters.Add(new SqlParameter("@CreatedBy", poRecord.CreatedBy));
                oParameters.Add(new SqlParameter("@UpdatedDate", poRecord.UpdatedDate));
                oParameters.Add(new SqlParameter("@LogQuery", _LogQuery));

                SqlParameter[] vSqlParameter = oParameters.ToArray();

                int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
                #endregion

                if (nReturn > 0)
                {
                    if (_filter != string.Empty)
                    {
                        #region UserRolePermission insert
                        poRecord.RoleID = Convert.ToInt64(vSqlParameter[0].Value);

                        sSQL = "" +
                      "INSERT INTO tblUserRolePermission  " +
                      "      ( " +
                      "      RoleID,MenuID,IsAllowAdd,IsAllowEdit,IsAllowDelete,isActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate" +
                      "      ) " +
                      "      select distinct" +
                      "      @RoleID,MenuID,@IsAllowAdd,@IsAllowEdit,@IsAllowDelete,1 as isActive,@CreatedBy,0 as UpdatedBy,getdate()as CreatedDate,getdate() as UpdatedDate from tblMenu WHERE Status=1 AND MenuID in (" + _filter + ")" +
                      ";"
                      ;

                        List<SqlParameter> oParameters1 = new List<SqlParameter>();

                        oParameters1.Add(new SqlParameter("@RoleID", poRecord.RoleID));
                        oParameters1.Add(new SqlParameter("@IsAllowAdd", Info.IsAllowAdd));
                        oParameters1.Add(new SqlParameter("@IsAllowEdit", Info.IsAllowEdit));
                        oParameters1.Add(new SqlParameter("@IsAllowDelete", Info.IsAllowDelete));
                        oParameters1.Add(new SqlParameter("@CreatedBy", poRecord.CreatedBy));

                        SqlParameter[] vSqlParameter1 = oParameters1.ToArray();
                        nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter1);
                        #endregion

                        if (nReturn > 0)
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
                    ErrorMessage = "Failed to insert record!";
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

        public bool AllInfoInsertRolePermission(UserRole_REC poRecord,bool _All)
        {
            bool bReturn = true;
            DatabaseContext oRemoteDB = new DatabaseContext();
            var oTransaction = oRemoteDB.Database.BeginTransaction();

            //------------------------------
            try
            {
                #region User Role Insert
                poRecord.RoleID = 0;

                //UserID
                poRecord.isActive = true;
                poRecord.CreatedDate = DateTime.Now;
                poRecord.UpdatedDate = DateTime.Now;
               
                string _LogQuery = "Insert into tblUserRole Table==>Role Name=" + poRecord.RoleName + ",Default Access =" + poRecord.DefaultAccess;
                //----------
                string sSQL = "" +
                    "INSERT INTO tblUserRole  " +
                    "      ( " +
                    "      RoleName, DefaultAccess, MenuAccess,Enquiry,UserRight,GeneralTable,CommonAccess,DataAccess,Finance,PIC,isActive," +
                    "      CreatedDate, CreatedBy,UpdatedDate,LogQuery " +
                    "      ) " +
                    "      VALUES " +
                    "      (" +
                    "      @RoleName, @DefaultAccess, @MenuAccess,@Enquiry,@UserRight,@GeneralTable,@CommonAccess,@DataAccess,@Finance,@PIC,@isActive, " +
                    "      @CreatedDate, @CreatedBy,@UpdatedDate,@LogQuery " +
                    "      )" +
                    ";" +
                    "SELECT @RoleID = SCOPE_IDENTITY(); "
                    ;

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter { ParameterName = "@RoleID", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Output });
                oParameters.Add(new SqlParameter("@RoleName", poRecord.RoleName));
                oParameters.Add(new SqlParameter("@DefaultAccess", poRecord.DefaultAccess));
                oParameters.Add(new SqlParameter("@MenuAccess", poRecord.MenuAccess));
                oParameters.Add(new SqlParameter("@Enquiry", poRecord.Enquiry));
                oParameters.Add(new SqlParameter("@UserRight", poRecord.UserRight));
                oParameters.Add(new SqlParameter("@GeneralTable", poRecord.GeneralTable));
                oParameters.Add(new SqlParameter("@CommonAccess", poRecord.CommonAccess));

                oParameters.Add(new SqlParameter("@DataAccess", poRecord.DataAccess));
                oParameters.Add(new SqlParameter("@Finance", poRecord.Finance));
                oParameters.Add(new SqlParameter("@PIC", poRecord.PIC));
            
                oParameters.Add(new SqlParameter("@isActive", poRecord.isActive));
                oParameters.Add(new SqlParameter("@CreatedDate", poRecord.CreatedDate));
                oParameters.Add(new SqlParameter("@CreatedBy", poRecord.CreatedBy));
                oParameters.Add(new SqlParameter("@UpdatedDate", poRecord.UpdatedDate));
                oParameters.Add(new SqlParameter("@LogQuery", _LogQuery));

                SqlParameter[] vSqlParameter = oParameters.ToArray();

                int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
                #endregion

                if (nReturn > 0)
                {
                    #region UserRolePermission insert
                    poRecord.RoleID = Convert.ToInt64(vSqlParameter[0].Value);

                     sSQL = "" +
                   "INSERT INTO tblUserRolePermission  " +
                   "      ( " +
                   "      RoleID,MenuID,IsAllowAdd,IsAllowEdit,IsAllowDelete,isActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate" +
                   "      ) " +
                   "      select distinct" +
                   "      @RoleID,MenuID,1 as IsAllowAdd,1 as IsAllowEdit,1 as IsAllowDelete,1 as isActive,@CreatedBy,0 as UpdatedBy,getdate()as CreatedDate,getdate() as UpdatedDate from tblMenu WHERE Status=1 " +
                   ";"
                   ;

                    List<SqlParameter> oParameters1 = new List<SqlParameter>();
                    oParameters1.Add(new SqlParameter("@RoleID", poRecord.RoleID));
                    oParameters1.Add(new SqlParameter("@CreatedBy", poRecord.CreatedBy));


                    SqlParameter[] vSqlParameter1 = oParameters1.ToArray();
                    int rReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter1);
                    #endregion

                    if(nReturn >0)
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
                    ErrorMessage = "Failed to insert record!";
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

      public bool AllInfoInsertRolePermission (UserRolePermission poRecord,bool status)
      {
         bool bReturn = true;
         //------------------------------
         try
         { 
            poRecord.registereddate = DateTime.Now;
            //----------
            string sSQL = "";
            if (status == true)
            {
               sSQL = "" +
             "INSERT INTO DMUserRoleFunctionMap  " +
              "      ( " +
              "      role_id, function_id, canInsert, canUpdate, canDelete, canViewDetail,canListing, registerDate, registerBy " +
              "      ) " +
              "      select " +
              "      @roleid as role_id,function_id as function_id,1 as canInsert,1 as canUpdate,1 as canDelete,1 as canViewDetail,1 as canListing, @registerDate as registerDate,@registerBy as registerBy FROM DMFunction " +
              ";"
              ;
            }
            else
            {
               sSQL = "" +
             "INSERT INTO DMUserRoleFunctionMap  " +
              "      ( " +
              "      role_id, function_id, canInsert, canUpdate, canDelete, canViewDetail,canListing, registerDate, registerBy " +
              "      ) " +
              "      select " +
              "      @roleid as role_id,function_id as function_id,0 as canInsert,0 as canUpdate,0 as canDelete,0 as canViewDetail,0 as canListing, @registerDate as registerDate,@registerBy as registerBy FROM DMFunction " +
              ";"
              ;
            }

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@roleid", poRecord.roleid));
            oParameters.Add(new SqlParameter("@registerDate", poRecord.registereddate));
            oParameters.Add(new SqlParameter("@registerBy", poRecord.registeredby));

            SqlParameter[] vSqlParameter = oParameters.ToArray();

            DatabaseContext oRemoteDB = new DatabaseContext();
            int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

            if (nReturn > 0)
            {
               //poRecord.Role_ID = Convert.ToInt64(vSqlParameter[0].Value);
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

      public bool AllInfoInsertRolePermission2(UserRolePermission poRecord, bool status)
      {
         bool bReturn = true;
         //------------------------------
         try
         {
            poRecord.updateddate = DateTime.Now;
            //----------
            string sSQL = "";
            if (status == true)
            {
               sSQL = "" +
             "INSERT INTO DMUserRoleFunctionMap  " +
              "      ( " +
              "      role_id, function_id, canInsert, canUpdate, canDelete, canViewDetail,canListing, registerDate, registerBy,updateDate,updateBy " +
              "      ) " +
              "      select " +
              "      @roleid as role_id,function_id as function_id,1 as canInsert,1 as canUpdate,1 as canDelete,1 as canViewDetail,1 as canListing, @registerDate as registerDate,@registerBy,@updateDate,@updateBy as registerBy FROM DMFunction " +
              ";"
              ;
            }
            else
            {
               sSQL = "" +
             "INSERT INTO DMUserRoleFunctionMap  " +
              "      ( " +
              "      role_id, function_id, canInsert, canUpdate, canDelete, canViewDetail,canListing, registerDate, registerBy,updateDate,updateBy " +
              "      ) " +
              "      select " +
              "      @roleid as role_id,function_id as function_id,0 as canInsert,0 as canUpdate,0 as canDelete,0 as canViewDetail,0 as canListing, @registerDate as registerDate,@registerBy,@updateDate,@updateBy as registerBy FROM DMFunction " +
              ";"
              ;
            }

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@roleid", poRecord.roleid));
            oParameters.Add(new SqlParameter("@registerDate", poRecord.registereddate));
            oParameters.Add(new SqlParameter("@registerBy", poRecord.registeredby));
            oParameters.Add(new SqlParameter("@updateDate", poRecord.updateddate));
            oParameters.Add(new SqlParameter("@updateBy", poRecord.updatedby));

            SqlParameter[] vSqlParameter = oParameters.ToArray();

            DatabaseContext oRemoteDB = new DatabaseContext();
            int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

            if (nReturn > 0)
            {
               //poRecord.Role_ID = Convert.ToInt64(vSqlParameter[0].Value);
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

      public bool InsertRolePermission(UserRolePermission poRecord)
      {
         bool bReturn = true;
         //------------------------------
         try
         {
            poRecord.registereddate = DateTime.Now;
            //----------
            string sSQL = "" +
                "INSERT INTO DMUserRoleFunctionMap  " +
                "      ( " +
                "     role_id, function_id, canInsert, canUpdate, canDelete, canViewDetail,canListing, registerDate, registerBy " +
                "      ) " +
                "      VALUES " +
                "      (" +
                "     @roleid, @functionid, @canInsert, @canUpdate, @canDelete, @canViewDetail,@canListing, @registerDate, @registerBy " +
                "      )" +
                ";"
                ;

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@roleid", poRecord.roleid));
            oParameters.Add(new SqlParameter("@functionid", poRecord.functionid));
            oParameters.Add(new SqlParameter("@registerDate", poRecord.registereddate));
            oParameters.Add(new SqlParameter("@registerBy", poRecord.registeredby));
            oParameters.Add(new SqlParameter("@canInsert", poRecord.canInsert));
            oParameters.Add(new SqlParameter("@canUpdate", poRecord.canUpdate));
            oParameters.Add(new SqlParameter("@canDelete", poRecord.canDelete));
            oParameters.Add(new SqlParameter("@canViewDetail", poRecord.canViewDetail));
            oParameters.Add(new SqlParameter("@canListing", poRecord.canListing));

            SqlParameter[] vSqlParameter = oParameters.ToArray();

            DatabaseContext oRemoteDB = new DatabaseContext();
            int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

            if (nReturn == 1)
            {
               bReturn = true;
               //poRecord.men = Convert.ToInt64(vSqlParameter[0].Value);
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

      public bool InsertRolePermission2(UserRolePermission poRecord)
      {
         bool bReturn = true;
         //------------------------------
         try
         {
            poRecord.updateddate = DateTime.Now;
            //----------
            string sSQL = "" +
                "INSERT INTO DMUserRoleFunctionMap  " +
                "      ( " +
                "     role_id, function_id, canInsert, canUpdate, canDelete, canViewDetail,canListing, registerDate, registerBy,updateDate,updateBy " +
                "      ) " +
                "      VALUES " +
                "      (" +
                "     @roleid, @functionid, @canInsert, @canUpdate, @canDelete, @canViewDetail,@canListing, @registerDate, @registerBy,@updateDate,@updateBy " +
                "      )" +
                ";"
                ;

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@roleid", poRecord.roleid));
            oParameters.Add(new SqlParameter("@functionid", poRecord.functionid));
            oParameters.Add(new SqlParameter("@registerDate", poRecord.registereddate));
            oParameters.Add(new SqlParameter("@registerBy", poRecord.registeredby));
            oParameters.Add(new SqlParameter("@canInsert", poRecord.canInsert));
            oParameters.Add(new SqlParameter("@canUpdate", poRecord.canUpdate));
            oParameters.Add(new SqlParameter("@canDelete", poRecord.canDelete));
            oParameters.Add(new SqlParameter("@canViewDetail", poRecord.canViewDetail));
            oParameters.Add(new SqlParameter("@canListing", poRecord.canListing));
            oParameters.Add(new SqlParameter("@updateDate", poRecord.updateddate));
            oParameters.Add(new SqlParameter("@updateBy", poRecord.updatedby));

            SqlParameter[] vSqlParameter = oParameters.ToArray();

            DatabaseContext oRemoteDB = new DatabaseContext();
            int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

            if (nReturn == 1)
            {
               bReturn = true;
               //poRecord.men = Convert.ToInt64(vSqlParameter[0].Value);
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

      #endregion

      #region Read

      #region Get User Role Name List

      public System.Data.Entity.Infrastructure.DbRawSqlQuery<FindDataByID> CheckUserRoleNameByID(long RoleID, string _RoleName)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "";

            sSQL = "select count(*) As  Rcount from DMUserRole with (nolock) where role_name='" + _RoleName + "' and role_id=" + RoleID;

            var vQuery = oRemoteDB.Database.SqlQuery<FindDataByID>(sSQL);
            return vQuery;
        }

      public System.Data.Entity.Infrastructure.DbRawSqlQuery<UserRoleFunction> GetFunctionData()
      {
         DatabaseContext oRemoteDB = new DatabaseContext();
         string sSQL = "";

         sSQL = "select function_id As functionid,function_name As functionname,function_code As functioncode,description from DMFunction with (nolock)";

         var vQuery = oRemoteDB.Database.SqlQuery<UserRoleFunction>(sSQL);
         return vQuery;
      }

      public System.Data.Entity.Infrastructure.DbRawSqlQuery<FindDataByID> CheckUserRoleName(string _RoleName)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "";

            sSQL = "select count(*) As  Rcount from DMUserRole with (nolock) where role_name='" + _RoleName + "'";

            var vQuery = oRemoteDB.Database.SqlQuery<FindDataByID>(sSQL);
            return vQuery;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<UserRole> GetList()
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "" +
                        "SELECT " +
                         "  role_id As roleid, " +
                         " (case when role_name is null then '' else role_name end) As rolename, " +
                         " (case when role_desc is null then '' else role_desc end) As roledesc, " +
                         " (case when update_by is null then '' else update_by end) As updatedby, " +
                         " (case when registered_by is null then '' else registered_by end) As registeredby, " +
                         " (case when menu_access is null then '' else menu_access end) As menuaccess, " +
                         " (case when data_access is null then '' else data_access end) As dataaccess, " +
                         "  update_date As updateddate,registered_date As registereddate,dbo.IsUsingRoleID(role_id) As IsUsing FROM DMUserRole with (nolock) " +
                         "   ORDER BY role_id " +
                        ";"
        ;

            var vQuery = oRemoteDB.Database.SqlQuery<UserRole>(sSQL);

            return vQuery;
        }

      
        #endregion

        #region Get User Role Name By ID
        public System.Data.Entity.Infrastructure.DbRawSqlQuery<UserRole> GetOneByID(long _RoleID)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();

            string sSQL = "" +
                        "SELECT " +
                         "    role_id As roleid,role_name As rolename,role_desc As roledesc, " +
                         "    registered_date As registereddate,registered_by As registeredby,menu_access As menuaccess,data_access As dataaccess  " +
                         "   FROM DMUserRole  with (nolock) " +
                         "   Where role_id= " + _RoleID +
                         "   ORDER BY role_name " +
                     ";"
                     ;

            var vQuery = oRemoteDB.Database.SqlQuery<UserRole>(sSQL);

            return vQuery;
        }
        #endregion

        #region Filter Role Name Search
        public System.Data.Entity.Infrastructure.DbRawSqlQuery<UserRole_REC> GetListForRoleSearch(string _filter)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();

            string sSQL = "" +
                "SELECT " +
                "       distinct RoleID, RoleName" +
                "   FROM tblUserRole  with (nolock) Where RoleName like '%" + _filter + "%'" +
                "   ORDER BY RoleName  " +
                ";"
                ;

            var vQuery = oRemoteDB.Database.SqlQuery<UserRole_REC>(sSQL);

            return vQuery;
        }
        #endregion

        #region Filter Department Search
        public System.Data.Entity.Infrastructure.DbRawSqlQuery<User_REC> GetListForDepartmentSearch(string _filter)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();

            string sSQL = "" +
                "SELECT " +
                "       distinct DepartmentID ,Department " +
                "   FROM tblDepartment  with (nolock) Where Department like '%" + _filter + "%'" +
                "   ORDER BY Department  " +
                ";"
                ;

            var vQuery = oRemoteDB.Database.SqlQuery<User_REC>(sSQL);

            return vQuery;
        }
        #endregion

        #endregion

        #region Update

        public bool UpdateRolePermissionByPermission(UserRole_REC poRecord, string _filter, UserRolePermission_REC Info)
        {
            bool bReturn = true;
            DatabaseContext oRemoteDB = new DatabaseContext();
            var oTransaction = oRemoteDB.Database.BeginTransaction();

            //------------------------------
            try
            {
                #region Update User Role 
                poRecord.UpdatedDate = DateTime.Now;
                string _LogQuery = "Update into tblUserRole Table==>RoleID=" + poRecord.RoleID + ",Role Name=" + poRecord.RoleName + ",Default Access =" + poRecord.DefaultAccess;

                //---------
                string sSQL = "" +
                    " Update tblUserRole  " +
                    "      SET  " +
                    "      RoleName=@RoleName," +
                    "      DefaultAccess=@DefaultAccess," +
                    "      MenuAccess=@MenuAccess, " +
                    "      Enquiry=@Enquiry, " +
                    "      UserRight=@UserRight," +
                    "      GeneralTable=@GeneralTable," +
                    "      CommonAccess=@CommonAccess," +
                    "      DataAccess=@DataAccess," +
                    "      Finance=@Finance," +
                    "      PIC=@PIC," +
                    "      UpdatedBy=@UpdatedBy," +
                    "      UpdatedDate=@UpdatedDate," +
                    "      LogQuery=@LogQuery " +
                    "     WHERE (RoleID=@RoleID)" +
                    ";"
                    ;

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@RoleID", poRecord.RoleID));
                oParameters.Add(new SqlParameter("@RoleName", poRecord.RoleName));
                oParameters.Add(new SqlParameter("@DefaultAccess", poRecord.DefaultAccess));
                oParameters.Add(new SqlParameter("@MenuAccess", poRecord.MenuAccess));
                oParameters.Add(new SqlParameter("@Enquiry", poRecord.Enquiry));
                oParameters.Add(new SqlParameter("@UserRight", poRecord.UserRight));
                oParameters.Add(new SqlParameter("@GeneralTable", poRecord.GeneralTable));
                oParameters.Add(new SqlParameter("@CommonAccess", poRecord.CommonAccess));

                oParameters.Add(new SqlParameter("@DataAccess", poRecord.DataAccess));
                oParameters.Add(new SqlParameter("@Finance", poRecord.Finance));
                oParameters.Add(new SqlParameter("@PIC", poRecord.PIC));

                oParameters.Add(new SqlParameter("@UpdatedBy", poRecord.UpdatedBy));
                oParameters.Add(new SqlParameter("@UpdatedDate", poRecord.UpdatedDate));
                oParameters.Add(new SqlParameter("@LogQuery", _LogQuery));

                SqlParameter[] vSqlParameter = oParameters.ToArray();

                int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
                #endregion


                if (nReturn > 0)
                {

                    #region Clear First UserRolePermission
                    sSQL = "" +
                        "DELETE tblUserRolePermission " +
                        "   WHERE (RoleID = @RoleID) " +
                        ";"
                        ;

                    List<SqlParameter> oDelParameters = new List<SqlParameter>();
                    oDelParameters.Add(new SqlParameter("@RoleID", poRecord.RoleID));
                    SqlParameter[] vDelSqlParameter = oDelParameters.ToArray();

                    nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
                    #endregion

                    if (nReturn >= 0)
                    {

                        #region User Role Permission Insert
                        if (_filter != string.Empty)
                        {
                            #region UserRolePermission insert
                            poRecord.RoleID = Convert.ToInt64(vSqlParameter[0].Value);

                            sSQL = "" +
                          "INSERT INTO tblUserRolePermission  " +
                          "      ( " +
                          "      RoleID,MenuID,IsAllowAdd,IsAllowEdit,IsAllowDelete,isActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate" +
                          "      ) " +
                          "      select distinct" +
                          "      @RoleID,MenuID,@IsAllowAdd,@IsAllowEdit,@IsAllowDelete,1 as isActive,@CreatedBy,0 as UpdatedBy,getdate()as CreatedDate,getdate() as UpdatedDate from tblMenu WHERE Status=1 AND MenuID in (" + _filter + ")" +
                          ";"
                          ;

                            List<SqlParameter> oParameters1 = new List<SqlParameter>();

                            oParameters1.Add(new SqlParameter("@RoleID", poRecord.RoleID));
                            oParameters1.Add(new SqlParameter("@IsAllowAdd", Info.IsAllowAdd));
                            oParameters1.Add(new SqlParameter("@IsAllowEdit", Info.IsAllowEdit));
                            oParameters1.Add(new SqlParameter("@IsAllowDelete", Info.IsAllowDelete));
                            oParameters1.Add(new SqlParameter("@CreatedBy", poRecord.UpdatedBy));

                            SqlParameter[] vSqlParameter1 = oParameters1.ToArray();
                            nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter1);
                            #endregion

                            if (nReturn > 0)
                            {
                                oTransaction.Commit();
                                bReturn = true;
                            }
                            else
                            {
                                oTransaction.Rollback();
                                ErrorMessage = "Failed to update record!";
                                bReturn = false;
                            }
                        }
                        else
                        {
                            oTransaction.Commit();
                            bReturn = true;
                        }


                        #endregion


                    }

                }
                else
                {
                    oTransaction.Rollback();
                    ErrorMessage = "Failed to update record!";
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

        public bool AllInfoUpdateRolePermission(UserRole_REC poRecord, bool _All)
        {
            bool bReturn = true;
            DatabaseContext oRemoteDB = new DatabaseContext();
            var oTransaction = oRemoteDB.Database.BeginTransaction();

            //------------------------------
            try
            {
                #region Update User Role 

                poRecord.isActive = true;
                poRecord.UpdatedDate = DateTime.Now;
       

                string _LogQuery = "Update into tblUserRole Table==>RoleID=" + poRecord.RoleID + ",Role Name=" + poRecord.RoleName + ",Default Access =" + poRecord.DefaultAccess;
                string sSQL = "" +
                    " Update tblUserRole  " +
                    "      SET  " +
                    "      RoleName=@RoleName," +
                    "      DefaultAccess=@DefaultAccess," +
                    "      MenuAccess=@MenuAccess, " +
                    "      Enquiry=@Enquiry, " +
                    "      UserRight=@UserRight," +
                    "      GeneralTable=@GeneralTable," + 
                    "      CommonAccess=@CommonAccess," +
                    "      DataAccess=@DataAccess," +
                    "      Finance=@Finance," +
                    "      PIC=@PIC," +
                    "      UpdatedBy=@UpdatedBy," +
                    "      UpdatedDate=@UpdatedDate," +
                    "      LogQuery=@LogQuery " +
                    "     WHERE (RoleID=@RoleID)" +
                    ";" 
                    ;

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@RoleID", poRecord.RoleID));
                oParameters.Add(new SqlParameter("@RoleName", poRecord.RoleName));
                oParameters.Add(new SqlParameter("@DefaultAccess", poRecord.DefaultAccess));
                oParameters.Add(new SqlParameter("@MenuAccess", poRecord.MenuAccess));
                oParameters.Add(new SqlParameter("@Enquiry", poRecord.Enquiry));
                oParameters.Add(new SqlParameter("@UserRight", poRecord.UserRight));
                oParameters.Add(new SqlParameter("@GeneralTable", poRecord.GeneralTable));
                oParameters.Add(new SqlParameter("@CommonAccess", poRecord.CommonAccess));

                oParameters.Add(new SqlParameter("@DataAccess", poRecord.DataAccess));
                oParameters.Add(new SqlParameter("@Finance", poRecord.Finance));
                oParameters.Add(new SqlParameter("@PIC", poRecord.PIC));
               
                //oParameters.Add(new SqlParameter("@isActive", poRecord.isActive));
          
                oParameters.Add(new SqlParameter("@UpdatedBy", poRecord.UpdatedBy));
                oParameters.Add(new SqlParameter("@UpdatedDate", poRecord.UpdatedDate));
                oParameters.Add(new SqlParameter("@LogQuery", _LogQuery));

                SqlParameter[] vSqlParameter = oParameters.ToArray();

                int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
                #endregion

                if (nReturn > 0)
                {
                    #region Clear First UserRolePermission
                    sSQL = "" +
                        "DELETE tblUserRolePermission " +
                        "   WHERE (RoleID = @RoleID) " +
                        ";"
                        ;

                    List<SqlParameter> oDelParameters = new List<SqlParameter>();
                    oDelParameters.Add(new SqlParameter("@RoleID", poRecord.RoleID));
                    SqlParameter[] vDelSqlParameter = oDelParameters.ToArray();

                    nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
                    #endregion

                    if(nReturn >= 0)
                    {
                        

                        #region UserRolePermission insert

                        sSQL = "" +
                      "INSERT INTO tblUserRolePermission  " +
                      "      ( " +
                      "      RoleID,MenuID,IsAllowAdd,IsAllowEdit,IsAllowDelete,isActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate" +
                      "      ) " +
                      "      select distinct" +
                      "      @RoleID,MenuID,1 as IsAllowAdd,1 as IsAllowEdit,1 as IsAllowDelete,1 as isActive,@CreatedBy,@CreatedBy as UpdatedBy,getdate()as CreatedDate,getdate() as UpdatedDate from tblMenu WHERE Status=1 " +
                      ";"
                      ;

                        List<SqlParameter> oParameters1 = new List<SqlParameter>();
                        oParameters1.Add(new SqlParameter("@RoleID", poRecord.RoleID));
                        oParameters1.Add(new SqlParameter("@CreatedBy", poRecord.UpdatedBy));


                        SqlParameter[] vSqlParameter1 = oParameters1.ToArray();
                         nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter1);
                        #endregion

                        if (nReturn > 0)
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
                        ErrorMessage = "Fail to Update record";
                        bReturn = false;
                    }


                }
                else
                {
                    oTransaction.Rollback();
                    ErrorMessage = "Failed to Update record!";
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

        public bool UpdateUserRole(UserRole_REC poRecord)
        {
            bool bReturn = true;

            #region Parameters

            _Menufilter = string.Empty;
            poRecord.DefaultAccess = string.Empty;

            _permissionInfo = new UserRolePermission_REC();
            _permissionInfo.IsAllowAdd = false;
            _permissionInfo.IsAllowEdit = false;
            _permissionInfo.IsAllowDelete = false;

            if (poRecord.MenuAccess.ToString().Contains("All"))
                _MenuAccessIDs = true;
            else
                _MenuAccessIDs = false;

            if ((poRecord.Enquiry.ToString().Contains("All")) || (poRecord.Enquiry.ToString().Contains("M3,")))
                _EnquiryIDs = true;
            else
                _EnquiryIDs = false;

            if ((poRecord.UserRight.ToString().Contains("All")) || (poRecord.UserRight.ToString().Contains("M5,")))
                _UserRightIDs = true;
            else
                _UserRightIDs = false;

            if ((poRecord.GeneralTable.ToString().Contains("All")) || (poRecord.GeneralTable.ToString().Contains("M4,")))
                _GeneralTableIDs = true;
            else
                _GeneralTableIDs = false;


            if (poRecord.CommonAccess.ToString().Contains("All"))
                _CommonAccessIDs = true;
            else
                _CommonAccessIDs = false;

            if (poRecord.DataAccess.ToString().Contains("All"))
                _DataAccessIDs = true;
            else
                _DataAccessIDs = false;

            if (poRecord.DepartmentRight.Trim() == "Finance".Trim())
            {
                poRecord.Finance = true;
                poRecord.PIC = false;
            }
            else
            {
                poRecord.PIC = true;
                poRecord.Finance = false;
            }

            #endregion

            #region AccessMenuAll ( AssetTagging,DataImport)
            if (_MenuAccessIDs == true)
            {
                string _strActual = "AssetTagging,DataImport";
                poRecord.MenuAccess = "All";
                _Menufilter = "'AssetTagging','DataImport'";
                poRecord.DefaultAccess = _strActual;
            }
            else
            {
                #region Access Menu parameters
                if (poRecord.MenuAccess != "0")
                {
                    _DefaultAccess = ExtractString(poRecord.MenuAccess.Trim().ToString());
                    poRecord.DefaultAccess = poRecord.DefaultAccess + _DefaultAccess;
                }
                #endregion
            }
            #endregion

            #region Enquiry ( M3,M3P1,M3P2,M3P3)
            if (_EnquiryIDs == true)
            {
                poRecord.Enquiry = "All";
                string _strActual = "AssetTagging,DataImport";

                if (_Menufilter != string.Empty)
                {
                    _Menufilter = _Menufilter + ",";
                    poRecord.DefaultAccess = poRecord.DefaultAccess + ",";
                }

                _Menufilter = _Menufilter + "'M3','M3P1','M3P2','M3P3'";
                poRecord.DefaultAccess = poRecord.DefaultAccess + _strActual;
            }
            else
            {
                #region Enquiry parameters
                if (poRecord.Enquiry != "0")
                {
                    _DefaultAccess = ExtractString(poRecord.Enquiry.Trim().ToString());
                    poRecord.DefaultAccess = poRecord.DefaultAccess + _DefaultAccess;
                }
                #endregion
            }
            #endregion

            #region User (M5,M5P1,M5P2)
            if (_UserRightIDs == true)
            {
                if (_Menufilter != string.Empty)
                {
                    _Menufilter = _Menufilter + ",";
                    poRecord.DefaultAccess = poRecord.DefaultAccess + ",";
                }
                string _strActual = "M5,M5P1,M5P2";
                poRecord.UserRight = "All";
                _Menufilter = _Menufilter + "'M5','M5P1','M5P2'";
                poRecord.DefaultAccess = poRecord.DefaultAccess + _strActual;

            }
            else
            {
                #region User parameters
                if (poRecord.UserRight != "0")
                {
                    _DefaultAccess = ExtractString(poRecord.UserRight.Trim().ToString());
                    poRecord.DefaultAccess = poRecord.DefaultAccess + _DefaultAccess;
                }
                #endregion
            }
            #endregion

            #region General Table (M4,M4P1,M4P2,M4P3,M4P4,M4P5,M4P6,M4P7,M4P8,M4P9,M4P10,M4P11,M4P12,M4P13)
            if (_GeneralTableIDs == true)
            {
                if (_Menufilter != string.Empty)
                {
                    _Menufilter = _Menufilter + ",";
                    poRecord.DefaultAccess = poRecord.DefaultAccess + ",";
                }
                string _strActual = "M4,M4P1,M4P2,M4P3,M4P4,M4P5,M4P6,M4P7,M4P8,M4P9,M4P10,M4P11,M4P12,M4P13";
                poRecord.GeneralTable = "All";
                _Menufilter = _Menufilter + "'M4','M4P1','M4P2','M4P3','M4P4','M4P5','M4P6','M4P7','M4P8','M4P9','M4P10','M4P11','M4P12','M4P13'";
                poRecord.DefaultAccess = poRecord.DefaultAccess + _strActual;
            }
            else
            {
                #region General Table parameters
                if (poRecord.GeneralTable != "0")
                {
                    _DefaultAccess = ExtractString(poRecord.GeneralTable.Trim().ToString());
                    poRecord.DefaultAccess = poRecord.DefaultAccess + _DefaultAccess;
                }

                #endregion
            }
            #endregion

            #region CommonAccessAll (Add,Edit,Delete)
            if (_CommonAccessIDs == true)
            {

                if (_Menufilter != string.Empty)
                {
                    poRecord.DefaultAccess = poRecord.DefaultAccess + ",";
                }
                string _strActual = "Add,Edit,Delete";
                poRecord.CommonAccess = "All";
                _permissionInfo.IsAllowAdd = true;
                _permissionInfo.IsAllowEdit = true;
                _permissionInfo.IsAllowDelete = true;
                poRecord.DefaultAccess = poRecord.DefaultAccess + _strActual;
            }
            else
            {
                #region Common Access Parameters
                string[] values = poRecord.CommonAccess.Split(',');
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = values[i].Trim();
                    if (values[i] == "Add")
                        _permissionInfo.IsAllowAdd = true;

                    if (values[i] == "Edit")
                        _permissionInfo.IsAllowEdit = true;

                    if (values[i] == "Delete")
                        _permissionInfo.IsAllowDelete = true;

                }
                #endregion

            }
            #endregion

            #region DataAccessAll (Department Listing)

            if (_DataAccessIDs == true)
            {

                string _strActual = string.Empty;

                #region Department Listing that mapped on Department Mapping Table
                GeneralModel oClass = new GeneralModel();

                // Extract Department List 
                var vResult = oClass.GetDepartmentListByDepartmentMapping().ToList();
                foreach (var Data in vResult)
                {
                    if (_strActual != string.Empty)
                    {
                        _strActual = _strActual + ",";
                    }
                    _strActual = _strActual + Data.ID.ToString();

                }


                #endregion

                poRecord.DataAccess = _strActual;//toSaves Department IDs 
                poRecord.DefaultAccess = poRecord.DefaultAccess + _strActual;

            }
            else
            {
                #region DataAccess Parameters
                string[] values = poRecord.DataAccess.Split(',');
                string _DepIDList = string.Empty;
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = values[i].Trim();
                    if (values[i] != "DataAccessAll")
                    {
                        if (_DepIDList != string.Empty)
                        {
                            _DepIDList = _DepIDList + ",";
                            poRecord.DefaultAccess = poRecord.DefaultAccess + ",";
                        }
                        poRecord.DefaultAccess = poRecord.DefaultAccess + values[i];
                        _DepIDList = _DepIDList + values[i];

                    }

                }

                poRecord.DataAccess = _DepIDList;

                #endregion
            }

            #endregion

            #region Processing
            if ((_MenuAccessIDs == true) && (_EnquiryIDs == true) && (_UserRightIDs == true) && (_GeneralTableIDs == true) && (_CommonAccessIDs == true) && (_DataAccessIDs == true))
            {
                #region Full Permission
                poRecord.DefaultAccess = "All";
                poRecord.MenuAccess = "All";
                poRecord.Enquiry = "All";
                poRecord.UserRight = "All";
                poRecord.GeneralTable = "All";
                poRecord.CommonAccess = "All";
                bReturn = AllInfoUpdateRolePermission(poRecord, true);
                #endregion

            }
            else
            {
                if ((poRecord.MenuAccess == "0") && (poRecord.Enquiry == "0") && (poRecord.UserRight == "0") && (poRecord.GeneralTable == "0"))
                    _Menufilter = string.Empty;

                bReturn = UpdateRolePermissionByPermission(poRecord, _Menufilter, _permissionInfo);
            }


            #endregion
            return bReturn;

        }

      public bool Update(UserRole poRecord)
      {
         bool bReturn = true;
         //------------------------------
         try
         {
            //----------
            string sSQL = "" +
                "UPDATE DMUserRole " +
                "   SET " +
                "      role_name = @rolename, " +
                  "    role_desc = @roledesc, " +
                "      menu_access = @menuaccess, " +
                "      data_access = @dataaccess, " +
                "      update_date = @updatedate, " +
                "      update_by = @updateby " +
                "   WHERE (role_id = @roleid) " +
                ";"
                ;

            List<SqlParameter> oParameters = new List<SqlParameter>();
           

            oParameters.Add(new SqlParameter("@roleid", poRecord.roleid));
            oParameters.Add(new SqlParameter("@rolename", poRecord.rolename));
            oParameters.Add(new SqlParameter("@roledesc", poRecord.rolename));
            oParameters.Add(new SqlParameter("@updatedate", DateTime.Now));
            oParameters.Add(new SqlParameter("@updateby", poRecord.updatedby));
            //oParameters.Add(new SqlParameter("@registereddate", DateTime.Now));
            //oParameters.Add(new SqlParameter("@registeredby", poRecord.registeredby));
            oParameters.Add(new SqlParameter("@menuaccess", poRecord.menuaccess));
            oParameters.Add(new SqlParameter("@dataaccess", poRecord.dataaccess));

            SqlParameter[] vSqlParameter = oParameters.ToArray();

            DatabaseContext oRemoteDB = new DatabaseContext();
            // var oTransaction = oRemoteDB.Database.BeginTransaction();
            int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
            if (nReturn == 1)
            {
               //oTransaction.Commit();
               bReturn = true;
            }
            else
            {
               // oTransaction.Rollback();
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

      public bool UpdateAllInfoRolePermission(UserRolePermission poRecord,bool status)
      {
         bool bReturn = true;
         //------------------------------
         try
         {
            poRecord.updateddate = DateTime.Now;
            if (status == true)
            {
               poRecord.canInsert = true;
               poRecord.canUpdate = true;
               poRecord.canDelete = true;
               poRecord.canViewDetail = true;
               poRecord.canListing = true;
            }
            else
            {
               poRecord.canInsert = false;
               poRecord.canUpdate = false;
               poRecord.canDelete = false;
               poRecord.canViewDetail = false;
               poRecord.canListing = false;
            }

            string sSQL = "" +
                "UPDATE DMUserRoleFunctionMap " +
                "   SET " +
                "      updateDate = @updateDate, " +
                "      updateBy = @updateBy, " +
                "      canInsert = @canInsert, " +
                "      canUpdate = @canUpdate, " +
                "      canDelete = @canDelete, " +
                "      canViewDetail = @canViewDetail, " +
                "      canListing = @canListing " +
                "   WHERE role_id=@roleid " +
                ";"
                ;

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@roleid", poRecord.roleid));
            oParameters.Add(new SqlParameter("@updateDate", poRecord.updateddate));
            oParameters.Add(new SqlParameter("@updateBy", poRecord.updatedby));
            oParameters.Add(new SqlParameter("@canInsert", poRecord.canInsert));
            oParameters.Add(new SqlParameter("@canUpdate", poRecord.canUpdate));
            oParameters.Add(new SqlParameter("@canDelete", poRecord.canDelete));
            oParameters.Add(new SqlParameter("@canViewDetail", poRecord.canViewDetail));
            oParameters.Add(new SqlParameter("@canListing", poRecord.canListing));

            SqlParameter[] vSqlParameter = oParameters.ToArray();

            DatabaseContext oRemoteDB = new DatabaseContext();
            // var oTransaction = oRemoteDB.Database.BeginTransaction();
            int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
            if (nReturn >= 1)
            {
               bReturn = true;
               // oTransaction.Commit();
            }
            else
            {
               // oTransaction.Rollback();
               ErrorMessage = "Failed to update record!" + "<br>" + "Record has been updated or deleted by another user.";
               bReturn = false;
            }

            #region Close Connection String
            if (oRemoteDB.Database.Connection.State == ConnectionState.Open)
            {
               oRemoteDB.Database.Connection.Close();
               oRemoteDB.Dispose();
            }
            #endregion
         }
         catch (Exception ex)
         {
            ErrorMessage = ex.Message;
            bReturn = false;
         }
         //------------------------------
         return bReturn;
      }

      public bool UpdateOneByOneRolePermission(UserRolePermission poRecord)
      {
         bool bReturn = true;
         //------------------------------
         try
         {
            poRecord.updateddate = DateTime.Now;

            string sSQL = "" +
               "UPDATE DMUserRoleFunctionMap " +
               "   SET " +
               "      updateDate = @updateDate, " +
               "      updateBy = @updateBy, " +
               "      canInsert = @canInsert, " +
               "      canUpdate = @canUpdate, " +
               "      canDelete = @canDelete, " +
               "      canViewDetail = @canViewDetail, " +
               "      canListing = @canListing " +
               "   WHERE role_id=@roleid " +
                  "   AND function_id=@functionid " +
               ";"
               ;

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@roleid", poRecord.roleid));
            oParameters.Add(new SqlParameter("@functionid", poRecord.functionid));
            oParameters.Add(new SqlParameter("@updateDate", poRecord.updateddate));
            oParameters.Add(new SqlParameter("@updateBy", poRecord.updatedby));
            oParameters.Add(new SqlParameter("@canInsert", poRecord.canInsert));
            oParameters.Add(new SqlParameter("@canUpdate", poRecord.canUpdate));
            oParameters.Add(new SqlParameter("@canDelete", poRecord.canDelete));
            oParameters.Add(new SqlParameter("@canViewDetail", poRecord.canViewDetail));
            oParameters.Add(new SqlParameter("@canListing", poRecord.canListing));

            SqlParameter[] vSqlParameter = oParameters.ToArray();

            DatabaseContext oRemoteDB = new DatabaseContext();
            // var oTransaction = oRemoteDB.Database.BeginTransaction();
            int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);
            if (nReturn >= 1)
            {
               bReturn = true;
               // oTransaction.Commit();
            }
            else
            {
               // oTransaction.Rollback();
               ErrorMessage = "Failed to update record!" + "<br>" + "Record has been updated or deleted by another user.";
               bReturn = false;
            }

            #region Close Connection String
            if (oRemoteDB.Database.Connection.State == ConnectionState.Open)
            {
               oRemoteDB.Database.Connection.Close();
               oRemoteDB.Dispose();
            }
            #endregion
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

      #region Delete
      public bool DeleteAllUserRole(Role_REC poRecord)
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
                    "   WHERE (RoleName = @RoleName); " +
                    "DELETE tblRole " +
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
                    bReturn = true;

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
                oTransaction.Rollback();
                bReturn = false;
            }
            //------------------------------
            return bReturn;
        }

      public bool DeleteOnlyUserRole(UserRole poRecord)
      {
         bool bReturn = true;
         //------------------------------
         try
         {
            //----------
            string sSQL = "" +
                  "DELETE DMUserRoleFunctionMap " +
                "   WHERE (role_id = @RoleID) " +
                ";"
                ;

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@RoleID", poRecord.roleid));
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

      public bool Delete(UserRole_REC poRecord)
        {
            bool bReturn = true;
            //------------------------------
            try
            {
                string sSQL = "" +
                    "DELETE tblUserRole " +
                    "   WHERE (RoleID = @RoleID) " +
                    ";"
                    ;

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@RoleID", poRecord.RoleID));

                SqlParameter[] vSqlParameter = oParameters.ToArray();

                DatabaseContext oRemoteDB = new DatabaseContext();
                var oTransaction = oRemoteDB.Database.BeginTransaction();
                int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

                if (nReturn > 0)
                {
                    oTransaction.Commit();
                    bReturn = true;
                }
                else
                {
                    oTransaction.Rollback();
                    ErrorMessage = "Failed to delete record!"  ;

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

        public bool DeleteRolePermissionOnly(UserRole_REC poRecord)
        {
            bool bReturn = true;
            //------------------------------
            try
            {
                string sSQL = "" +
                    "DELETE tblUserRolePermission " +
                    "   WHERE (RoleID = @RoleID) " +
                    ";"
                    ;

                List<SqlParameter> oParameters = new List<SqlParameter>();
                oParameters.Add(new SqlParameter("@RoleID", poRecord.RoleID));

                SqlParameter[] vSqlParameter = oParameters.ToArray();

                DatabaseContext oRemoteDB = new DatabaseContext();
                var oTransaction = oRemoteDB.Database.BeginTransaction();
                int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

                if (nReturn >= 0)
                {
                    oTransaction.Commit();
                    bReturn = true;
                }
                else
                {
                    oTransaction.Rollback();
                    ErrorMessage = "Failed to delete record!"  ;

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

        #endregion
    }

    
}