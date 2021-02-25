using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using EagleServicesWebApp.Models.GeneralTableModel;
using EagleServicesWebApp.Models;

namespace EagleServicesWebApp.Models
{
    public class Menu_REC
    {
        public long RecordID { get; set; }

        public long RoleID { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public bool RecordStatus { get; set; }
        public int RecordFlag { get; set; }
        //------------------------------
        [Key]
        public string MenuID { get; set; }
        public string MenuName { get; set; }
        public string Description { get; set; }
        public string MenuIcon { get; set; }
        public bool Status { get; set; }
        public int Ordering { get; set; }

        
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class MenuModel
    {
        public int ErrorNo { get; set; }
        public string ErrorMessage { get; set; }

        public MenuModel()
        {
            ErrorNo = 0;
            ErrorMessage = "";
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<Menu_REC> DashboardMenuAccessByRoleID(long _RoleID)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();

            string sSQL = "" +
                "SELECT " +
                "      distinct MenuName from Menu_vw  with (nolock) " +
                "      where RoleID = " + _RoleID +
                "   and MenuName in ('AssetTagging', 'DataImport', 'Enquiry', 'GeneralTable', 'User') " +
                ";"
                ;

            var vQuery = oRemoteDB.Database.SqlQuery<Menu_REC>(sSQL);

            return vQuery;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<Menu_REC> GetMainMenu()
        {
            DatabaseContext oRemoteDB = new DatabaseContext();

            string sSQL = "" +
                "SELECT " +
                "   Distinct  SUBSTRING(MenuID, 1, 2) as MenuID, MenuName,Description,MenuIcon,Ordering " +
                "   FROM tblMenu  with (nolock) " +
                "   WHERE Status = 1   and MenuIcon<> '' " +
                "   ORDER BY MenuID " +
                ";"
                ;

            var vQuery = oRemoteDB.Database.SqlQuery<Menu_REC>(sSQL);

            return vQuery;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<Menu_REC> GetMainMenuIDRoleID(string _RoleName)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = string.Empty;
            string UserID = Convert.ToString(System.Web.HttpContext.Current.Session["UserID"]);


            sSQL = " SELECT Distinct MenuID,MenuName,Description,MenuIcon,Ordering FROM " +
                       " Menu_vw WHERE  Status = 1 AND " +
                       " RoleName= '" + _RoleName + "';";
            ;

            var vQuery = oRemoteDB.Database.SqlQuery<Menu_REC>(sSQL);

            return vQuery;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<Menu_REC> GetList()
        {
            DatabaseContext oRemoteDB = new DatabaseContext();

            string sSQL = "" +
                "SELECT " +
                "      MenuID,MenuName,Description,MenuIcon,Ordering " +
                "   FROM tblMenu  with (nolock) " +
				"   ORDER BY MenuID " +
                ";"
                ;

            var vQuery = oRemoteDB.Database.SqlQuery<Menu_REC>(sSQL);

            return vQuery;
        }

        public System.Data.Entity.Infrastructure.DbRawSqlQuery<FindDataByID> NotMappingCostCenterCount()
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = "";
            sSQL = "select count(*) As  Rcount from tblCostCenter with(nolock) where CostCenter_ID not in (Select CostCenter_ID from tblDepartmentMapping with(nolock))";

            var vQuery = oRemoteDB.Database.SqlQuery<FindDataByID>(sSQL);
            return vQuery;
        }
    }


}