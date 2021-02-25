using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using EagleServicesWebApp.Models.GeneralTableModel;
using EagleServicesWebApp.Models;


namespace EagleServicesWebApp.Models
{
    public class CommonRoleAccessModel
    {
        public bool IsAllowAdd { get; set; } = false;
        public bool IsAllowEdit { get; set; } = false;
        public bool IsAllowDelete { get; set; } = false;
    }

    public class RolePermissionModel
    {
        public int ErrorNo { get; set; }
        public string ErrorMessage { get; set; }

        public RolePermissionModel()
        {
            ErrorNo = 0;
            ErrorMessage = "";
        }

        #region Select 
        public System.Data.Entity.Infrastructure.DbRawSqlQuery<CommonRoleAccessModel> GetMenuPermissionByMenuName(string _RoleName,string MenuName)
        {
            DatabaseContext oRemoteDB = new DatabaseContext();
            string sSQL = string.Empty;

            //select IsAllowAdd,IsAllowEdit,IsAllowDelete from Menu_vw where RoleName='Administrator' and MenuName='AssetEnquiry'
            sSQL = " SELECT Distinct IsAllowAdd,IsAllowEdit,IsAllowDelete FROM " +
                       " Menu_vw WHERE  Status = 1 AND " +
                       " RoleName= '" + _RoleName + "' AND MenuName='"+ MenuName + "' AND MenuIcon is null;";
            ;

            var vQuery = oRemoteDB.Database.SqlQuery<CommonRoleAccessModel>(sSQL);

            return vQuery;
        }
        #endregion

    }


}