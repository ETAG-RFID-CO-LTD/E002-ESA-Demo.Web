using EagleServicesWebApp.Components;
using EagleServicesWebApp.Models.RollRoyceSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using EagleServicesWebApp.Models;

namespace EagleServicesWebApp.Models.GeneralTableModel
{
   
   public class GeneralTableModel
   {
      public bool? isActive { get; set; }
      public long? CreatedBy { get; set; }
      public DateTime? CreatedDate { get; set; }
      public long? UpdatedBy { get; set; }
      public DateTime? UpdatedDate { get; set; }
      public string LogQuery { get; set; }
   }

   public class Data : GeneralTableModel
   {
      public long ID { get; set; }
      public string Name { get; set; }
      public string Description { get; set; }
   }

   public class DepMappingDataView: GeneralTableModel
   {
      public long RecordID { get; set; }
      public long DepartmentID { get; set; }
      public string Department { get; set; }
      public long CostCenterID { get; set; }
      public string CostCenter { get; set; }
   }

   public class FindDataByID
   {
      public int Rcount { get; set; }
   }

   public class GeneralModel
   {
      public int ErrorNo { get; set; }
      public string ErrorMessage { get; set; }

      public GeneralModel()
      {
         ErrorNo = 0;
         ErrorMessage = "";
      }

        #region  For Use in User Access Right Department List
        public System.Data.Entity.Infrastructure.DbRawSqlQuery<Data> GetDepartmentListByDepartmentMapping()
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "";
            sSQL = "select DepartmentID As ID,Department As Name from tblDepartment with (nolock) WHERE IsActive = 1 AND DepartmentID in ( Select DepartmentID from tblDepartmentMapping) ORDER bY DepartmentID desc";

            var vQuery = oRemoteDB.Database.SqlQuery<Data>(sSQL);
            return vQuery;
        }

        #endregion 

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<Data> GetDepartmentList()
      {
         DatabaseContext oRemoteDB = new DatabaseContext();
         string sSQL = "";
         sSQL = "select DepartmentID As ID,Department As Name,isActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,(case when Description is null then '' else Description end) As Description from tblDepartment with (nolock) where IsActive = 1 order by DepartmentID desc";

         var vQuery = oRemoteDB.Database.SqlQuery<Data>(sSQL);
         return vQuery;
      }

      public System.Data.Entity.Infrastructure.DbRawSqlQuery<Data> GetCostCenterList(string selectedID)
      {
         DatabaseContext oRemoteDB = new DatabaseContext();
         string sSQL = "";
         sSQL = "select CostCenter_ID As ID,CostCenter As Name,isActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,(case when Description is null then '' else Description end) As Description from tblCostCenter with (nolock) " +
            " where IsActive = 1 and CostCenter_ID not in (select CostCenter_ID from tblDepartmentMapping with (nolock) where CostCenter_ID not in ('" + int.Parse(selectedID) + "')) order by CostCenter_ID desc";

         var vQuery = oRemoteDB.Database.SqlQuery<Data>(sSQL);
         return vQuery;
      }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<Data> GetIOCreationYear(string _filter)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
 
             string sSQL = "" +
                "SELECT " +
                "       distinct IOCreationYear as ID" +
                "   FROM tblAsset with (nolock) Where IOCreationYear like '%" + _filter + "%'" +
                "   ORDER BY IOCreationYear  " +
                ";"
                ;

            var vQuery = oRemoteDB.Database.SqlQuery<Data>(sSQL);

            return vQuery;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<Data> GeCostCentertListByDepartmentID(string TableName)
        {

            #region General Table Data Message From XML
            ErrorMessageModel Model = new ErrorMessageModel();

            GeneralTableMsg Fields = Model.GeneralTableMsgSelect();
            #endregion

            long RoleID = Convert.ToInt64(System.Web.HttpContext.Current.Session["RoleID"]);
            string DepartmentID = System.Web.HttpContext.Current.Session["DepartmentID"].ToString();

            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "";
            if (TableName.ToUpper() == Fields.CostCenter)
                sSQL = "select CostCenter_ID As ID,CostCenter As Name,isActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,(case when Description is null then '' else Description end) As Description " +
                      " from tblCostCenter WITH (NOLOCK) where IsActive = 1 " +
                      " AND CostCenter_ID in (" +
                        "  SELECT CostCenter_ID FROM tblDepartmentMapping WITH (NOLOCK) WHERE DepartmentID in (" +
   "  SELECT DepartmentID FROM intlist_to_tbl('" + DepartmentID + "')))" +
                      "order by CostCenter_ID desc";
            var vQuery = oRemoteDB.Database.SqlQuery<Data>(sSQL);
            return vQuery;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<Data> GetListSearch(string TableName, string filter)
        {

            #region General Table Data Message From XML
            ErrorMessageModel Model = new ErrorMessageModel();

            GeneralTableMsg Fields = Model.GeneralTableMsgSelect();
            #endregion
            // with(nolock) Where RoleName like '%" + _filter + "%'" +
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "";
            if (TableName.ToUpper() == Fields.AssetPIC)
                sSQL = "select AssetPIC_ID As ID,AssetPIC As Name from tblAssetPIC  with (nolock) Where AssetPIC like  '%" + filter + "%' AND IsActive = 1   order by AssetPIC_ID desc";
            else if (TableName.ToUpper() == Fields.CostCenter)
                sSQL = "select CostCenter_ID As ID,CostCenter As Name from tblCostCenter with (nolock) Where CostCenter like  '%" + filter + "%' AND IsActive = 1 order by CostCenter_ID desc";
            else if (TableName.ToUpper() == Fields.PAR)
                sSQL = "select PAR_ID As ID,PAR As Name from tblPAR with (nolock) Where PAR like  '%" + filter + "%' AND IsActive = 1 order by PAR_ID desc";
            else if (TableName.ToUpper() == Fields.Responser)
                sSQL = "select Responser_ID As ID,Responser As Name from tblResponser with (nolock) Where Responser like  '%" + filter + "%' AND IsActive = 1 order by Responser_ID desc";
            else if (TableName.ToUpper() == Fields.AssetClass)
                sSQL = "select AssetClass_ID As ID,AssetClass As Name from tblAssetClass with (nolock) Where AssetClass like  '%" + filter + "%' AND IsActive = 1 order by AssetClass_ID desc";
            else if (TableName.ToUpper() == Fields.ActivityType)
                sSQL = "select ActivityType_ID As ID,ActivityType As Name from tblActivityType with (nolock) Where ActivityType like  '%" + filter + "%' AND IsActive=1 order by ActivityType_ID desc";
            else if (TableName.ToUpper() == Fields.VendorCode)
                sSQL = "select VendorCode_ID As ID,VendorCode As Name from tblVendorCode with (nolock) Where VendorCode like  '%" + filter + "%' AND  IsActive = 1 order by VendorCode_ID desc";
            else if (TableName.ToUpper() == Fields.CustodyPerson || TableName.ToUpper() == Fields.CustodyPeron)
                sSQL = "select CustodyPerson_ID As ID,CustodyPerson As Name  from tblCustodyPerson with (nolock) Where CustodyPerson like  '%" + filter + "%' AND  IsActive = 1 order by CustodyPerson_ID desc";
            else if (TableName.ToUpper() == Fields.Location)
                sSQL = "select Location_ID As ID,Location As Name from tblLocation with (nolock) Where Location like  '%" + filter + "%' AND  IsActive = 1 order by Location_ID desc";
            else if (TableName.ToUpper() == Fields.Room)
                sSQL = "select Room_ID As ID,Room As Name from tblRoom with (nolock) Where Room like  '%" + filter + "%' AND  IsActive = 1 order by Room_ID desc";
            else if (TableName.ToUpper() == Fields.TaxIncentive)
                sSQL = "select TaxIncentive_ID As ID,TaxIncentive As Name from tblTaxIncentive with (nolock) Where TaxIncentive like  '%" + filter + "%' AND  IsActive = 1 order by TaxIncentive_ID desc";
            else if (TableName.ToUpper() == Fields.Department)
                sSQL = "select DepartmentID As ID,Department As Name from tblDepartment with (nolock) Where Department like  '%" + filter + "%' AND  IsActive = 1 order by DepartmentID desc";

            var vQuery = oRemoteDB.Database.SqlQuery<Data>(sSQL);
            return vQuery;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<Data> GetList(string TableName)
        {

            #region General Table Data Message From XML
            ErrorMessageModel Model = new ErrorMessageModel();

            GeneralTableMsg Fields = Model.GeneralTableMsgSelect();
            #endregion

            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "";
            if (TableName.ToUpper() == Fields.AssetPIC)
                sSQL = "select AssetPIC_ID As ID,AssetPIC As Name,isActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,(case when Description is null then '' else Description end) As Description from tblAssetPIC with (nolock) where IsActive = 1 order by AssetPIC_ID desc";
            else if (TableName.ToUpper() == Fields.CostCenter)
                sSQL = "select CostCenter_ID As ID,CostCenter As Name,isActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,(case when Description is null then '' else Description end) As Description from tblCostCenter with (nolock) where IsActive = 1 order by CostCenter_ID desc";
            else if (TableName.ToUpper() == Fields.PAR)
                sSQL = "select PAR_ID As ID,PAR As Name,isActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,(case when Description is null then '' else Description end) As Description from tblPAR with (nolock) where IsActive = 1 order by PAR_ID desc";
            else if (TableName.ToUpper() == Fields.Responser)
                sSQL = "select Responser_ID As ID,Responser As Name,isActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,(case when Description is null then '' else Description end) As Description from tblResponser with (nolock) where IsActive = 1 order by Responser_ID desc";
            else if (TableName.ToUpper() == Fields.AssetClass)
                sSQL = "select AssetClass_ID As ID,AssetClass As Name,isActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,(case when Description is null then '' else Description end) As Description from tblAssetClass with (nolock) where IsActive = 1 order by AssetClass_ID desc";
            else if (TableName.ToUpper() == Fields.ActivityType)
                sSQL = "select ActivityType_ID As ID,ActivityType As Name,isActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,(case when Description is null then '' else Description end) As Description from tblActivityType with (nolock) where IsActive = 1 order by ActivityType_ID desc";
            else if (TableName.ToUpper() == Fields.VendorCode)
                sSQL = "select VendorCode_ID As ID,VendorCode As Name,isActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,(case when Description is null then '' else Description end) As Description from tblVendorCode with (nolock) where IsActive = 1 order by VendorCode_ID desc";
            else if (TableName.ToUpper() == Fields.CustodyPerson || TableName.ToUpper() == Fields.CustodyPeron)
                sSQL = "select CustodyPerson_ID As ID,CustodyPerson As Name,isActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,(case when Description is null then '' else Description end) As Description from tblCustodyPerson with (nolock) where IsActive = 1 order by CustodyPerson_ID desc";
            else if (TableName.ToUpper() == Fields.Location)
                sSQL = "select Location_ID As ID,Location As Name,isActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,(case when Description is null then '' else Description end) As Description from tblLocation with (nolock) where IsActive = 1 order by Location_ID desc";
            else if (TableName.ToUpper() == Fields.Room)
                sSQL = "select Room_ID As ID,Room As Name,isActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,(case when Description is null then '' else Description end) As Description from tblRoom with (nolock) where IsActive = 1 order by Room_ID desc";
            else if (TableName.ToUpper() == Fields.TaxIncentive)
                sSQL = "select TaxIncentive_ID As ID,TaxIncentive As Name,isActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,(case when Description is null then '' else Description end) As Description from tblTaxIncentive with (nolock) where IsActive = 1 order by TaxIncentive_ID desc";
            else if (TableName.ToUpper() == Fields.Department)
                sSQL = "select DepartmentID As ID,Department As Name,isActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,(case when Description is null then '' else Description end) As Description from tblDepartment with (nolock) where IsActive = 1 order by DepartmentID desc";

            var vQuery = oRemoteDB.Database.SqlQuery<Data>(sSQL);
            return vQuery;
        }

      public System.Data.Entity.Infrastructure.DbRawSqlQuery<DepMappingDataView> GetDepartmentMappingList(string TableName)
      {
         DatabaseContext oRemoteDB = new DatabaseContext();
         string sSQL = "";
            sSQL = "select * from DepartmentMapping_vw with (nolock) order by RecordID desc";
         
         var vQuery = oRemoteDB.Database.SqlQuery<DepMappingDataView>(sSQL);
         return vQuery;
      }

      public System.Data.Entity.Infrastructure.DbRawSqlQuery<FindDataByID> GetAssetTableByID(string id,string TableName)
      {
            #region General Table Data Message From XML
            ErrorMessageModel Model = new ErrorMessageModel();

            GeneralTableMsg Fields = Model.GeneralTableMsgSelect();
            #endregion

            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "";

         if (TableName.ToUpper() == Fields.AssetPIC)
            sSQL = "select count(*) As  Rcount from tblAsset with (nolock) where AssetPIC_ID=" + id;
         else if (TableName.ToUpper() == Fields.CostCenter)
            sSQL = "select count(*) As  Rcount from tblAsset with (nolock) where CostCenter_ID=" + id;
         else if (TableName.ToUpper() == Fields.PAR)
            sSQL = "select count(*) As  Rcount from tblAsset with (nolock) where PAR_ID=" + id;
         else if (TableName.ToUpper() == Fields.Responser)
            sSQL = "select count(*) As  Rcount from tblAsset with (nolock) where Responser_ID=" + id;
         else if (TableName.ToUpper() == Fields.AssetClass)
            sSQL = "select count(*) As  Rcount from tblAsset with (nolock) where AssetClass_ID=" + id;
         else if (TableName.ToUpper() == Fields.ActivityType)
            sSQL = "select count(*) As  Rcount from tblAsset with (nolock) where ActivityType_ID=" + id;
         else if (TableName.ToUpper() == Fields.VendorCode)
            sSQL = "select count(*) As  Rcount from tblAsset with (nolock) where VendorCode_ID=" + id;
         else if (TableName.ToUpper() == Fields.CustodyPerson || TableName.ToUpper() == Fields.CustodyPeron)
            sSQL = "select count(*) As  Rcount from tblAsset with (nolock) where CustodyPerson_ID=" + id;
         else if (TableName.ToUpper() == Fields.Location)
            sSQL = "select count(*) As  Rcount from tblAsset with (nolock) where Location_ID=" + id;
         else if (TableName.ToUpper() == Fields.Room)
            sSQL = "select count(*) As  Rcount from tblAsset with (nolock) where  Room_ID=" + id;
         else if (TableName.ToUpper() ==Fields.TaxIncentive)
            sSQL = "select count(*) As  Rcount from tblAsset with (nolock) where TaxIncentive_ID=" + id;

         var vQuery = oRemoteDB.Database.SqlQuery<FindDataByID>(sSQL);
         return vQuery;
      }

      public System.Data.Entity.Infrastructure.DbRawSqlQuery<FindDataByID> GTCheckAlreadtExitByName(string id,string name, string TableName)
      {
            #region General Table Data Message From XML
            ErrorMessageModel Model = new ErrorMessageModel();

            GeneralTableMsg Fields = Model.GeneralTableMsgSelect();
            #endregion

            DatabaseContext oRemoteDB = new DatabaseContext();
         string sSQL = "";
         if (TableName.ToUpper() == Fields.AssetPIC)
            sSQL = "select count(*) As  Rcount from tblAssetPIC with (nolock) where AssetPIC='" + name + "' and AssetPIC_ID <> '" + id + "'";
         else if (TableName.ToUpper() == Fields.CostCenter)
            sSQL = "select count(*) As  Rcount from tblCostCenter with (nolock) where CostCenter='" + name + "' and CostCenter_ID <> '" + id + "'";
         else if (TableName.ToUpper() == Fields.PAR)
            sSQL = "select count(*) As  Rcount from tblPAR with (nolock) where PAR='" + name + "' and PAR_ID <> '" + id + "'";
         else if (TableName.ToUpper() == Fields.Responser)
            sSQL = "select count(*) As  Rcount from tblResponser with (nolock) where Responser='" + name + "' and Responser_ID <> '" + id + "'";
         else if (TableName.ToUpper() == Fields.AssetClass)
            sSQL = "select count(*) As  Rcount from tblAssetClass with (nolock) where AssetClass='" + name + "' and AssetClass_ID <> '" + id + "'";
         else if (TableName.ToUpper() == Fields.ActivityType)
            sSQL = "select count(*) As  Rcount from tblActivityType with (nolock) where ActivityType='" + name + "' and ActivityType_ID <> '" + id + "'";
         else if (TableName.ToUpper() == Fields.VendorCode)
            sSQL = "select count(*) As  Rcount from tblVendorCode with (nolock) where VendorCode='" + name + "' and VendorCode_ID <> '" + id + "'";
         else if (TableName.ToUpper() == Fields.CustodyPerson || TableName.ToUpper() == Fields.CustodyPeron)
            sSQL = "select count(*) As  Rcount from tblCustodyPerson with (nolock)where CustodyPerson='" + name + "' and CustodyPerson_ID <> '" + id + "'";
         else if (TableName.ToUpper() == Fields.Location)
            sSQL = "select count(*) As  Rcount from tblLocation with (nolock) where Location='" + name + "' and Location_ID <> '" + id + "'";
         else if (TableName.ToUpper() == Fields.Room)
            sSQL = "select count(*) As  Rcount from tblRoom with (nolock) where  Room='" + name + "' and Room_ID <> '" + id + "'";
         else if (TableName.ToUpper() == Fields.TaxIncentive)
            sSQL = "select count(*) As  Rcount from tblTaxIncentive with (nolock) where TaxIncentive='" + name + "' and TaxIncentive_ID <> '" + id + "'";
         else if (TableName.ToUpper() == Fields.Department)
            sSQL = "select count(*) As  Rcount from tblDepartment with (nolock) where Department='" + name + "' and DepartmentID <> '" + id + "'";

         var vQuery = oRemoteDB.Database.SqlQuery<FindDataByID>(sSQL);
         return vQuery;
      }

      public System.Data.Entity.Infrastructure.DbRawSqlQuery<FindDataByID> GetMappingTableByID(string id, string TableName)
      {
         #region General Table Data Message From XML
            ErrorMessageModel Model = new ErrorMessageModel();

            GeneralTableMsg Fields = Model.GeneralTableMsgSelect();
            #endregion

            DatabaseContext oRemoteDB = new DatabaseContext();
         string sSQL = "";
         if (TableName.ToUpper() == Fields.Department)
            sSQL = "select count(*) As  Rcount from tblDepartmentMapping with (nolock) where DepartmentID=" + id;
         else if (TableName.ToUpper() == Fields.CostCenter)
            sSQL = "select count(*) As  Rcount from tblDepartmentMapping with (nolock) where CostCenter_ID=" + id;
        
         var vQuery = oRemoteDB.Database.SqlQuery<FindDataByID>(sSQL);
         return vQuery;
      }

      public System.Data.Entity.Infrastructure.DbRawSqlQuery<FindDataByID> MappingTableByID(string RecordID,string Did, string Cid)
      {
         DatabaseContext oRemoteDB = new DatabaseContext();
         string sSQL = "";
            sSQL = "select count(*) As  Rcount from tblDepartmentMapping with (nolock) where RecordID <> " + RecordID + " and DepartmentID=" + Did + " and CostCenter_ID=" + Cid;

         var vQuery = oRemoteDB.Database.SqlQuery<FindDataByID>(sSQL);
         return vQuery;
      }

      public System.Data.Entity.Infrastructure.DbRawSqlQuery<FindDataByID> GetUserTableByID(string id, string TableName)
      {
         DatabaseContext oRemoteDB = new DatabaseContext();
         string sSQL = "";   
            sSQL = "select count(*) As  Rcount from tblUser with (nolock) where DepartmentID=" + id;

         var vQuery = oRemoteDB.Database.SqlQuery<FindDataByID>(sSQL);
         return vQuery;
      }

      public bool Insert(Data poRecord,string TableName)
      {
         bool bReturn = true;
         //------------------------------
         try
         {
            poRecord.ID = 0;
            string sSQL = "";

                #region General Table Data Message From XML
                ErrorMessageModel Model = new ErrorMessageModel();

                GeneralTableMsg Fields = Model.GeneralTableMsgSelect();
                #endregion

                long UserID = Convert.ToInt64(System.Web.HttpContext.Current.Session["UserGUID"]);

            if (TableName.ToUpper() == Fields.AssetPIC)
               sSQL = "INSERT INTO tblAssetPIC  (AssetPIC,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
     "SELECT @ID = SCOPE_IDENTITY(); ";
            else if (TableName.ToUpper() == Fields.CostCenter)
               sSQL = "INSERT INTO tblCostCenter  (CostCenter,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
     "SELECT @ID = SCOPE_IDENTITY(); ";
            else if (TableName.ToUpper() == Fields.PAR)
               sSQL = "INSERT INTO tblPAR  (PAR,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
    "SELECT @ID = SCOPE_IDENTITY(); ";
            else if (TableName.ToUpper() == Fields.Responser)
               sSQL = "INSERT INTO tblResponser  (Responser,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
     "SELECT @ID = SCOPE_IDENTITY(); ";
            else if (TableName.ToUpper() == Fields.ActivityType)
               sSQL = "INSERT INTO tblActivityType  (ActivityType,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
    "SELECT @ID = SCOPE_IDENTITY(); ";
            else if (TableName.ToUpper() == Fields.VendorCode)
               sSQL = "INSERT INTO tblVendorCode  (VendorCode,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
    "SELECT @ID = SCOPE_IDENTITY(); ";
            else if (TableName.ToUpper() == Fields.CustodyPerson || TableName.ToUpper() == Fields.CustodyPeron)
               sSQL = "INSERT INTO tblCustodyPerson  (CustodyPerson,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
     "SELECT @ID = SCOPE_IDENTITY(); ";
            else if (TableName.ToUpper() == Fields.Location)
               sSQL = "INSERT INTO tblLocation  (Location,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
     "SELECT @ID = SCOPE_IDENTITY(); ";
            else if (TableName.ToUpper() == Fields.Room)
               sSQL = "INSERT INTO tblRoom  (Room,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
     "SELECT @ID = SCOPE_IDENTITY(); ";
            else if (TableName.ToUpper() == Fields.TaxIncentive)
               sSQL = "INSERT INTO tblTaxIncentive  (TaxIncentive,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
     "SELECT @ID = SCOPE_IDENTITY(); ";
            else if (TableName.ToUpper() == Fields.Department)
               sSQL = "INSERT INTO tblDepartment  (Department,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
    "SELECT @ID = SCOPE_IDENTITY(); ";
            else if (TableName.ToUpper() == Fields.AssetClass)
               sSQL = "INSERT INTO tblAssetClass  (AssetClass,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
    "SELECT @ID = SCOPE_IDENTITY(); ";

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter { ParameterName = "@ID", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Output });
            oParameters.Add(new SqlParameter("@Name", poRecord.Name));
            oParameters.Add(new SqlParameter("@Description", poRecord.Description == null ? " " : poRecord.Description));
            oParameters.Add(new SqlParameter("@IsActive", true));
            oParameters.Add(new SqlParameter("@CreatedBy", UserID));
            oParameters.Add(new SqlParameter("@UpdatedBy", UserID));
            oParameters.Add(new SqlParameter("@CreatedDate", DateTime.Now));
            oParameters.Add(new SqlParameter("@UpdatedDate", DateTime.Now));
            oParameters.Add(new SqlParameter("@LogQuery", poRecord.LogQuery == null ? " " : poRecord.LogQuery));

            SqlParameter[] vSqlParameter = oParameters.ToArray();

            DatabaseContext oRemoteDB = new DatabaseContext();
            int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

            if (nReturn == 1)
            {
               poRecord.ID = Convert.ToInt64(vSqlParameter[0].Value);
            }
            else
            {
               // ErrorMessage = "Failed to insert record!";
               bReturn = false;

            }
         }
         catch (Exception exc)
         {
            ErrorMessage = exc.Message;
            GlobalFunction.SendErrorToText(exc);
            bReturn = false;
         }
         //------------------------------
         return bReturn;
      }

      public bool InsertMapping(DepMappingDataView poRecord, string TableName)
      {
         bool bReturn = true;
         //------------------------------
         try
         {
            poRecord.RecordID = 0;
            string sSQL = "";

            long UserID = Convert.ToInt64(System.Web.HttpContext.Current.Session["UserGUID"]);
            if (TableName.ToUpper() == "DEPARTMENTMAPPING")
               sSQL = "INSERT INTO tblDepartmentMapping  (DepartmentID,CostCenter_ID,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery)  VALUES (@DepartmentID,@CostCenterID,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery) ;" +
    "SELECT @RecordID = SCOPE_IDENTITY(); ";

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter { ParameterName = "@RecordID", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Output });
            oParameters.Add(new SqlParameter("@DepartmentID", poRecord.DepartmentID));
            oParameters.Add(new SqlParameter("@CostCenterID", poRecord.CostCenterID));
            oParameters.Add(new SqlParameter("@IsActive", true));
            oParameters.Add(new SqlParameter("@CreatedBy", UserID));
            oParameters.Add(new SqlParameter("@UpdatedBy", UserID));
            oParameters.Add(new SqlParameter("@CreatedDate", DateTime.Now));
            oParameters.Add(new SqlParameter("@UpdatedDate", DateTime.Now));
            oParameters.Add(new SqlParameter("@LogQuery", poRecord.LogQuery == null ? " " : poRecord.LogQuery));

            SqlParameter[] vSqlParameter = oParameters.ToArray();

            DatabaseContext oRemoteDB = new DatabaseContext();
            int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

            if (nReturn == 1)
            {
               poRecord.RecordID = Convert.ToInt64(vSqlParameter[0].Value);
            }
            else
            {
               // ErrorMessage = "Failed to insert record!";
               bReturn = false;

            }
         }
         catch (Exception exc)
         {
            ErrorMessage = exc.Message;
            GlobalFunction.SendErrorToText(exc);
            bReturn = false;
         }
         //------------------------------
         return bReturn;
      }

      public bool Update(Data poRecord,string TableName)
      {
         bool bReturn = true;

         DatabaseContext oRemoteDB = new DatabaseContext();
         var oTransaction = oRemoteDB.Database.BeginTransaction();
         try
         {
                #region General Table Data Message From XML
                ErrorMessageModel Model = new ErrorMessageModel();

                GeneralTableMsg Fields = Model.GeneralTableMsgSelect();
                #endregion

                long UserID = Convert.ToInt64(System.Web.HttpContext.Current.Session["UserGUID"]);
            string sSQL = "";

            if (TableName.ToUpper() == Fields.AssetPIC)
               sSQL = "UPDATE tblAssetPIC SET  AssetPIC = @Name,Description = @Description,UpdatedBy = @UpdatedBy,UpdatedDate = @UpdatedDate WHERE (AssetPIC_ID = @ID) ;";
            else if (TableName.ToUpper() == Fields.CostCenter)
               sSQL = "UPDATE tblCostCenter SET  CostCenter = @Name,Description = @Description,UpdatedBy = @UpdatedBy,UpdatedDate = @UpdatedDate WHERE (CostCenter_ID = @ID) ;";
            else if (TableName.ToUpper() == Fields.PAR)
               sSQL = "UPDATE tblPAR SET  PAR = @Name,Description = @Description,UpdatedBy = @UpdatedBy,UpdatedDate = @UpdatedDate WHERE (PAR_ID = @ID) ;";
            else if (TableName.ToUpper() == Fields.Responser)
               sSQL = "UPDATE tblResponser SET  Responser = @Name,Description = @Description,UpdatedBy = @UpdatedBy,UpdatedDate = @UpdatedDate WHERE (Responser_ID = @ID) ;";        
            else if (TableName.ToUpper() == Fields.ActivityType)
               sSQL = "UPDATE tblActivityType SET  ActivityType = @Name,Description = @Description,UpdatedBy = @UpdatedBy,UpdatedDate = @UpdatedDate WHERE (ActivityType_ID = @ID) ;";
            else if (TableName.ToUpper() == Fields.VendorCode)
               sSQL = "UPDATE tblVendorCode SET  VendorCode = @Name,Description = @Description,UpdatedBy = @UpdatedBy,UpdatedDate = @UpdatedDate WHERE (VendorCode_ID = @ID) ;";
            else if (TableName.ToUpper() == Fields.CustodyPerson || TableName.ToUpper() == Fields.CustodyPeron)
               sSQL = "UPDATE tblCustodyPerson SET  CustodyPerson = @Name,Description = @Description,UpdatedBy = @UpdatedBy,UpdatedDate = @UpdatedDate WHERE (CustodyPerson_ID = @ID) ;";
            else if (TableName.ToUpper() == Fields.Location)
               sSQL = "UPDATE tblLocation SET  Location = @Name,Description = @Description,UpdatedBy = @UpdatedBy,UpdatedDate = @UpdatedDate WHERE (Location_ID = @ID) ;";
            else if (TableName.ToUpper() == Fields.Room)
               sSQL = "UPDATE tblRoom SET  Room = @Name,Description = @Description,UpdatedBy = @UpdatedBy,UpdatedDate = @UpdatedDate WHERE (Room_ID = @ID) ;";
            else if (TableName.ToUpper() == Fields.TaxIncentive)
               sSQL = "UPDATE tblTaxIncentive SET  TaxIncentive = @Name,Description = @Description,UpdatedBy = @UpdatedBy,UpdatedDate = @UpdatedDate WHERE (TaxIncentive_ID = @ID) ;";
            else if (TableName.ToUpper() == Fields.Department)
               sSQL = "UPDATE tblDepartment SET  Department = @Name,Description = @Description,UpdatedBy = @UpdatedBy,UpdatedDate = @UpdatedDate WHERE (DepartmentID = @ID) ;";
            else if (TableName.ToUpper() == Fields.AssetClass)
               sSQL = "UPDATE tblAssetClass SET  AssetClass = @Name,Description = @Description,UpdatedBy = @UpdatedBy,UpdatedDate = @UpdatedDate WHERE (AssetClass_ID = @ID) ;";

            List<SqlParameter> oParameters = new List<SqlParameter>();

            oParameters.Add(new SqlParameter("@ID", poRecord.ID));
            oParameters.Add(new SqlParameter("@Name", poRecord.Name));
            oParameters.Add(new SqlParameter("@Description", poRecord.Description == null ? " " : poRecord.Description));
            oParameters.Add(new SqlParameter("@UpdatedDate", DateTime.Now));
            oParameters.Add(new SqlParameter("@UpdatedBy", UserID));

            SqlParameter[] vSqlParameter = oParameters.ToArray();

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
         catch (Exception exc)
         {
            ErrorMessage = exc.Message;
            GlobalFunction.SendErrorToText(exc);
            bReturn = false;
         }
         //------------------------------
         return bReturn;
      }

      public bool UpdateMapping(DepMappingDataView poRecord, string TableName)
      {
         bool bReturn = true;

         DatabaseContext oRemoteDB = new DatabaseContext();
         var oTransaction = oRemoteDB.Database.BeginTransaction();
         try
         {
            string sSQL = "";
            long UserID = Convert.ToInt64(System.Web.HttpContext.Current.Session["UserGUID"]);

            if (TableName.ToUpper() == "DEPARTMENTMAPPING")
               sSQL = "UPDATE tblDepartmentMapping SET DepartmentID = @DepartmentID,CostCenter_ID = @CostCenterID,UpdatedBy = @UpdatedBy,UpdatedDate = @UpdatedDate WHERE (RecordID = @RecordID) ;";

            List<SqlParameter> oParameters = new List<SqlParameter>();

            oParameters.Add(new SqlParameter("@RecordID", poRecord.RecordID));
            oParameters.Add(new SqlParameter("@DepartmentID", poRecord.DepartmentID));
            oParameters.Add(new SqlParameter("@CostCenterID", poRecord.CostCenterID));
            oParameters.Add(new SqlParameter("@UpdatedDate", DateTime.Now));
            oParameters.Add(new SqlParameter("@UpdatedBy", UserID));

            SqlParameter[] vSqlParameter = oParameters.ToArray();

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
         catch (Exception exc)
         {
            ErrorMessage = exc.Message;
            GlobalFunction.SendErrorToText(exc);
            bReturn = false;
         }
         //------------------------------
         return bReturn;
      }

      public bool Delete(Data poRecord,string TableName)
      {
         bool bReturn = true;
         try
         {
                #region General Table Data Message From XML
                ErrorMessageModel Model = new ErrorMessageModel();

                GeneralTableMsg Fields = Model.GeneralTableMsgSelect();
                #endregion

                string sSQL = "";

            if (TableName.ToUpper() == Fields.AssetPIC)
               sSQL = "DELETE tblAssetPIC  WHERE (AssetPIC_ID = @ID);";
            else if (TableName.ToUpper() == Fields.CostCenter)
               sSQL = "DELETE tblCostCenter  WHERE (CostCenter_ID = @ID);";
            else if (TableName.ToUpper() == Fields.PAR)
               sSQL = "DELETE tblPAR  WHERE (PAR_ID = @ID);";
            else if (TableName.ToUpper() == Fields.Responser)
            sSQL = "DELETE tblResponser  WHERE (Responser_ID = @ID);";
            else if (TableName.ToUpper() == Fields.AssetClass)
               sSQL = "DELETE tblAssetClass  WHERE (AssetClass_ID = @ID);";
            else if (TableName.ToUpper() == Fields.ActivityType)
               sSQL = "DELETE tblActivityType  WHERE (ActivityType_ID = @ID);";
            else if (TableName.ToUpper() == Fields.VendorCode)
               sSQL = "DELETE tblVendorCode  WHERE (VendorCode_ID = @ID);";
            else if (TableName.ToUpper() == Fields.CustodyPerson || TableName.ToUpper() == Fields.CustodyPeron)
               sSQL = "DELETE tblCustodyPerson  WHERE (CustodyPerson_ID = @ID);";
            else if (TableName.ToUpper() == Fields.Location)
               sSQL = "DELETE tblLocation  WHERE (Location_ID = @ID);";
            else if (TableName.ToUpper() == Fields.Room)
               sSQL = "DELETE tblRoom  WHERE (Room_ID = @ID);";
            else if (TableName.ToUpper() == Fields.TaxIncentive)
               sSQL = "DELETE tblTaxIncentive  WHERE (TaxIncentive_ID = @ID);";
            else if (TableName.ToUpper() == Fields.Department)
               sSQL = "DELETE tblDepartment  WHERE (DepartmentID = @ID);";

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@ID", poRecord.ID));

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
               ErrorMessage = "Failed to delete record!";

               bReturn = false;
            }
         }
         catch (Exception exc)
         {
            ErrorMessage = exc.Message;
            GlobalFunction.SendErrorToText(exc);
            bReturn = false;
         }
         //------------------------------
         return bReturn;
      }

      public bool DeleteMapping(DepMappingDataView poRecord, string TableName)
      {
         bool bReturn = true;
         try
         {
            string sSQL = "";
            if (TableName.ToUpper() == "DEPARTMENTMAPPING")
               sSQL = "DELETE tblDepartmentMapping  WHERE (RecordID = @ID);";

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@ID", poRecord.RecordID));

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
               ErrorMessage = "Failed to delete record!";

               bReturn = false;
            }
         }
         catch (Exception exc)
         {
            ErrorMessage = exc.Message;
            GlobalFunction.SendErrorToText(exc);
            bReturn = false;
         }
         //------------------------------
         return bReturn;
      }

      public bool InsertImportGeneralTable(Data poRecord, string TableName, out long IdRecord)
      {
         bool bReturn = true;
         IdRecord = 0;
         //------------------------------
         try
         {
            poRecord.ID = 0;
            string sSQL = "";

                #region General Table Data Message From XML
                ErrorMessageModel Model = new ErrorMessageModel();

                GeneralTableMsg Fields = Model.GeneralTableMsgSelect();
                #endregion

                long UserID = Convert.ToInt64(System.Web.HttpContext.Current.Session["UserGUID"]);

            if (TableName.ToUpper() == Fields.AssetPIC)
               sSQL = "INSERT INTO tblAssetPIC  (AssetPIC,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
     "SELECT @ID = SCOPE_IDENTITY(); ";
            else if (TableName.ToUpper() == Fields.CostCenter)
               sSQL = "INSERT INTO tblCostCenter  (CostCenter,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
     "SELECT @ID = SCOPE_IDENTITY(); ";
            else if (TableName.ToUpper() == Fields.PAR)
               sSQL = "INSERT INTO tblPAR  (PAR,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
    "SELECT @ID = SCOPE_IDENTITY(); ";
            else if (TableName.ToUpper() == Fields.Responser)
               sSQL = "INSERT INTO tblResponser  (Responser,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
     "SELECT @ID = SCOPE_IDENTITY(); ";
            else if (TableName.ToUpper() == Fields.ActivityType)
               sSQL = "INSERT INTO tblActivityType  (ActivityType,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
    "SELECT @ID = SCOPE_IDENTITY(); ";
            else if (TableName.ToUpper() == Fields.VendorCode)
               sSQL = "INSERT INTO tblVendorCode  (VendorCode,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
    "SELECT @ID = SCOPE_IDENTITY(); ";
            else if (TableName.ToUpper() == Fields.CustodyPerson || TableName.ToUpper() == Fields.CustodyPeron)
               sSQL = "INSERT INTO tblCustodyPerson  (CustodyPerson,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
     "SELECT @ID = SCOPE_IDENTITY(); ";
            else if (TableName.ToUpper() == Fields.Location)
               sSQL = "INSERT INTO tblLocation  (Location,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
     "SELECT @ID = SCOPE_IDENTITY(); ";
            else if (TableName.ToUpper() == Fields.Room)
               sSQL = "INSERT INTO tblRoom  (Room,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
     "SELECT @ID = SCOPE_IDENTITY(); ";
            else if (TableName.ToUpper() == Fields.TaxIncentive)
               sSQL = "INSERT INTO tblTaxIncentive  (TaxIncentive,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
     "SELECT @ID = SCOPE_IDENTITY(); ";
    //        else if (TableName.ToUpper() == Fields.Department)
    //           sSQL = "INSERT INTO tblDepartment  (Department,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
    //"SELECT @ID = SCOPE_IDENTITY(); ";
            else if (TableName.ToUpper() == Fields.AssetClass)
               sSQL = "INSERT INTO tblAssetClass  (AssetClass,IsActive,CreatedBy,UpdatedBy,CreatedDate,UpdatedDate,LogQuery,Description)  VALUES (@Name,@IsActive,@CreatedBy, @UpdatedBy, @CreatedDate, @UpdatedDate,@LogQuery,@Description) ;" +
    "SELECT @ID = SCOPE_IDENTITY(); ";

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter { ParameterName = "@ID", SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Output });
            oParameters.Add(new SqlParameter("@Name", poRecord.Name));
            oParameters.Add(new SqlParameter("@Description", poRecord.Description == null ? " " : poRecord.Description));
            oParameters.Add(new SqlParameter("@IsActive", true));
            oParameters.Add(new SqlParameter("@CreatedBy", UserID));
            oParameters.Add(new SqlParameter("@UpdatedBy", UserID));
            oParameters.Add(new SqlParameter("@CreatedDate", DateTime.Now));
            oParameters.Add(new SqlParameter("@UpdatedDate", DateTime.Now));
            oParameters.Add(new SqlParameter("@LogQuery", poRecord.LogQuery == null ? " " : poRecord.LogQuery));

            SqlParameter[] vSqlParameter = oParameters.ToArray();

            DatabaseContext oRemoteDB = new DatabaseContext();
            int nReturn = oRemoteDB.Database.ExecuteSqlCommand(sSQL, vSqlParameter);

            if (nReturn == 1)
            {
               poRecord.ID = Convert.ToInt64(vSqlParameter[0].Value);
               IdRecord = Convert.ToInt64(vSqlParameter[0].Value);
            }
            else
            {
               // ErrorMessage = "Failed to insert record!";
               bReturn = false;

            }
         }
         catch (Exception exc)
         {
            ErrorMessage = exc.Message;
            GlobalFunction.SendErrorToText(exc);
            bReturn = false;
         }
         //------------------------------
         return bReturn;
      }

      public bool AutoDeleteGeneralTable(Data poRecord, string TableName)
      {
         bool bReturn = true;
         try
         {
            string sSQL = "";

                #region General Table Data Message From XML
                ErrorMessageModel Model = new ErrorMessageModel();

                GeneralTableMsg Fields = Model.GeneralTableMsgSelect();
                #endregion

                if (TableName.ToUpper() == Fields.AssetPIC)
               sSQL = "DELETE tblAssetPIC  WHERE (AssetPIC = @Name);";
            else if (TableName.ToUpper() == Fields.CostCenter)
               sSQL = "DELETE tblCostCenter  WHERE (CostCenter = @Name);";
            else if (TableName.ToUpper() == Fields.PAR)
               sSQL = "DELETE tblPAR  WHERE (PAR = @Name);";
            else if (TableName.ToUpper() == Fields.Responser)
               sSQL = "DELETE tblResponser  WHERE (Responser = @Name);";
            else if (TableName.ToUpper() == Fields.AssetClass)
               sSQL = "DELETE tblAssetClass  WHERE (AssetClass = @Name);";
            else if (TableName.ToUpper() == Fields.ActivityType)
               sSQL = "DELETE tblActivityType  WHERE (ActivityType = @Name);";
            else if (TableName.ToUpper() == Fields.VendorCode)
               sSQL = "DELETE tblVendorCode  WHERE (VendorCode = @Name);";
            else if (TableName.ToUpper() == Fields.CustodyPerson || TableName.ToUpper() == Fields.CustodyPeron)
               sSQL = "DELETE tblCustodyPerson  WHERE (CustodyPerson = @Name);";
            else if (TableName.ToUpper() == Fields.Location)
               sSQL = "DELETE tblLocation  WHERE (Location = @Name);";
            else if (TableName.ToUpper() == Fields.Room)
               sSQL = "DELETE tblRoom  WHERE (Room = @Name);";
            else if (TableName.ToUpper() == Fields.TaxIncentive)
               sSQL = "DELETE tblTaxIncentive  WHERE (TaxIncentive = @Name);";

            List<SqlParameter> oParameters = new List<SqlParameter>();
            oParameters.Add(new SqlParameter("@Name", poRecord.Name));

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
               ErrorMessage = "Failed to delete record!";

               bReturn = false;
            }
         }
         catch (Exception exc)
         {
            ErrorMessage = exc.Message;
            GlobalFunction.SendErrorToText(exc);
            bReturn = false;
         }
         //------------------------------
         return bReturn;
      }

   }
}